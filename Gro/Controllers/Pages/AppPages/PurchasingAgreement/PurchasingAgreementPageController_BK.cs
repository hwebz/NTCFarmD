using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EPiServer.Framework.DataAnnotations;
using Gro.Core.ContentTypes.Pages.AppPages.PurchasingAgreement;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.DataModels.PurchasingAgreements;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.AppPages.PurchasingAgreement;

namespace Gro.Controllers.Pages.AppPages.PurchasingAgreement
{
    [TemplateDescriptor(Inherited = true)]
    public class PurchasingAgreementPageController_BK : SiteControllerBase<PurchasingAgreementPage>
    {
        private readonly IPurchasingAgreementRepository _agreementRepository;

        public PurchasingAgreementPageController_BK(
            IPurchasingAgreementRepository agreementRepository)
        {
            _agreementRepository = agreementRepository;
        }

        public async Task<ActionResult> Index(PurchasingAgreementPage currentPage)
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);

            if (SiteUser == null || supplier == null)
            {
                ViewData["NotLoggedIn"] = true;
                return View("~/Views/AppPages/PurchasingAgreementPage/PurchasingAgreement.cshtml", new PurchasingAgreementViewModel(currentPage));
            }
            var model = await CreatePurchasingAgreementViewModel(currentPage, supplier.CustomerNo);
            var viewName = currentPage.AgreementType == AgreementType.PrissakringDepaavtal ? "ProtectedStorageAgreement" : "PurchasingAgreement";

            return View($"~/Views/AppPages/PurchasingAgreementPage/{viewName}.cshtml", model);
        }

        private async Task<PurchasingAgreementViewModel> CreatePurchasingAgreementViewModel(PurchasingAgreementPage currentPage, string customerNo, string priceAreaId = "", string productionItemId = "", string grainTypeId = "", string agreementId = "")
        {
            PricePeriod[] pricePeriods = null;
            var result = new PurchasingAgreementViewModel(currentPage);
//#if DEBUG
//            customerNo = "1000000";
//#endif
            if (currentPage.AgreementType == AgreementType.PrissakringDepaavtal)
            {
                var storageAgreements = await _agreementRepository.GetStorageAgreementsForPriceProtectionAsync(customerNo);
                storageAgreements = storageAgreements ?? new StorageAgreement[0];
                StorageAgreement selectedAgreement = null;
                if (storageAgreements.Any())
                {
                    selectedAgreement = string.IsNullOrEmpty(agreementId) ? storageAgreements.FirstOrDefault() : storageAgreements.FirstOrDefault(x => x.AgreementId == agreementId);
                    result.SelectedAgreement = selectedAgreement;
                }

                if (!string.IsNullOrEmpty(priceAreaId) || !string.IsNullOrEmpty(productionItemId))
                {
#if DEBUG
                    priceAreaId = "1";
                    productionItemId = "100160";
                    grainTypeId = "BRONS";
#endif
                    pricePeriods = await GetPricePeriods(customerNo, priceAreaId, currentPage.AgreementType, productionItemId, grainTypeId);
                }
                else
                {
                    result.StorageAgreements = storageAgreements.ToList();
                    if (selectedAgreement != null)
                    {
                        pricePeriods = await GetPricePeriods(customerNo, selectedAgreement.PriceArea.ToString(),
                            currentPage.AgreementType, selectedAgreement.ProductItemId, selectedAgreement.GrainType);
                    }
                }
                pricePeriods = pricePeriods ?? await Task.FromResult(new PricePeriod[0]);
            }
            else
            {
                var priceAreas = await _agreementRepository.GetPriceAreasAsync(customerNo);
                priceAreas = priceAreas ?? await Task.FromResult(new PriceArea[0]);
                var selectedPriceArea = priceAreas.FirstOrDefault(x => x.FavoritePriceAreaId) ?? priceAreas.FirstOrDefault();

                pricePeriods =
                       await GetPricePeriods(customerNo, selectedPriceArea?.PriceAreaId, currentPage.AgreementType);
                pricePeriods = pricePeriods ?? await Task.FromResult(new PricePeriod[0]);

                var productCategories = pricePeriods.Where(x => !string.IsNullOrEmpty(x.ProductItemHierarchy))
               .GroupBy(x => x.ProductItemHierarchy)
               .Select(x => new ProductItemCategory()
               {
                   ID = Guid.NewGuid().ToString(),
                   Hierarchy = x.Key,
                   ProductItems = x.ToList()
               }).ToList();
                result.SelectedPriceArea = selectedPriceArea;
                result.PriceAreas = priceAreas.ToList();
                result.ProductCategories = productCategories.ToList();
            }
            result.PricePeriods = GetPricePeriodHeader(pricePeriods.FirstOrDefault(), currentPage.AgreementType);
            return result;
        }

        private Task<PricePeriod[]> GetPricePeriods(string customerNo, string priceAreaId, string agreementType, string productionItemId = "", string grainTypeId = "")
        {
            if (string.IsNullOrEmpty(agreementType) || agreementType.Equals(AgreementType.SportAndForwardAvtal))
            {
                return _agreementRepository.GetPricePeriodsForSpotAndForwardAgreementsAsync(customerNo, priceAreaId);
            }
            if (agreementType.Equals(AgreementType.Depaavtal))
            {
                return _agreementRepository.GetPricePeriodsForStorageAgreementsAsync(customerNo, priceAreaId);
            }
            if (agreementType.Equals(AgreementType.Poolavtal))
            {
                return _agreementRepository.GetPricePeriodsForPoolAgreementsAsync(customerNo, priceAreaId);
            }
            if (agreementType.Equals(AgreementType.PrissakringDepaavtal))
            {
                return _agreementRepository.GetPricePeriodsForPriceProtectingStorageAgreementAsync(priceAreaId, productionItemId, grainTypeId);
            }

            return Task.FromResult(new PricePeriod[0]);
        }

        private List<PricePeriodHeader> GetPricePeriodHeader(PricePeriod pricePeriod, string agreementType)
        {
            var pricePeriodHeaders = new List<PricePeriodHeader>();

            if (pricePeriod == null) return pricePeriodHeaders;

            foreach (var periodInfo in pricePeriod.Prices)
            {
                var startIndex = periodInfo.IndexOf(';');
                var periodStr = periodInfo.Substring(startIndex + 1);
                var pricePeriodHeader = new PricePeriodHeader
                {
                    Period = periodStr,
                    Priskey = GetPriskey(periodInfo, agreementType)
                };

                if (periodInfo.StartsWith("S"))
                {
                    pricePeriodHeader.MonthRange = "Senast";
                    pricePeriodHeader.DateRange = $"{periodStr.Substring(11, 6)}";
                }
                else //if (periodInfo.StartsWith("T"))
                {
                    var spaceForRange = agreementType == AgreementType.PrissakringDepaavtal ? " " : string.Empty;
                    pricePeriodHeader.MonthRange = $"{GetMonth(periodStr.Substring(4, 2))}{spaceForRange}-{spaceForRange}{GetMonth(periodStr.Substring(13, 2))}";
                    pricePeriodHeader.DateRange = $"{periodStr.Substring(2, 6)}- {periodStr.Substring(11, 6)}";
                }
                pricePeriodHeaders.Add(pricePeriodHeader);
            }

            return pricePeriodHeaders;
        }

        private string GetPriskey(string periodInfo, string agreementType)
        {
            if (string.IsNullOrEmpty(agreementType)) return string.Empty;

            if (agreementType.Equals(AgreementType.SportAndForwardAvtal) || agreementType.Equals(AgreementType.PrissakringDepaavtal))
            {
                var startIndex = periodInfo.IndexOf(';');
                return periodInfo.Substring(0, startIndex);
            }

            if (agreementType.Equals(AgreementType.Depaavtal))
            {
                return "Depa";
            }

            if (agreementType.Equals(AgreementType.Poolavtal))
            {
                return "Pool";
            }

            return string.Empty;
        }

        private static string GetMonth(string mm)
        {
            switch (mm)
            {
                case "01":
                    return "Jan";
                case "02":
                    return "Feb";
                case "03":
                    return "Mar";
                case "04":
                    return "Apr";
                case "05":
                    return "Maj";
                case "06":
                    return "Jun";
                case "07":
                    return "Jul";
                case "08":
                    return "Aug";
                case "09":
                    return "Sep";
                case "10":
                    return "Okt";
                case "11":
                    return "Nov";
                case "12":
                    return "Dec";
                default:
                    return string.Empty;
            }
        }

        [HttpGet]
        public async Task<ActionResult> Get(PurchasingAgreementPage currentPage, string priceAreaId, string productionItemId = "", string grainTypeId = "", string agreementId = "")
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);
            if (SiteUser == null || supplier == null)
            {
                return HttpNotFound();
            }

            if (currentPage.AgreementType != AgreementType.PrissakringDepaavtal)
            {
                await _agreementRepository.SaveCustomerFavoritePriceAreaAsync(supplier.CustomerNo, priceAreaId);
            }
            var model = await CreatePurchasingAgreementViewModel(currentPage, supplier.CustomerNo, priceAreaId, productionItemId, grainTypeId, agreementId);

            // ReSharper disable once Mvc.PartialViewNotResolved
            var viewName = currentPage.AgreementType != AgreementType.PrissakringDepaavtal
                ? "Partial/PriceWatchList"
                : "Partial/PriceWatchListForProtectedAgreement";
            return PartialView(viewName, model);
        }

        [HttpPost]
        [Route("api/purchase-agreement/marked")]
        public async Task<JsonResult> MarkAsFavorite(string priceAreaId, string priceType, string productItemId, string grainType, string favorite)
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);
            if (SiteUser == null || supplier == null)
            {
                return Json(new { success = false, message = "Du är inte påloggad, vänligen logga in för att få fullständig funktionalitet" }, JsonRequestBehavior.AllowGet);
            }
            var result = await _agreementRepository.UpdateCustomerFavoriteProductitem(supplier.CustomerNo, priceAreaId,
                priceType, productItemId, grainType, favorite.Equals("1"));

            return Json(new { success = result, message = !result ? "Felmeddelande!" : string.Empty }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(PurchasingAgreementPage currentPage)
        {
            // ReSharper disable once Mvc.ViewNotResolved
            return View("Index", null);
        }

    }
}
