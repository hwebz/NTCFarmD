using System.Web.Mvc;
using Gro.Core.ContentTypes.Pages.AppPages.PurchasingAgreement;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data;
using Gro.ViewModels.Pages.AppPages.PriceAlert;
using Gro.Business.Services.Users;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gro.Controllers.Pages.AppPages.PurchasingAgreement
{
    [RoutePrefix("api/price-alert")]
    public class PriceAlertPageController : SiteControllerBase<PriceAlertPage>
    {
        private readonly IPurchasingAgreementRepository _agreementRepository;
        private readonly IUserManagementService _usersManagementService;
        private readonly string _ticket;

        public PriceAlertPageController(IPurchasingAgreementRepository agreementRepository, TicketProvider ticketProvider,
            IUserManagementService usersManagementService)
        {
            _agreementRepository = agreementRepository;
            _ticket = ticketProvider.GetTicket();
            _usersManagementService = usersManagementService;
        }
        public async Task<ActionResult> Index(PriceAlertPage currentPage)
        {
            var supplier = _usersManagementService.GetActiveCustomer(HttpContext);
            var result = await _agreementRepository.GetPriceWatchAsync(supplier?.CustomerNo ?? string.Empty, _ticket);
            var priceWatchList = new List<PriceWatchView>();
            if (result?.Length > 0)
            {
                var resultList = result.ToList();
                foreach (var item in resultList)
                {
                    var priceWatch = new PriceWatchView()
                    {
                        Id = item.Id,
                        Name = item.Name + " " + item.ItemName,
                        Quantity = item.AgreedQuantity,
                        Price = item.Price,
                        PriceMin = item.WatchPriceMin,
                        ContractType = item.AgreementType,
                        ValidTo = item.WatchDate.ToString("yyyy-MM-dd"),
                        Activity = GetActivityDisplay(item.Activity, item.AgreementType)
                    };

                    if (item.PriceList.Trim().Substring(0, 1) == "T")
                    {
                        priceWatch.DeliveryPeriod = item.ValidFrom.ToString("yyyy-MM-dd") + " - " +
                                                    item.ValidTo.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        priceWatch.DeliveryPeriod = "Spot";
                    }
                    priceWatchList.Add(priceWatch);
                }
            }
            var model = new PriceAlertPageViewModel(currentPage)
            {
                PriceWatchList = priceWatchList
            };
            return View("~/Views/AppPages/PriceAlertPage/Index.cshtml", model);
        }

        [HttpPost]
        [Route("DeletePriceWatch")]
        public async Task<JsonResult> DeletePriceWatch(int id)
        {
            var result = await _agreementRepository.DeletePriceWatchAsync(id, _ticket);
            return Json(new { isRemoved = result });
        }

        private string GetActivityDisplay(string activity, string contractType)
        {
            var activivtyDisplay = "-";
            if (activity.Equals("Meddelande via SMS"))
            {
                return "SMS";
            }

            switch (contractType)
            {
                case "Depåavtal":
                    return "Prissäkra Depå";
                case "Terminsavtal":
                    return "Prissäkra Termin";
                case "Spotavtal":
                    return "Prissäkra Spot";
                case "Poolavtal":
                    return "Prissäkra Pool";
                case "Offertavtal":
                    return "Prissäkra Offert";
            }

            return activivtyDisplay;
        }
    }
}