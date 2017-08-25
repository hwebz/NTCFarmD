using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using EPiServer.Framework.DataAnnotations;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.AppPages;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.DataModels.ShippingDtos;
using Gro.Core.Interfaces;
using Gro.Helpers;
using Gro.ViewModels.Pages.AppPages.CalculateDeliveryFee;
using Gro.Infrastructure.Data;
using Gro.Core.DataModels.DeliveryAssuranceDtos;

namespace Gro.Controllers.Pages.AppPages
{
    [SessionState(SessionStateBehavior.Disabled)]
    [TemplateDescriptor(Inherited = true)]
    [CustomerRole]
    public class CalculateDeliveryFeePageController : SiteControllerBase<CalculateDeliveryFeePage>
    {
        private readonly IDeliveryAssuranceRepository _deliveryAssuranceRepository;
        private readonly IShippingRepository _shippingRepository;
        private readonly string _ticket;

        public CalculateDeliveryFeePageController(
            IDeliveryAssuranceRepository deliveryAssuranceRepository,
            IShippingRepository shippingRepository,
            IUserManagementService userManager,
            TicketProvider ticketProvider) : base(userManager)
        {
            _deliveryAssuranceRepository = deliveryAssuranceRepository;
            _shippingRepository = shippingRepository;
            _ticket = ticketProvider.GetTicket();
        }

        public ActionResult Index(CalculateDeliveryFeePage currentPage)
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);
            ViewData["supplier"] = supplier != null ? supplier.CustomerNo : string.Empty;
            if (string.IsNullOrWhiteSpace(supplier?.CustomerNo))
            {
                return View("Index", new CalculateDeliveryFeePageViewModel(currentPage, new Item[0], new DeliveryAddress[0],
                        new Item[0]));
            }

            var listLorryTypes = _deliveryAssuranceRepository.GetLorryTypes();
            var listDeliveryAddresses = _deliveryAssuranceRepository.GetDeliveryAdresses(supplier.CustomerNo);

            // at the migration phase, skip all business for the internal user
            // TODO: Implement business for the internl users later.
            var listMergedItems = _deliveryAssuranceRepository.GetMergedItems(supplier.CustomerNo,
                DateTime.Now.Date, true);

            var model = new CalculateDeliveryFeePageViewModel(currentPage, listLorryTypes, listDeliveryAddresses,
                listMergedItems)
            {
                SupplierId = supplier.CustomerNo
            };

            return View("Index", model);
        }

        [Route("api/deliveryfee/caluculate")]
        [HttpPost]
        public async Task<ActionResult> CaluculateDeliveryFee(string supplier, string lorryType,
            string deliveryAddressId, string quantity, string deliveryDate, string itemId)
        {
            var deliveryFeeRequest = PopulateDeliveryFeeRequest(supplier, lorryType, deliveryAddressId, quantity,
                deliveryDate, itemId);

            var deliveryFeeResponse = await _shippingRepository.GetDeliveryFeeAsync(deliveryFeeRequest, _ticket);

            ViewBag.IsInternal = ContentExtensions.GetSettingsPage()?.IsInternal;
            return PartialView("~/Views/AppPages/CalculateDeliveryFeePage/CalculationResult.cshtml", deliveryFeeResponse);
        }

        private static DeliveryFeeRequest PopulateDeliveryFeeRequest(string supplier, string lorryType,
            string deliveryAddressId, string quantity, string deliveryDate, string itemId)
        {
            // TODO: need to implement business about "Sort" for DeliveryFeeRequest obj
            double quantityDbl;
            double.TryParse(quantity, out quantityDbl);

            DateTime deliveryDateVal;
            if (!DateTime.TryParse(deliveryDate, out deliveryDateVal))
            {
                deliveryDateVal = DateTime.Now.Date;
            }

            return new DeliveryFeeRequest()
            {
                Item = itemId ?? string.Empty,
                DeliveryAddress = deliveryAddressId ?? string.Empty,
                Quantity = quantityDbl.ConvertToKg(),
                Leveransdatum = deliveryDateVal.ToStandardDateTimeString(),
                Suppliernumber = supplier ?? string.Empty,
                Levsatt = lorryType ?? string.Empty
            };
        }
    }
}
