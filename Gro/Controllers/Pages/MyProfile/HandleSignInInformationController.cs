using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Pages.MyProfile;
using Gro.Core.ContentTypes.Pages.StartPages;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.MyProfile;

namespace Gro.Controllers.Pages.MyProfile
{
    //[CustomerRole]
    public class HandleSignInInformationController : SiteControllerBase<HandleSignInInformationPage>
    {
        private readonly ISecurityRepository _securityRepository;
        private readonly IContentRepository _contentRepo;

        public HandleSignInInformationController(
            ISecurityRepository securityRepository, IContentRepository contentRepo,
            IUserManagementService userManager) : base(userManager)
        {
            _securityRepository = securityRepository;
            _contentRepo = contentRepo;
        }

        public ActionResult Index(HandleSignInInformationPage currentPage)
        {
            if (SiteUser == null)
            {
                return View("~/Views/MyProfile/HandleSignInInformation.cshtml",
                    new HandleSigInInformationViewModel(currentPage));
            }

            var personNumber = _securityRepository.GetSocialSecurityNumber(SiteUser.UserName);
            var model = new HandleSigInInformationViewModel(currentPage)
            {
                SocialSecurityNumber = personNumber
            };
            return View("~/Views/MyProfile/HandleSignInInformation.cshtml", model);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateSocialSecurityNumber(HandleSignInInformationPage currentPage,
            FormCollection collection)
        {
            if (SiteUser == null)
            {
                return HttpNotFound();
            }

            var socialSecurityNumber = collection["SocialSecurityNumber"];
            var regex = new Regex(@"^([0-9]{12})$");
            if (!regex.IsMatch(socialSecurityNumber))
            {
                TempData["SocialNumberInvalid"] = "Den personnummer är inte giltigt.";
            }
            else
            {
                var result = await _securityRepository.UpdateSocialSecurityNumberAsync(SiteUser.UserName, socialSecurityNumber);
                if (result)
                {
                    //var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
                    var startPage = _contentRepo.Get<StartPage>(ContentReference.StartPage);
                    if (startPage == null) return RedirectToAction("Index", new {node = currentPage.ContentLink});
                    if (PageReference.IsNullOrEmpty(startPage.SettingsPage))
                    {
                        return RedirectToAction("Index", new {node = currentPage.ContentLink});
                    }

                    var settingPage = _contentRepo.Get<SettingsPage>(startPage.SettingsPage);

                    if (settingPage == null)
                    {
                        return RedirectToAction("Index", new {node = currentPage.ContentLink});
                    }

                    TempData["UpdatePersonNumberSuccess"] = true;
                    return RedirectToAction("Index", new {node = settingPage.MyAccountLink});
                }

                TempData["SocialNumberInvalid"] = "Det går inte att uppdatera personnummer. Var god försök igen!";
            }

            return RedirectToAction("Index", new {node = currentPage.ContentLink});
        }
    }
}
