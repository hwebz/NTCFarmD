using EPiServer;
using EPiServer.Core;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Pages.Organization;
using Gro.Core.ContentTypes.Pages.StartPages;
using Gro.ViewModels;
using System.Web.Mvc;

namespace Gro.Controllers.Pages.Organization
{
    public class ProfileInformationPageController : SiteControllerBase<ProfileInformationPage>
    {
        private readonly IContentRepository _contentRepo;

        public ProfileInformationPageController(IUserManagementService userManager, IContentRepository contentRepo) : base(userManager)
        {
            _contentRepo = contentRepo;
        }

        [HttpGet]
        public ActionResult Index(ProfileInformationPage currentPage)
        {
            // store roles and profiles in ViewData
            var viewModel = new PageViewModel<ProfileInformationPage>(currentPage);
            var startPage = _contentRepo.Get<StartPage>(ContentReference.StartPage);
            var settingPage = _contentRepo.Get<SettingsPage>(startPage.SettingsPage);
            ViewData["myAccountLink"] = settingPage.MyAccountLink;
            return View("~/Views/Organization/ProfileInformationPage.cshtml", viewModel);
        }
    }
}
