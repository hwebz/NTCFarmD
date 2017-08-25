using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Editor;
using Gro.Business.Services.Users;
using Gro.Core.Interfaces;
using Gro.Helpers;

namespace Gro.Controllers
{
    /// <summary>
    /// Only allow a customer user with certain roles to access this page
    /// </summary>
    public class CustomerRoleAttribute : ActionFilterAttribute, IActionFilter
    {
        private readonly IUserManagementService _userManager;
        private readonly ISecurityRepository _securityRepo;
        private readonly List<string> _roles;

        public CustomerRoleAttribute(params string[] roles)
        {
            _roles = roles?.ToList();
            _userManager = DependencyResolver.Current.GetService<IUserManagementService>();
            _securityRepo = DependencyResolver.Current.GetService<ISecurityRepository>();
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (PageEditing.PageIsInEditMode
                || filterContext.ActionDescriptor.GetCustomAttributes(typeof(SkipRoleAttribute), false).Any())
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            var siteUser = _userManager.GetSiteUser(filterContext.HttpContext);
            var activeCustomer = _userManager.GetActiveCustomer(filterContext.HttpContext);

            if (siteUser == null || activeCustomer == null)
            {
                filterContext.ReturnUnAuthorizedResult();
                return;
            }

            if (_roles.Count == 0)
            {
                //all roles, pass
                OnActionExecuting(filterContext);
                return;
            }

            var userRoles = _securityRepo
                .GetRolesForUser(siteUser.UserName)
                .Where(ur => ur.CustomerId == activeCustomer.CustomerId && ur.Sysrole == false)
                .ToList();

            var access = userRoles.Any(ur => _roles.Contains(ur.RoleName));
            if (!access)
            {
                filterContext.ReturnUnAuthorizedResult();
                return;
            }

            OnActionExecuting(filterContext);
        }
    }

    public class SkipRoleAttribute : Attribute
    {
    }
}
