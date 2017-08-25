using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.AppPages.PurchasingAgreement;
using Gro.Core.DataModels.PurchasingAgreements;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.AppPages.PriceGrain;

namespace Gro.Controllers.Pages.AppPages.PurchasingAgreement
{
    [RoutePrefix("api/price-grain")]
    public class PriceGrainController : SiteControllerBase<PriceGrainPage>
    {
        private readonly IPurchasingAgreementRepository _agreementRepository;
        private readonly IUserManagementService _usersManagementService;

        public PriceGrainController(IPurchasingAgreementRepository agreementRepository, IUserManagementService usersManagementService)
        {
            _agreementRepository = agreementRepository;
            _usersManagementService = usersManagementService;
        }
        public async Task<ActionResult> Index(PriceGrainPage currentPage)
        {
            var model = new PriceGrainPageViewModel(currentPage);
            var supplier = _usersManagementService.GetActiveCustomer(HttpContext);
            if (supplier == null)
            {
                return View("~/Views/AppPages/PriceGrain/Index.cshtml", model);
            }

            var resultPriceAreas = await _agreementRepository.GetPriceAreasAsync();
            if (resultPriceAreas == null || resultPriceAreas.Length == 0) return View("~/Views/AppPages/PriceGrain/Index.cshtml", model);

            model.PriceAreaList = resultPriceAreas.ToList();
            var resultPricePeriod =
                await _agreementRepository.GetPricePeriodsGrainPriceAsync(resultPriceAreas[0].PriceAreaId);

            if (resultPricePeriod?.Length > 0)
            {
                DateTime parsedDate;
                var parsedDateStr = string.Empty;
                if (DateTime.TryParse(resultPricePeriod[0].PriceType, out parsedDate))
                {
                    parsedDateStr = parsedDate.ToString("yyyy-MM-dd");
                }
                resultPricePeriod[0].PriceType = parsedDateStr;
                model.PricePeriodFirst = resultPricePeriod.ToList();
            }
            else
            {
                model.PricePeriodFirst = new List<PricePeriod>();
            }
            return View("~/Views/AppPages/PriceGrain/Index.cshtml", model);
        }

        [HttpPost]
        [Route("GetPricePeriodbyArea")]
        public async Task<ActionResult> GetPricePeriods(string priceAreaId)
        {
            var supplier = _usersManagementService.GetActiveCustomer(HttpContext);
            var result = await _agreementRepository.GetPricePeriodsGrainPriceAsync(priceAreaId);
            if (supplier == null || result== null || result.Length <= 0) return Json(new { resultPricePeriods = string.Empty });

            DateTime parsedDate;
            var parsedDateStr = string.Empty;
            if (DateTime.TryParse(result[0].PriceType, out parsedDate))
            {
                parsedDateStr = parsedDate.ToString("yyyy-MM-dd");
            }
            result[0].PriceType = parsedDateStr;
            return PartialView("~/Views/AppPages/PriceGrain/_TablePricePeriodsResult.cshtml", result.ToList());
        }
    }
}