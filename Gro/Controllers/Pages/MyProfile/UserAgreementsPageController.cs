using System.Web.Mvc;
using Gro.Core.ContentTypes.Pages.MyProfile;
using Gro.ViewModels;
using Gro.Business.Services.Users;
using Gro.Helpers;
using static System.Web.Security.FormsAuthentication;

namespace Gro.Controllers.Pages.MyProfile
{
    public class UserAgreementsPageController : SiteControllerBase<UserAgreementsPage>
    {
        public UserAgreementsPageController(IUserManagementService userManager) : base(userManager)
        {
        }

        public ActionResult Index(UserAgreementsPage currentPage)
        {
            return View("~/Views/MyProfile/UserAgreementsPage.cshtml", new PageViewModel<UserAgreementsPage>(currentPage));
        }

        [HttpPost]
        public ActionResult Index(UserAgreementsPage currentPage, int version, string logoutRedirect)
        {
            if (version < 0)
            {
                this.SignoutSiteUser();
                SignOut();
                return Redirect(logoutRedirect);
            }

            var updatedResult = UserManager.UpdateInsertUserAccepts(currentPage.TermId, version, SiteUser.UserId);
            if (!updatedResult)
            {
                return View("~/Views/MyProfile/UserAgreementsPage.cshtml", new PageViewModel<UserAgreementsPage>(currentPage));
            }

            SiteUser.AcceptedAgreementVersion = version;
            this.SetUserSession(SiteUser);
            return Redirect("/");
        }
    }
}
