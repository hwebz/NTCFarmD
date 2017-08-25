using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Web.Mvc;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Pages.SearchTransport;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.DataModels.SearchTransport;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.SearchTransport;
using SearchCategories = Gro.Constants.SearchTransportCatogories;
using Gro.Business.Caching;

namespace Gro.Controllers.Pages.SearchTransport
{
    [RoutePrefix("api/search-transport")]
    [NoCache]
    public class SearchTransportController : SiteControllerBase<SearchTransportPage>
    {
        private readonly ISearchTransportRepository _searchTransportRepository;
        private readonly IUserManagementService _usersManagementService;
        private static SettingsPage SettingPage => ContentExtensions.GetSettingsPage();

        public SearchTransportController(ISearchTransportRepository searchTransportRepository,
            IUserManagementService usersManagementService)
        {
            _searchTransportRepository = searchTransportRepository;
            _usersManagementService = usersManagementService;
        }

        public ActionResult Index(SearchTransportPage currentPage)
        {
            var siteUser = _usersManagementService.GetSiteUser(HttpContext);
            if (siteUser == null)
            {
                return Redirect("/");
            }

            var searchPage = new SearchTransportPageView(currentPage)
            {
                SearchInfo = new SearchTransportInfo
                {
                    SearchText = string.Empty,
                    FromDate = string.Empty,
                    ToDate = string.Empty,
                    Category = SettingPage.IsInternal ? SearchCategories.CustomerNumber : SearchCategories.Ordernummer
                }
            };
            if (!SettingPage.IsInternal)
            {
                var keyValuePair = searchPage.ListCategory.Find(
                    pair => pair.Key == SearchCategories.CustomerNumber && pair.Value == SearchCategories.CustomerNumber);
                searchPage.ListCategory.Remove(keyValuePair);
                searchPage.IsInternal = false;
            }

            return View("~/Views/SearchTransport/Index.cshtml", searchPage);
        }

        [HttpPost]
        public ActionResult Index(SearchTransportPage currentPage, SearchTransportInfo searchModel)
        {
            var isInternal = SettingPage.IsInternal;
            var supplier = _usersManagementService.GetActiveCustomer(HttpContext);
            var siteUser = _usersManagementService.GetSiteUser(HttpContext);
            if (siteUser == null || supplier == null)
            {
                return Redirect("/");
            }

            var customerNo = supplier.CustomerNo;
            var listShipment = new List<ShipmentResponse>();
            var listOder = new List<OrderRowResponse>();
            switch (searchModel.Category)
            {
                case SearchCategories.CustomerNumber:
                    if (isInternal)
                    {
                        listOder = SearchByCustomerNr(searchModel);
                    }
                    break;
                case SearchCategories.Ordernummer:
                    listOder = SearchByOrderNr(searchModel, customerNo);
                    if (listOder?.Count > 0)
                    {
                        listShipment = GetShipmentsByOrderNr(searchModel, customerNo);
                    }
                    break;
                case SearchCategories.ShipmentId:
                    listShipment = SearchByShipmentId(searchModel, customerNo);
                    if (listShipment?.Count > 0)
                    {
                        listOder = GetOrderRowByShipmentId(searchModel.SearchText, customerNo);
                    }
                    break;
                case SearchCategories.CustomerOrderNumber:
                    listOder = SearchByCustomerOrderNr(searchModel, customerNo);
                    if (listOder?.Count > 0)
                    {
                        listShipment = GetShipmentsByCustomerOrderNr(searchModel, customerNo);
                    }
                    break;
                case SearchCategories.Carrier:
                    listShipment = SearchByCarrier(searchModel, customerNo);
                    break;
                case SearchCategories.WayBill:
                    listShipment = SearchByWaybillNr(searchModel, customerNo);
                    if (listShipment?.Count > 0)
                    {
                        listOder = GetOrderRowByShipmentId(listShipment.First().ShipmentIdMX.ToString(), customerNo);
                    }
                    break;
            }

            var totalOrderedQty = 0;
            var totalDeliveredQty = 0;

            if (listOder != null && listOder.Count > 0)
            {
                totalOrderedQty += listOder.Sum(order => order.OrderedQty);
                totalDeliveredQty += listOder.Sum(order => order.DeliveredQty);
            }

            if (listShipment == null || listOder == null || (listOder.Count == 0 && listShipment.Count == 0))
            {
                ModelState.AddModelError("", "Inga träffar");
            }


            var searchPage = new SearchTransportPageView(currentPage)
            {
                SearchInfo = searchModel,
                ListTransport = listShipment,
                ListOrder = listOder,
                TotalBestKvant = totalOrderedQty,
                TotalLevKvant = totalDeliveredQty
            };
            if (!SettingPage.IsInternal)
            {
                var keyValuePair = searchPage.ListCategory.Find(
                    pair => pair.Key == SearchCategories.CustomerNumber && pair.Value == SearchCategories.CustomerNumber);
                searchPage.ListCategory.Remove(keyValuePair);
                searchPage.IsInternal = false;
            }

            return View("~/Views/SearchTransport/Index.cshtml", searchPage);
        }

        [Route("get-annoncements")]
        [HttpPost]
        public ActionResult GetAnnoncements(string orderRowId)
        {
            int orderId;
            if (!string.IsNullOrWhiteSpace(orderRowId))
            {
                int.TryParse(orderRowId, out orderId);
            }
            else
            {
                orderId = 0;
            }

            var result = _searchTransportRepository.GetAnnoncements(orderId);
            return PartialView("_Annoncements", result);
        }

        #region handling searching

        private List<OrderRowResponse> SearchByCustomerNr(SearchTransportInfo searchModel)
        {
            var customerNr = !string.IsNullOrEmpty(searchModel.SearchText)
                ? searchModel.SearchText.Trim()
                : string.Empty;
            DateTime fromDate;
            if (!DateTime.TryParse(searchModel.FromDate, out fromDate))
                fromDate = new DateTime(1899, 12, 29, 12, 0, 0); //Sql DateTime.MinValue
            DateTime toDate;
            if (!DateTime.TryParse(searchModel.ToDate, out toDate))
                toDate = DateTime.MaxValue;

            var result = _searchTransportRepository.SearchByCustomerNumber(customerNr, fromDate, toDate);
            if (result != null && result.Any())
            {
                return result;
            }
            return new List<OrderRowResponse>();
        }

        private List<OrderRowResponse> SearchByOrderNr(SearchTransportInfo searchModel, string customerNo)
        {
            var orderNr = !string.IsNullOrEmpty(searchModel.SearchText) ? searchModel.SearchText.Trim() : string.Empty;
            var result = _searchTransportRepository.SearchByOrderNumberExternal(orderNr, customerNo);
            return result;
        }

        private List<ShipmentResponse> GetShipmentsByOrderNr(SearchTransportInfo searchModel, string customerNo)
        {
            var orderNr = !string.IsNullOrEmpty(searchModel.SearchText) ? searchModel.SearchText.Trim() : string.Empty;
            var result = _searchTransportRepository.GetShipmentsByOrderNumberExt(orderNr, customerNo);
            return result;
        }

        private List<ShipmentResponse> SearchByShipmentId(SearchTransportInfo searchModel, string customerNo)
        {
            int shipmentIdMx;
            var shipmentIdTxt = !string.IsNullOrEmpty(searchModel.SearchText)
                ? searchModel.SearchText.Trim()
                : string.Empty;
            return int.TryParse(shipmentIdTxt, out shipmentIdMx)
                ? _searchTransportRepository.SearchByShipmentIdMxExternal(shipmentIdMx, customerNo)
                : new List<ShipmentResponse>();
        }

        private List<OrderRowResponse> GetOrderRowByShipmentId(string shipmentIdTxt, string customerNo)
        {
            int shipmentIdMx;
            int.TryParse(shipmentIdTxt?.Trim(), out shipmentIdMx);
            var result = _searchTransportRepository.GetOrderRowByShipmentId(shipmentIdMx, customerNo);
            return result;
        }

        private List<OrderRowResponse> SearchByCustomerOrderNr(SearchTransportInfo searchModel, string customerNo)
        {
            var customerOrderNumber = !string.IsNullOrEmpty(searchModel.SearchText)
                ? searchModel.SearchText.Trim()
                : string.Empty;
            var result = _searchTransportRepository.SearchByCustomerOrderNumberExternal(customerOrderNumber, customerNo);

            return result;
        }

        private List<ShipmentResponse> GetShipmentsByCustomerOrderNr(SearchTransportInfo searchModel, string customerNo)
        {
            var customerOrderNumber = !string.IsNullOrEmpty(searchModel.SearchText)
                ? searchModel.SearchText.Trim()
                : string.Empty;
            var result = _searchTransportRepository.GetShipmentsByCustomerOrderNumber(customerOrderNumber, customerNo);

            return result;
        }

        private List<ShipmentResponse> SearchByCarrier(SearchTransportInfo searchModel, string customerNo)
        {
            var carrier = !string.IsNullOrEmpty(searchModel.SearchText) ? searchModel.SearchText.Trim() : string.Empty;
            DateTime fromDate;
            if (!DateTime.TryParse(searchModel.FromDate, out fromDate))
                fromDate = new DateTime(1899, 12, 29, 12, 0, 0); //Sql DateTime.MinValue
            DateTime toDate;
            if (!DateTime.TryParse(searchModel.ToDate, out toDate))
                toDate = DateTime.MaxValue;

            var result = _searchTransportRepository.SearchByCarrierExternal(carrier, customerNo, fromDate, toDate);

            return result;
        }

        private List<ShipmentResponse> SearchByWaybillNr(SearchTransportInfo searchModel, string customerNo)
        {
            var wayBillNumber = !string.IsNullOrEmpty(searchModel.SearchText)
                ? searchModel.SearchText.Trim()
                : string.Empty;
            var result = _searchTransportRepository.SearchByWayBillNumberExternal(wayBillNumber, customerNo);
            return result;
        }

        #endregion
    }
}