using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;
using Gro.Constants;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Pages.AppPages.PurchasingAgreement;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.DataModels.PurchasingAgreements;
using Gro.Core.Interfaces;
using Gro.Helpers;
using Gro.ViewModels.Pages.AppPages.PurchasingAgreement;

namespace Gro.Controllers.Pages.AppPages.PurchasingAgreement
{
    public class PriceHedgePageController : SiteControllerBase<PriceHedgePage>
    {
        private readonly IPurchasingAgreementRepository _agreementRepository;
        private static SettingsPage SettingPage => ContentExtensions.GetSettingsPage();

        public PriceHedgePageController(
            IPurchasingAgreementRepository agreementRepository)
        {
            _agreementRepository = agreementRepository;
        }
        public async Task<ActionResult> Index(PriceHedgePage currentPage,
            string pArea, string period, string priskey,
            string grain,
            string pItemId, string pItem,
            string agreement, string pid, string agreementId = "")
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);

            if (SiteUser == null || supplier == null)
            {
                ViewData["NotLoggedIn"] = true;
                return View("~/Views/AppPages/PriceHedgePage/PriceHedge.cshtml", new PriceHedgeViewModel(currentPage));
            }

            if (string.IsNullOrEmpty(pArea) || string.IsNullOrEmpty(period) || string.IsNullOrEmpty(priskey) || string.IsNullOrEmpty(pid))
            {
                ViewData["IncorrectParams"] = true;
                return View("~/Views/AppPages/PriceHedgePage/PriceHedge.cshtml", new PriceHedgeViewModel(currentPage));
            }
            agreement = string.IsNullOrWhiteSpace(agreement) ? AgreementType.SportAndForwardAvtal : agreement;
            var purchaseDateObject = GenerateFromDatePeriod(period);
            int agrId;
            int.TryParse(agreementId, out agrId);
            var model = new PriceHedgeViewModel(currentPage)
            {
                PriceLow = SettingPage.PurchasePriceLow,
                CommitQuantityMin = 12, //Need to get from service instead of
                TimeWithClock = $"{DateTime.Now:dd MMM yyyy} klockan {DateTime.Now:HH:mm}",
                SelectedPriceArea = (await _agreementRepository.GetSelectedPriceArea(supplier.CustomerNo)),
                AgreementTypeName = GetAgreementTypeName(priskey, agreement),
                AgreementHeading = GetAgreementHeading(agreement),
                ProductItemName = pItem,
                DeliveryPeriod = $"{purchaseDateObject.ValidFrom:yyyy-MM-dd} - {purchaseDateObject.ValidTo:yyyy-MM-dd}",
                RegisterDate = $"{DateTime.Now:yyyy-MM-dd}",
                Customer = (await _agreementRepository.GetCustomerAsync(supplier.CustomerNo)) ?? new Customer(),
                PurchaseAgreementUrl = string.IsNullOrEmpty(pid) ? "#" : GetPurchaseAgreementPageUrl(pid),
                PriceHedgeForm = new PriceHedgeFormModel
                {
                    PriceArea = int.Parse(pArea),
                    ProductItemId = pItemId,
                    GrainType = grain,
                    PriceType = priskey,
                    PriceWatchEndDate = DateTime.Now,
                    HarvestYear = purchaseDateObject.HarvestYear,
                    ValidTo = purchaseDateObject.ValidTo,
                    ValidFrom = purchaseDateObject.ValidFrom,
                    AgreementType = string.IsNullOrEmpty(agreement) ? AgreementType.SportAndForwardAvtal : agreement,
                    AgreementId = agrId
                },
                User = SiteUser,
            };

            return View("~/Views/AppPages/PriceHedgePage/PriceHedge.cshtml", model);
        }

        private string GetAgreementHeading(string agreement)
        {
            if (string.IsNullOrEmpty(agreement)) { return string.Empty; }

            if (agreement.Equals(AgreementType.Poolavtal))
            {
                return "Nytt poolavtal";
            }
            if (agreement.Equals(AgreementType.Depaavtal))
            {
                return "Nytt depåavtal";
            }
            if (agreement.IsMemberOfList(AgreementType.PrissakringDepaavtal, AgreementType.SportAndForwardAvtal))
            {
                return "Uppdrag för prissäkring";
            }
            return string.Empty;
        }

        private string GetAgreementTypeName(string priskey, string agreement)
        {
            if (agreement.Equals(AgreementType.SportAndForwardAvtal) && !string.IsNullOrWhiteSpace(priskey) && priskey.StartsWith("S"))
            {
                return Priskey.Spotprisavtal;
            }
            if (agreement.Equals(AgreementType.SportAndForwardAvtal) && !string.IsNullOrWhiteSpace(priskey) && priskey.StartsWith("T"))
            {
                return Priskey.Terminsavtal;
            }
            if (agreement.Equals(AgreementType.Poolavtal))
            {
                return Priskey.Poolavtal;
            }
            if (agreement.Equals(AgreementType.Depaavtal))
            {
                return Priskey.Depaavtal;
            }
            if (agreement.Equals(AgreementType.PrissakringDepaavtal))
            {
                return Priskey.Depaavtal;
            }
            return string.Empty;
        }

        private string GetConfirmationText(PriceHedgePage currentPage, string agreement)
        {
            if (string.IsNullOrEmpty(agreement))
            {
                return string.Empty;
            }
            XhtmlString confirmationXhtml= null;
            if (agreement.Equals(AgreementType.Depaavtal))
            {
                confirmationXhtml = currentPage.ConfirmationTextForDepaavtal;
            }
            else if (agreement.Equals(AgreementType.Poolavtal))
            {
                confirmationXhtml = currentPage.ConfirmationTextForPoolavtal;
            }
            else if (agreement.Equals(AgreementType.SportAndForwardAvtal))
            {
                confirmationXhtml = currentPage.ConfirmationTextForSportavtal;
            }
            else if (agreement.Equals(AgreementType.PrissakringDepaavtal))
            {
                confirmationXhtml = currentPage.ConfirmationTextForPrissakringDepaavtal;
            }
            return  confirmationXhtml?.ToString() ?? string.Empty;
        }

        [HttpPost]
        public async Task<ActionResult> Index(PriceHedgePage currentPage, PriceHedgeFormModel formModel, string purchaseAgreementUrl)
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);

            if (SiteUser == null || supplier == null)
            {
                return HttpNotFound();
            }

            var customer = await _agreementRepository.GetCustomerAsync(supplier.CustomerNo);

            var model = new PriceHedgeViewModel(currentPage)
            {
                Customer = customer,
                PurchaseAgreementUrl = purchaseAgreementUrl,
                PriceHedgeForm = formModel,
                ConfiramtionText = GetConfirmationText(currentPage, formModel.AgreementType)
            };

            AgreementResponse result = null;
            if (formModel.AgreementType.Equals(AgreementType.SportAndForwardAvtal))
            {
                result = await SaveSpotAndForwardAgreement(formModel, customer);
            }
            else if (formModel.AgreementType.Equals(AgreementType.Depaavtal))
            {
                result = await SaveStorageAgreementAgreement(formModel, customer);
            }
            else if (formModel.AgreementType.Equals(AgreementType.Poolavtal))
            {
                result = await SavePoolAgreement(formModel, customer);
            }
            else if (formModel.AgreementType.Equals(AgreementType.PrissakringDepaavtal))
            {
                result = await SavePriceProtectedStorageAgreement(formModel, customer);
            }

            if (result?.Id != null)
            {
                return View("~/Views/AppPages/PriceHedgePage/PriceWatchStep3.cshtml", model);
            }

            TempData["Error"] = true;
            return View("~/Views/AppPages/PriceHedgePage/PriceWatchStep3.cshtml", model);
        }

        private async Task<AgreementResponse> SavePriceProtectedStorageAgreement(PriceHedgeFormModel formModel, Customer customer)
        {
            var storageAgreement = new PriceProtectStorageAgreement()
            {
                CustomerUserName = customer.CustomerName,
                WatchDate = formModel.PriceWatchEndDate,
                WatchPrice = formModel.UpperPrice,
                WatchPriceMinimum = formModel.LowerPrice,
                WatchAction = formModel.TargetAction,
                AgreementId = formModel.AgreementId.ToString()
            };
            //TODO: waiting Olle to update the service, saving method should return AgreementResponse obj ==>done!
            return await _agreementRepository.SavePriceProtectedStorageAgreementAsync(storageAgreement);
        }

        private async Task<AgreementResponse> SaveStorageAgreementAgreement(PriceHedgeFormModel formModel, Customer customer)
        {
            var storageAgreement = new StorageAgreement()
            {
                //AgreementId = 
                CustomerId = customer.CustomerId,
                GrainType = formModel.GrainType,
                HarvestYear = formModel.HarvestYear,
                ModeOfDelivery = formModel.DeliveryMode,
                PriceArea = formModel.PriceArea,
                ProductItemId = formModel.ProductItemId,
                Quantity = formModel.CommitQuantity,
                ValidFrom = formModel.ValidFrom,
                ValidTo = formModel.ValidTo,
            };

            return await _agreementRepository.SaveStorageAgreementAsync(storageAgreement);
        }

        private async Task<AgreementResponse> SaveSpotAndForwardAgreement(PriceHedgeFormModel formModel, Customer customer)
        {
            var spotAgreement = new SpotAndForwardAgreement
            {
                PriceArea = formModel.PriceArea,
                HarvestYear = formModel.HarvestYear,
                ValidTo = formModel.ValidTo,
                ValidFrom = formModel.ValidFrom,
                ModeOfDelivery = formModel.DeliveryMode,
                Quantity = formModel.CommitQuantity,
                ProductItemId = formModel.ProductItemId,
                //AgreementType = formModel.AgreementTypeCode,
                PriceType = formModel.PriceType,
                GrainType = formModel.GrainType,
                WatchDate = formModel.PriceWatchEndDate,
                Price = formModel.UpperPrice, //From Orri - Price as WatchPriceMaximum
                WatchPriceMinimum = formModel.LowerPrice,
                WatchAction = formModel.TargetAction,

                CustomerId = customer.CustomerId,
                //CustomerName = customer.CustomerName
            };

            return await _agreementRepository.SaveSpotAndForwardAgreementAsync(spotAgreement);
        }

        private async Task<AgreementResponse> SavePoolAgreement(PriceHedgeFormModel formModel, Customer customer)
        {
            var spotAgreement = new PoolAgreement()
            {
                PriceArea = formModel.PriceArea,
                PriceType = formModel.PriceType,
                HarvestYear = formModel.HarvestYear,
                ValidTo = formModel.ValidTo,
                ValidFrom = formModel.ValidFrom,
                ModeOfDelivery = formModel.DeliveryMode,
                Quantity = formModel.CommitQuantity,
                ProductItemId = formModel.ProductItemId,
                GrainType = formModel.GrainType,
                CustomerId = customer.CustomerId,
                //CustomerName = customer.CustomerName
            };

            return await _agreementRepository.SavePoolAgreement(spotAgreement);
        }

        private static PurchaseAgreementDates GenerateFromDatePeriod(string datePeriod)
        {
            if (string.IsNullOrEmpty(datePeriod)) return new PurchaseAgreementDates();
            var startDate = datePeriod.Substring(0, 8);
            var endDate = datePeriod.Substring(9, 8);

            var validFrom = DateTime.ParseExact(startDate, "yyyyMMdd", CultureInfo.InvariantCulture);
            var validTo = DateTime.ParseExact(endDate, "yyyyMMdd", CultureInfo.InvariantCulture);

            var startDateYYYY = int.Parse(startDate.Substring(0, 4));
            var startDateMMdd = int.Parse(startDate.Substring(4, 4));

            var harvestYear = startDateMMdd < 701 ? startDateYYYY - 1 : startDateYYYY;

            return new PurchaseAgreementDates()
            {
                ValidFrom = validFrom,
                ValidTo = validTo,
                HarvestYear = harvestYear
            };
        }

        private static string GetPurchaseAgreementPageUrl(string pid)
        {
            var urlHelper = ServiceLocator.Current.GetInstance<UrlHelper>();
            return urlHelper.ContentUrl(new ContentReference(pid));
        }
    }
}
