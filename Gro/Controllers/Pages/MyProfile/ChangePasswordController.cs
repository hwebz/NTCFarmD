using System.Threading.Tasks;
using System.Web.Mvc;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.MyProfile;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.Interfaces;
using Gro.ViewModels;
using System.ServiceModel;

namespace Gro.Controllers.Pages.MyProfile
{
    public class ChangePasswordController : SiteControllerBase<ChangePasswordPage>
    {
        private readonly ISecurityRepository _securityRepository;

        public ChangePasswordController(ISecurityRepository securityRepository, IUserManagementService userManager) : base(userManager)
        {
            _securityRepository = securityRepository;
        }

        public ActionResult Index(ChangePasswordPage currentPage)
        {
            var model = new PageViewModel<ChangePasswordPage>(currentPage);
            return View("~/Views/MyProfile/ChangePassword.cshtml", model);
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordPage currentPage, string oldPassword, string newPassword, string retypeNewPassword)
        {
            var message = string.Empty;
            var incorrectPasswordMessage = string.Empty;

            var isOldPasswordCorrect = _securityRepository.ValidateUser(SiteUser.UserName, oldPassword);

            if (!isOldPasswordCorrect)
            {
                incorrectPasswordMessage = "Lösenordet du angav stämmer inte överens med ditt nuvarande lösenord.";
            }
            else if (!newPassword.Equals(retypeNewPassword))
            {
                message = "Detta lösenord stämmer inte överens med det första du angav.";
            }
            else if (!UserManager.IsUserPasswordValid(newPassword))
            {
                message = "Lösenordet är inte av rätt typ. Se krav för nytt lösenord.";
            }
            else
            {
                try
                {
                    await _securityRepository.ChangePasswordAsync(SiteUser.UserName, newPassword);

                    var settingPage = ContentExtensions.GetSettingsPage();
                    TempData["UpdatePasswordSuccess"] = true;
                    return RedirectToAction("Index", new { node = settingPage.MyAccountLink });
                }
                catch (FaultException)
                {
                    message = "Lösenordet är inte av rätt typ. Se krav för nytt lösenord";
                }
            }

            ViewData["message"] = message;
            ViewData["incorrectPasswordMessage"] = incorrectPasswordMessage;

            return View("~/Views/MyProfile/ChangePassword.cshtml", new PageViewModel<ChangePasswordPage>(currentPage));
        }
    }
}
