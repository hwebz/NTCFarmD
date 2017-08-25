using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gro.Core.ContentTypes.Pages.AppPages;
using Gro.Core.DataModels.PurchasingAgreements;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.AppPages.DryingAgreement;

namespace Gro.Controllers.Pages.AppPages
{
    [RoutePrefix("api/drying-agreement")]
    public class DryingAgreementPageController : SiteControllerBase<DryingAgreementPage>
    {
        private readonly IPurchasingAgreementRepository _purchaseRepository;

        public DryingAgreementPageController(IPurchasingAgreementRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<ActionResult> Index(DryingAgreementPage currentPage)
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);

            if (SiteUser == null || supplier == null)
            {
                TempData["NotLoggedIn"] = true;
                return View("~/Views/AppPages/DryingAgreementPage/DryingAgreement.cshtml", new DryingAgreementViewModel(currentPage));
            }

            var dryingAgreement = await _purchaseRepository.GetDryingAgreementAsync();
            var model = new DryingAgreementViewModel(currentPage)
            {
                Agreement = dryingAgreement ?? new DryingAgreement(),
                Customer = (await _purchaseRepository.GetCustomerAsync(supplier.CustomerNo)) ?? new Customer(),
            };
            return View("~/Views/AppPages/DryingAgreementPage/DryingAgreement.cshtml", model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(DryingAgreementPage currentPage, FormCollection form)
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);

            if (SiteUser == null || supplier == null)
            {
                return HttpNotFound();
            }

//#if !DEBUG
            var result = await _purchaseRepository.SaveDryingAgreementAsync(supplier.CustomerNo);
            if (!string.IsNullOrEmpty(result) && result.Contains("Saved dryagreement to database."))
            {
                ViewBag.GeneratePdf = true;
                return View("~/Views/AppPages/DryingAgreementPage/DryingAgreementStep3.cshtml", new DryingAgreementViewModel(currentPage));
            }
//#endif
            ViewBag.GeneratePdf = false;
            return View("~/Views/AppPages/DryingAgreementPage/DryingAgreementStep3.cshtml", new DryingAgreementViewModel(currentPage));
        }

        [Route("generatePdf")]
        public ActionResult GeneratePdf()
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);
            var currentPage = new DryingAgreementPage();

            if (SiteUser == null || supplier == null)
            {
                return HttpNotFound();
            }

            var dryingAgreement =  _purchaseRepository.GetDryingAgreement();
            var model = new DryingAgreementViewModel(currentPage)
            {
                Agreement = dryingAgreement ?? new DryingAgreement(),
                Customer = _purchaseRepository.GetCustomer(supplier.CustomerNo) ?? new Customer(),
            };
            return View("~/Views/AppPages/DryingAgreementPage/DryingAgreementPdf.cshtml", model);
        }
    }
}
