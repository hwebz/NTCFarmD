using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EPiServer.Framework.DataAnnotations;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Pages.AppPages.PurchasingAgreement;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.DataModels.PurchasingAgreements;
using Gro.Core.DataModels.Security;
using Gro.Core.Interfaces;
using Gro.Helpers;
using Gro.ViewModels.Pages.AppPages.PurchasingAgreement;

namespace Gro.Controllers.Pages.AppPages.PurchasingAgreement
{
    [TemplateDescriptor(Inherited = true)]
    public class PurchasingAgreementPageController : SiteControllerBase<PurchasingAgreementPage>
    {
        private readonly IPurchasingAgreementRepository _agreementRepository;

        public PurchasingAgreementPageController(
            IPurchasingAgreementRepository agreementRepository)
        {
            _agreementRepository = agreementRepository;
        }

        private static SettingsPage SettingPage => ContentExtensions.GetSettingsPage();

        public async Task<ActionResult> Index(PurchasingAgreementPage currentPage)
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);
            var viewName = GetViewName(currentPage.AgreementType);

            if (SiteUser == null || supplier == null)
            {
                ViewData["NotLoggedIn"] = true;
                return View($"~/Views/AppPages/PurchasingAgreementPage/{viewName}.cshtml",
                    new PurchasingAgreementViewModel(currentPage));
            }
            var model = await CreateViewModel(currentPage, supplier.CustomerNo);
            
            return View($"~/Views/AppPages/PurchasingAgreementPage/{viewName}.cshtml", model);
        }


        private string GetViewName(string agreementType)
        {
            if (agreementType == AgreementType.PrissakringDepaavtal) return "ProtectStorageAgreement";
            if (agreementType == AgreementType.SportAndForwardAvtal) return "SpotAndForwardAgreement";
            if (agreementType == AgreementType.Poolavtal || agreementType == AgreementType.Depaavtal)
                return "PoolAndDepaAgreement";
            return "Index";
        }

        private async Task<PurchasingAgreementViewModel> CreateViewModel(PurchasingAgreementPage currentPage,
            string customerNo, string productionItemId = "", string grainTypeId = "",
            string agreementId = "")
        {
            var viewModel = new PurchasingAgreementViewModel(currentPage);
            Dictionary<string, string> periods = new Dictionary<string, string>();

            var selectedPriceArea =await _agreementRepository.GetSelectedPriceArea(customerNo);
            var priceAreaId = selectedPriceArea != null ? selectedPriceArea.PriceAreaId : string.Empty;

            if (currentPage.AgreementType == AgreementType.PrissakringDepaavtal)
            {
                var storageAgreements = await _agreementRepository.GetStorageAgreementsForPriceProtectionAsync(customerNo);
                storageAgreements = storageAgreements ?? new StorageAgreement[0];
                var selectedStorageAgreement = storageAgreements.Any() && !string.IsNullOrEmpty(agreementId)
                    ? storageAgreements.FirstOrDefault(x => x.AgreementId == agreementId)
                    : null;
                periods = ConvertPeriodToMonthRange(await _agreementRepository.GetPeriodsPriceProtectingStorageAgreementAsync(priceAreaId, productionItemId, grainTypeId));

                viewModel.StorageAgreements = storageAgreements.ToList();
                viewModel.SelectedAgreement = selectedStorageAgreement;
            }
            else if (currentPage.AgreementType == AgreementType.SportAndForwardAvtal)
            {
                var products = _agreementRepository.GetProductsSpotAndForwardAgreement(priceAreaId);
                var grainTypes = !string.IsNullOrEmpty(productionItemId)
                    ? await _agreementRepository.GetGrainTypesSpotAndForwardAgreementAsync(productionItemId, priceAreaId)
                    : new Product[0];
                periods = ConvertPeriodToMonthRange(await _agreementRepository.GetPeriodsSpotAndForwardAgreementAsync(priceAreaId));

                viewModel.Products = products;
                viewModel.GrainTypes = grainTypes;
            }
            else if (currentPage.AgreementType == AgreementType.Poolavtal || currentPage.AgreementType == AgreementType.Depaavtal)
            {

                if (currentPage.AgreementType == AgreementType.Poolavtal)
                {
                    viewModel.Header = "Teckna Poolavtal";
                    viewModel.Products = await _agreementRepository.GetProductsPoolAgreementAsync(priceAreaId);
                    viewModel.ModesOfDelivery = await _agreementRepository.GetModesOfDeliveryPoolAgreementAsync();
                    periods = ConvertPeriodToMonthRange(await _agreementRepository.GetPeriodsPoolAgreementAsync(priceAreaId));
                }
                else if (currentPage.AgreementType == AgreementType.Depaavtal)
                {
                    viewModel.Header = "Teckna depåavtal";
                    viewModel.Products = await _agreementRepository.GetProductsStorageAgreementAsync(priceAreaId);
                    viewModel.ModesOfDelivery = await _agreementRepository.GetModesOfDeliveryStorageAgreementAsync();
                    periods = ConvertPeriodToMonthRange(await _agreementRepository.GetPeriodsStorageAgreementAsync(priceAreaId));
                    viewModel.DepaPeriod = GetDepaPeriod(periods.FirstOrDefault());
                }
            }

            viewModel.SelectedPriceArea = selectedPriceArea;
            viewModel.Periods = periods;
            viewModel.FormModel = new PurchasingAgreementFormModel();
            viewModel.ReferencePrice = GetReferencePrice();
            viewModel.MinPrice = SettingPage.PurchasePriceLow;
            viewModel.CommitQuantityMin = 12;
            return viewModel;
        }

        private int GetReferencePrice()
        {
            //TODO: implement later when the service is ready
            return 1480;
        }

        //[HttpGet]
        //[Route("api/agreement/get-protect-agreement")]
        //public async Task<JsonResult> GetProtectAgreement(string agreementId = "")
        //{
        //    var supplier = UserManager.GetActiveCustomer(HttpContext);
        //    if (SiteUser == null || supplier == null)
        //    {
        //        return Json(new {}, JsonRequestBehavior.AllowGet);
        //    }
        //    var agreement = !string.IsNullOrEmpty(agreementId)
        //        ? await _agreementRepository.GetStorageAgreementValuesForPriceProtectionAsync(agreementId)
        //        : null;
        //    object viewInfor = new {};
        //    if (agreement != null)
        //    {
        //        var productItemName = agreement.ProductItemName;
        //        var grainType = agreement.GrainType;
        //        var periods =
        //            await
        //                _agreementRepository.GetPeriodsPriceProtectingStorageAgreementAsync(
        //                    agreement.PriceArea.ToString(), agreement.ProductItemId, agreement.GrainType);
        //        var periodsView =
        //            this.RenderPartialViewToString(
        //                "~/Views/AppPages/PurchasingAgreementPage/Partial/PricePeriodsList.cshtml", periods);
        //        viewInfor = new
        //        {
        //            productItemName,
        //            grainType,
        //            periodsView
        //        };
        //    }
        //    return Json(viewInfor, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        [Route("api/agreement/get-protect-agreement")]
        public async Task<JsonResult> GetProtectAgreement(string productItemId = "", string priceAreaId="", string grainType="")
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);
            if (SiteUser == null || supplier == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            object viewInfor = new { };
            if (!string.IsNullOrEmpty(productItemId))
            {
                var periods = ConvertPeriodToMonthRange(await _agreementRepository.GetPeriodsPriceProtectingStorageAgreementAsync(priceAreaId, productItemId, grainType));
                var periodsView =
                    this.RenderPartialViewToString(
                        "~/Views/AppPages/PurchasingAgreementPage/Partial/PricePeriodsList.cshtml", periods);
                viewInfor = new
                {
                    periodsView
                };
            }
            return Json(viewInfor, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("api/agreement/get-grain-type")]
        public async Task<JsonResult> GetGrainTypes(string priceAreaId, string productItemId)
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);
            if (SiteUser == null || supplier == null)
            {
                return Json(new {}, JsonRequestBehavior.AllowGet);
            }
            var grainType = await _agreementRepository.GetGrainTypesPoolAgreementAsync(priceAreaId, productItemId);
            return Json(grainType, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("api/agreement/get-grain-type-spot-agreement")]
        public async Task<JsonResult> GetGrainTypesForSpotAgreement(string priceAreaId, string productItemId)
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);
            if (SiteUser == null || supplier == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            var grainType = await _agreementRepository.GetGrainTypesSpotAndForwardAgreementAsync(productItemId, priceAreaId);
            return Json(grainType, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SaveAgreement(PurchasingAgreementPage currentPage,
            PurchasingAgreementFormModel formModel)
        {
            if (formModel == null ||
                (formModel.AgreementId == 0 && currentPage.AgreementType == AgreementType.PrissakringDepaavtal))
            {
                return RedirectToAction("Index");
            }

            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);
            if (SiteUser == null || activeCustomer == null)
            {
                return HttpNotFound();
            }

            AgreementResponse savedAgreement = null;
            if (currentPage.AgreementType == AgreementType.PrissakringDepaavtal)
            {
                savedAgreement = await SavePriceProtectedStorageAgreement(formModel, activeCustomer.CustomerName);
            }
            else if (currentPage.AgreementType == AgreementType.Poolavtal)
            {
                savedAgreement = await SavePoolAgreement(formModel, activeCustomer);
            }
            else if (currentPage.AgreementType == AgreementType.Depaavtal)
            {
                savedAgreement = await SaveDepaAgreement(formModel, activeCustomer);
            }
            else
            {
                savedAgreement = await SaveSpotAndForwardAgreement(formModel, activeCustomer.CustomerNo);
            }

            if (savedAgreement?.Id == null)
            {
                TempData["Saved"] = false;
            }
            else
            {
                TempData["Saved"] = true;
            }
            return RedirectToAction("Index");
        }

        private async Task<AgreementResponse> SavePoolAgreement(PurchasingAgreementFormModel formModel,
            CustomerBasicInfo customer)
        {
            var purchasePeriod = GenerateFromDatePeriod(formModel.AgreementPeriod);
            var spotAgreement = new PoolAgreement
            {
                PriceArea = formModel.PriceArea,
                PriceType = purchasePeriod.PriceType,
                HarvestYear = purchasePeriod.HarvestYear,
                ValidTo = purchasePeriod.ValidTo,
                ValidFrom = purchasePeriod.ValidFrom,
                ModeOfDelivery = formModel.DeliveryMode,
                Quantity = formModel.CommitQuantity,
                ProductItemId = formModel.ProductItemId,
                GrainType = formModel.GrainType,
                CustomerId = customer.CustomerNo
            };

            return await _agreementRepository.SavePoolAgreement(spotAgreement);
        }

        private async Task<AgreementResponse> SaveDepaAgreement(PurchasingAgreementFormModel formModel,
            CustomerBasicInfo customer)
        {
            var purchasePeriod = GenerateFromDatePeriod(formModel.AgreementPeriod);
            
            var storageAgreement = new StorageAgreement()
            {
                CustomerId = customer.CustomerNo,
                GrainType = formModel.GrainType,
                HarvestYear = purchasePeriod.HarvestYear,
                ModeOfDelivery = formModel.DeliveryMode,
                PriceArea = formModel.PriceArea,
                ProductItemId = formModel.ProductItemId,
                Quantity = formModel.CommitQuantity,
                ValidFrom = purchasePeriod.ValidFrom,
                ValidTo = purchasePeriod.ValidTo,
            };

            return await _agreementRepository.SaveStorageAgreementAsync(storageAgreement);
        }

        private async Task<AgreementResponse> SavePriceProtectedStorageAgreement(PurchasingAgreementFormModel formModel,
            string customerName)
        {
            var purchasePeriod = GenerateFromDatePeriod(formModel.AgreementPeriod);
            var agreement = new PriceProtectStorageAgreement
            {
                CustomerUserName = customerName,
                WatchPrice = formModel.UpperPrice,
                WatchPriceMinimum = formModel.LowerPrice,
                WatchDate = formModel.PriceWatchEndDate,
                AgreementId = formModel.AgreementId.ToString(),
                WatchAction = formModel.TargetAction,
                PriceType = purchasePeriod.PriceType,
                ValidTo = purchasePeriod.ValidTo,
                ValidFrom = purchasePeriod.ValidFrom,
            };
            return await _agreementRepository.SavePriceProtectedStorageAgreementAsync(agreement);
        }

        private async Task<AgreementResponse> SaveSpotAndForwardAgreement(PurchasingAgreementFormModel formModel, string customerNo)
        {
            var purchasePeriod = GenerateFromDatePeriod(formModel.AgreementPeriod);
            var spotAgreement = new SpotAndForwardAgreement
            {
                PriceArea = formModel.PriceArea,
                HarvestYear = purchasePeriod.HarvestYear,
                ValidTo = purchasePeriod.ValidTo,
                ValidFrom = purchasePeriod.ValidFrom,
                ModeOfDelivery = formModel.DeliveryMode,
                Quantity = formModel.CommitQuantity,
                ProductItemId = formModel.ProductItemId,
                //AgreementType = formModel.AgreementTypeCode,
                PriceType = purchasePeriod.PriceType,
                GrainType = formModel.GrainType,
                WatchDate = formModel.PriceWatchEndDate,
                Price = formModel.UpperPrice, //From Orri - Price as WatchPriceMaximum
                WatchPriceMinimum = formModel.LowerPrice,
                WatchAction = formModel.TargetAction,

                CustomerId = customerNo,
            };

            return await _agreementRepository.SaveSpotAndForwardAgreementAsync(spotAgreement);
        }

        private Dictionary<string, string> ConvertPeriodToMonthRange(Dictionary<string, string> periods)
        {
            return periods.ToDictionary(period => $"{period.Key};{period.Value}",
                period => $"{GetMonth(period.Value.Substring(4, 2))}-{GetMonth(period.Value.Substring(13, 2))}");
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

        private static PurchaseAgreementPeriod GenerateFromDatePeriod(string periodInfo)
        {
            if (string.IsNullOrEmpty(periodInfo)) return new PurchaseAgreementPeriod();

            var startIndex = periodInfo.IndexOf(';');

            var priceType = periodInfo.Substring(0, startIndex);

            var datePeriod = periodInfo.Substring(startIndex + 1);

            var startDate = datePeriod.Substring(0, 8);
            var endDate = datePeriod.Substring(9, 8);

            var validFrom = DateTime.ParseExact(startDate, "yyyyMMdd", CultureInfo.InvariantCulture);
            var validTo = DateTime.ParseExact(endDate, "yyyyMMdd", CultureInfo.InvariantCulture);

            var startDateYYYY = int.Parse(startDate.Substring(0, 4));
            var startDateMMdd = int.Parse(startDate.Substring(4, 4));

            var harvestYear = startDateMMdd < 701 ? startDateYYYY - 1 : startDateYYYY;

            return new PurchaseAgreementPeriod
            {
                PriceType = priceType,
                ValidFrom = validFrom,
                ValidTo = validTo,
                HarvestYear = harvestYear
            };
        }

        private static KeyValuePair<string, string> GetDepaPeriod(KeyValuePair<string, string> period)
        {
            var startIndex = period.Key.IndexOf(';');
            return new KeyValuePair<string, string>(period.Value, period.Key.Substring(startIndex + 1));
        }
    }
}