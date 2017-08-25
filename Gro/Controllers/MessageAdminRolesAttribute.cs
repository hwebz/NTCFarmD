using Gro.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace Gro.Controllers
{
    /// <summary>
    /// Allow only users with webadmin roles to enter the message administration pages
    /// </summary>
    public class MessageAdminRolesAttribute : ActionFilterAttribute, IActionFilter
    {
        private const string WebAdminRole = "WebAdmins";

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var authorized = filterContext.HttpContext.User.Identity.IsAuthenticated;
            if (authorized)
            {
                var userName = filterContext.HttpContext.User.Identity.Name;
                authorized = Roles.IsUserInRole(userName, WebAdminRole);
            }

            if (!authorized)
            {
                filterContext.ReturnUnAuthorizedResult();
                return;
            }

            OnActionExecuting(filterContext);
        }
    }
}
