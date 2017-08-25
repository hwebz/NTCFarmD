using Gro.Controllers.Pages.MyProfile;
using Gro.Core.ContentTypes.Utils;
using Gro.Helpers;
using System.Web.Mvc;

namespace Gro.Controllers
{
    public class AgreementAcceptedAttribute : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var siteUser = filterContext.HttpContext.GetSiteUser();
            if (siteUser == null || filterContext.Controller is UserAgreementsPageController)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            var agreementPage = ContentExtensions.GetUserAgreementPage();
            if (agreementPage != null && siteUser.AcceptedAgreementVersion < agreementPage.Version)
            {
                var userAgreementRef = ContentExtensions.GetSettingsPage()?.UserAgreementPage;
                var userAgreementLink = LinkHelpers.GetFriendlyLinkOfPage(userAgreementRef);
                filterContext.Result = new RedirectResult(userAgreementLink);
                return;
                //filterContext.HttpContext.Response.Redirect(userAgreementLink, true);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
