using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Pages.Organization;
using Gro.Core.ContentTypes.Pages.StartPages;
using Gro.Core.DataModels.Organization;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.Organization;

namespace Gro.Controllers.Pages.Organization
{
    //[OwnerCustomer]
    public class CompanyInformationPageController : SiteControllerBase<CompanyInformationPage>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IContentRepository _contentRepo;

        public CompanyInformationPageController(
            ISecurityRepository securityRepository,
            IUserManagementService userManager,
            IOrganizationRepository organizationRepository,
            IContentRepository contentRepo) : base(userManager)
        {
            _organizationRepository = organizationRepository;
            _contentRepo = contentRepo;
        }

        public ActionResult Index(CompanyInformationPage currentPage)
        {
            if (SiteUser == null)
            {
                return View("~/Views/Organization/CompanyInformation.cshtml", new CompanyInformationViewModel(currentPage));
            }

            var organization = UserManager.GetActiveCustomer(HttpContext);
            if (organization == null)
            {
                return View("~/Views/Organization/CompanyInformation.cshtml",
                    new CompanyInformationViewModel(currentPage));
            }

            var companyInformation = _organizationRepository.GetCompanyInformation(organization.CustomerId) ?? new OrganisationInformation();

            var viewModel = new CompanyInformationViewModel(currentPage)
            {
                CompanyInformation = new CompanyInformationModel
                {
                    Address = companyInformation.Address,
                    City = companyInformation.City,
                    ZipCode = companyInformation.Zip,
                    CompanyName = companyInformation.OrganisationName,
                    Email = companyInformation.Email,
                    PhoneMobile = companyInformation.PhoneMobile,
                    PhoneWork = companyInformation.PhoneWork,
                    Comment = companyInformation.Comment
                },
                IsLoginBankId = !string.IsNullOrEmpty(SiteUser.SerialNumber)
            };
            return View("~/Views/Organization/CompanyInformation.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Index(CompanyInformationPage currentPage, CompanyInformationModel companyInformation)
        {         
            if (string.IsNullOrEmpty(SiteUser?.SerialNumber))
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                ViewBag.Message = "Some fields have invalid value. Please try again!";
                return View("~/Views/Organization/CompanyInformation.cshtml",
                    new CompanyInformationViewModel(currentPage)
                    {
                        CompanyInformation = companyInformation,
                        IsLoginBankId = !string.IsNullOrEmpty(SiteUser.SerialNumber)
                    });
            }
            var organization = UserManager.GetActiveCustomer(HttpContext);

            if (organization != null)
            {
                var result = _organizationRepository.UpdateCompanyInformation(new OrganisationInformation
                {
                    OrganisationId = organization.CustomerId,
                    Address = companyInformation.Address,
                    City = companyInformation.City,
                    Zip = companyInformation.ZipCode,
                    OrganisationName = companyInformation.CompanyName,
                    Email = companyInformation.Email,
                    PhoneMobile = companyInformation.PhoneMobile,
                    PhoneWork = companyInformation.PhoneWork,
                    Comment = companyInformation.Comment
                });

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
                                TempData["UpdateCompanyInformationSuccess"] = true;
                                return RedirectToAction("Index", new {node = settingPage.MyAccountLink});
                            }
                        }
                    }
                }
            }

            var viewModel = new CompanyInformationViewModel(currentPage)
            {
                CompanyInformation = companyInformation,
                IsLoginBankId = !string.IsNullOrEmpty(SiteUser.SerialNumber)
            };
            ViewBag.Message = "Update company information is not successful. Please try again!";
            return View("~/Views/Organization/CompanyInformation.cshtml", viewModel);
        }
    }
}
