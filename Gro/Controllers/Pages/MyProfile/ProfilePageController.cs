using System.Web.Mvc;
using Gro.Core.ContentTypes.Pages.MyProfile;
using Gro.Core.Interfaces;
using System.Threading.Tasks;
using Gro.ViewModels.Users;
using Gro.Business.Services.Users;
using Gro.ViewModels.Pages.MyProfile;
using Gro.Helpers;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Controllers.Pages.MyProfile
{
    public class ProfilePageController : SiteControllerBase<ProfilePage>
    {
        private readonly ISecurityRepository _securityRepository;

        public ProfilePageController(ISecurityRepository securityRepository, IUserManagementService userManager) : base(userManager)
        {
            _securityRepository = securityRepository;
        }

        // GET: UserMessage
        [HttpGet]
        public async Task<ActionResult> Index(ProfilePage currentPage)
        {
            if (SiteUser == null)
            {
                return View("~/Views/MyProfile/ProfilePage.cshtml", new ProfilePageViewModel(currentPage, new EditProfileViewModel()));
            }

            var dbUser = await _securityRepository.QueryUserAsync(SiteUser.UserName);
            if (dbUser == null) return HttpNotFound();

            var editViewModel = new EditProfileViewModel
            {
                FirstName = dbUser.FirstName,
                LastName = dbUser.LastName,
                City = dbUser.City,
                Street = dbUser.Street,
                Email = dbUser.Email,
                Zip = dbUser.Zip,
                Mobilephone = dbUser.PhoneMobile,
                Telephone = dbUser.PhoneWork
            };

            return View("~/Views/MyProfile/ProfilePage.cshtml", new ProfilePageViewModel(currentPage, editViewModel));
        }

        [HttpPost]
        public async Task<ActionResult> Index(ProfilePage currentPage, EditProfileViewModel editViewModel)
        {
            if (SiteUser == null) return HttpNotFound();

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return View("~/Views/MyProfile/ProfilePage.cshtml", new ProfilePageViewModel(currentPage, editViewModel));
            }

            await UserManager.UpdateUserInfoAsync(SiteUser.PersonDn, editViewModel.FirstName, editViewModel.LastName, editViewModel.Telephone,
                editViewModel.Mobilephone, editViewModel.Email, editViewModel.Street, editViewModel.Zip, editViewModel.City);

            var newSiteUser = await UserManager.QuerySiteUserAsync(SiteUser.UserName);
            this.SetUserSession(newSiteUser);

            var settingPage = ContentExtensions.GetSettingsPage();
            TempData["UpdateInfoSuccess"] = true;

            return RedirectToAction("Index", new { node = settingPage.MyAccountLink });
        }
    }
}
