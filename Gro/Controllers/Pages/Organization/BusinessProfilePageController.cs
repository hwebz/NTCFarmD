using System.Collections.Generic;
using System.Linq;
using Gro.Core.ContentTypes.Pages.Organization;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Pages.StartPages;
using Gro.Core.DataModels.Organization;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.Organization;

namespace Gro.Controllers.Pages.Organization
{
    public class BusinessProfilePageController : SiteControllerBase<BusinessProfilePage>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IContentRepository _contentRepo;

        public BusinessProfilePageController(
            IUserManagementService userManager,
            IOrganizationRepository organizationRepository,
            IContentRepository contentRepo) : base(userManager)
        {
            _organizationRepository = organizationRepository;
            _contentRepo = contentRepo;
        }

        public ActionResult Index(BusinessProfilePage currentPage)
        {
            if (SiteUser == null)
            {
                return View("~/Views/Organization/BusinessProfile.cshtml", new BusinessProfileViewModel(currentPage));
            }

            var organization = UserManager.GetActiveCustomer(HttpContext);
            if (organization == null)
            {
                return View("~/Views/Organization/BusinessProfile.cshtml", new BusinessProfileViewModel(currentPage));
            }

            var businessProfile = _organizationRepository.GetBusinessProfile(organization.CustomerId) ?? new BusinessProfile();
            var viewModel = new BusinessProfileViewModel(currentPage)
            {
                BusinessProfile = new BusinessProfileModel
                {
                    Id = businessProfile.Id,
                    Name = businessProfile.Name,
                    Rows = businessProfile.Rows?.ToList() ?? new List<ProfileRow>()
                },
                IsLoginBankId = !string.IsNullOrEmpty(SiteUser.SerialNumber)
            };
            return View("~/Views/Organization/BusinessProfile.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Index(BusinessProfilePage currentPage, BusinessProfileModel businessProfile)
        {
            if (string.IsNullOrEmpty(SiteUser?.SerialNumber)) return HttpNotFound();

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                ViewBag.Message = "Some fields have invalid value. Please try again!";
                return View("~/Views/Organization/BusinessProfile.cshtml",
                    new BusinessProfileViewModel(currentPage)
                    {
                        BusinessProfile = businessProfile,
                        IsLoginBankId = !string.IsNullOrEmpty(SiteUser.SerialNumber)
                    });
            }

            var customer = UserManager.GetActiveCustomer(HttpContext);
            if (customer != null)
            {
                var result = _organizationRepository.UpdateBusinessProfile(new BusinessProfile
                {
                    CustomerId = customer.CustomerId,
                    Id = businessProfile.Id,
                    Name = businessProfile.Name,
                    Rows = businessProfile.Rows.ToArray()
                }, customer.CustomerNo);

                if (result)
                {
                    var startPage = _contentRepo.Get<StartPage>(ContentReference.StartPage);
                    if (startPage != null)
                    {
                        if (!PageReference.IsNullOrEmpty(startPage.SettingsPage))
                        {
                            var settingPage = _contentRepo.Get<SettingsPage>(startPage.SettingsPage);
                            if (settingPage != null)
                            {
                                //TempData["UpdateInfoSuccess"] = true;
                                TempData["UpdateBusinessProfileSuccess"] = true;
                                return RedirectToAction("Index", new { node = settingPage.MyAccountLink });
                            }
                        }
                    }
                }
            }

            var viewModel = new BusinessProfileViewModel(currentPage)
            {
                BusinessProfile = businessProfile,
                IsLoginBankId = !string.IsNullOrEmpty(SiteUser.SerialNumber)
            };
            ViewBag.Message = "Uppdateringen lyckades inte. Försök igen!";
            return View("~/Views/Organization/BusinessProfile.cshtml", viewModel);
        }
    }
}
