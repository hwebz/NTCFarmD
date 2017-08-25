using System.Configuration;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Web.Mvc;
using Gro.Business;
using Gro.Business.Rendering;
using Gro.Business.Services.Users;
using Gro.Core.Interfaces;
using Gro.Helpers;
using Gro.Business.Caching;
using System;

namespace Gro.Controllers
{
    /// <summary>
    /// Controller base for pages in site with authentication
    /// </summary>
    [AgreementAccepted]
    [NoCache]
    public abstract class SiteControllerBase<T> : PageController<T> where T : PageData
    {
        protected readonly IUserManagementService UserManager;
        private readonly IOrganizationUserRepository _orgUserRepo = DependencyResolver.Current.GetService<IOrganizationUserRepository>();

        private SiteUser _siteUser;
        protected SiteUser SiteUser => _siteUser ?? (_siteUser = UserManager.GetSiteUser(HttpContext));

        protected SiteControllerBase()
        {
            UserManager = DependencyResolver.Current.GetService<IUserManagementService>();
        }

        protected SiteControllerBase(IUserManagementService userManager)
        {
            UserManager = userManager;
        }

        protected override void OnAuthorization(AuthorizationContext actionContext)
        {
            base.OnAuthorization(actionContext);
            //stop filtering if in edit mode
            if (PageEditing.PageIsInEditMode) return;

            if (!"XMLHttpRequest".Equals(actionContext.HttpContext.Request.Headers["X-Requested-With"], StringComparison.OrdinalIgnoreCase) &&
                    (SiteUser == null && string.IsNullOrWhiteSpace(actionContext.HttpContext.User?.Identity?.Name)))
            {
                actionContext.Result = Redirect(ConfigurationManager.AppSettings["loginUrl"]);
                return;
            }

            var currentPage = PageContext.Page;
            var canAccess = PageAccess.CanAccessPage(UserManager, _orgUserRepo, currentPage, actionContext.HttpContext);
            if (canAccess) return;

            actionContext.ReturnUnAuthorizedResult();
        }
    }
}
