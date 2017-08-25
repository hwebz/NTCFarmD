using System.Web.Mvc;
using Gro.Core.ContentTypes.Pages.BokaPages;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.Boka;
using Gro.Business.Services.Users;
using Gro.Core.DataModels.Boka;
using System;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.DataModels.Boka.UpdateReservation;

namespace Gro.Controllers.Pages.Boka
{
    [RoutePrefix("api/time-booking")]
    public class BokaController : SiteControllerBase<BokaPage>
    {
        private readonly IBokaRepository _bokaRepository;
        private readonly IUserManagementService _usersManagementService;

        private static SettingsPage SettingPage => ContentExtensions.GetSettingsPage();

        public BokaController(IBokaRepository bokaRepository, IUserManagementService usersManagementService)
        {
            _bokaRepository = bokaRepository;
            _usersManagementService = usersManagementService;
        }
        public ActionResult Index(BokaPage currentPage)
        {
            var supplier = _usersManagementService.GetActiveCustomer(HttpContext);
            if (supplier == null)
            {
                var model = new BokaPageView(currentPage)
                {
                    Customer = new CustomerInfo(),
                };
                if (!SettingPage.IsInternal)
                {
                    if (model.SearchTypes?.Count > 4)
                    {
                        model.SearchTypes.RemoveRange(2, 2);
                    }
                }
                return View("~/Views/Boka/Index.cshtml", model);
            }
            var ion = string.Empty;
            if (Request.QueryString.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["ion"]))
                {
                    ion = Request.QueryString["ion"];
                }
            }
            
            var resourceList = _bokaRepository.GetResourceGroupList(false);
            var customerInfoList = _bokaRepository.GetCustomers(supplier.CustomerNo, string.Empty, SettingPage.IsInternal, SiteUser.UserName ?? string.Empty);

            var customerInfor = new CustomerInfo();
            if (customerInfoList?.Count >= 1)
            {
                customerInfor.OwnerEmail = customerInfoList[0].Email;
                customerInfor.CustomerNo = customerInfoList[0].CustomerNo;
                customerInfor.CustomerName = customerInfoList[0].Name;
                customerInfor.PhoneNumber = customerInfoList[0].MobileNo;
            }
            var bokaPage = new BokaPageView(currentPage)
            {
                ResourceItemList = resourceList,
                Customer = customerInfor,
                SeachValue = ion

            };
            if (!SettingPage.IsInternal)
            {
                if (bokaPage.SearchTypes?.Count > 4)
                {
                    bokaPage.SearchTypes.RemoveRange(2, 2);
                }
            }
            return View("~/Views/Boka/Index.cshtml", bokaPage);
        }

        [Route("loadItemsOnresourceGroup")]
        [HttpPost]
        public JsonResult LoadItemsOnresourceGroup(string resourceGroupId, string selectedDate, string currentArticleItem, bool showOnlyUnloadingItems)
        {
            var result = _bokaRepository.LoadItemsOnresourceGroup(resourceGroupId, selectedDate, currentArticleItem, showOnlyUnloadingItems);
            if (result == null) return Json(new { Items = string.Empty, Vehicles = string.Empty, ReservationStops = string.Empty });

            var jsItems = result.Items.ToArray();
            var jsVehicles = result.Vehicles.ToArray();
            var jsReservationStops = result.ReservationStops.ToArray();
            return Json(new { Items = jsItems, Vehicles = jsVehicles, ReservationStops = jsReservationStops });
        }

        [Route("SearchAvailbleSlots")]
        [HttpPost]
        public ActionResult SearchAvailbleSlots(string resourceGroupId, string selectedDate, string article, string qty, string loadunload, string veichleType, string driedUnDried, string customerNo, string searchType)
        {
            var result = _bokaRepository.SearchAvailbleSlots(resourceGroupId, selectedDate, article, qty, loadunload, veichleType, driedUnDried, customerNo, searchType);
            return PartialView("~/Views/Boka/_TableTimeBookingResult.cshtml", result);
        }

        [Route("ExistBooking")]
        [HttpPost]
        public JsonResult ExistBooking(string resourceGroupId,
                                        string selectedDate,
                                        string article,
                                        string qty,
                                        string loadunload,
                                        string veichleType,
                                        string driedUnDried,
                                        string customerNo,
                                        string searchType,
                                        string iONumber)
        {
            var result = _bokaRepository.ExistBooking(resourceGroupId, selectedDate, article, qty, loadunload, veichleType, driedUnDried, customerNo, searchType, iONumber);
            return Json(new { IsExistBooking = result });
        }
        [Route("MakeReservation")]
        [HttpPost]
        public JsonResult MakeReservation(MakeReservationDto reservationToMake)
        {
            var supplier = _usersManagementService.GetActiveCustomer(HttpContext);
            if (supplier == null) return Json(new { makeReservations = string.Empty });

            var result = _bokaRepository.MakeReservation(reservationToMake, supplier.CustomerNo);
            return Json(new { makeReservations = result.ToArray() });
        }

        [Route("DeleteReservation")]
        [HttpPost]
        public JsonResult DeleteReservation(string reservationId, string owner, string customerNo, string dateRegistered)
        {
            var result = _bokaRepository.DeleteReservation(reservationId, owner, customerNo, Convert.ToDateTime(dateRegistered));

            return result == null ? Json(new { deleteReservations = string.Empty }) : Json(new { deleteReservations = result.ToArray() });
        }

        [Route("UpdateReservation")]
        [HttpPost]
        public JsonResult UpdateReservation(UpdateReservationDto reservationToUpdate)
        {
            var supplier = _usersManagementService.GetActiveCustomer(HttpContext);
            var customerNo = supplier != null ? supplier.CustomerNo : string.Empty;
            var result = _bokaRepository.UpdateReservation(reservationToUpdate, customerNo);
            return result == null ? Json(new { updateReservations = string.Empty }) : Json(new { updateReservations = result.ToArray() });
        }
        [Route("LoadResourceGroupsOnIO")]
        [HttpPost]
        public JsonResult LoadResourceGroupsOnIo(string warehouseId)
        {
            var result = _bokaRepository.GetResourceGroupsOnIo(warehouseId);
            return result != null ? Json(new { Resource = result.ToArray() }) : Json(new { Resource = string.Empty });
        }

        [Route("CustomerSearch")]
        [HttpPost]
        public JsonResult CustomerSearch(string resourceGroupId, string searchString, string searchType, string loadOrUnlodValue, string customerSearcType)
        {
            var supplier = _usersManagementService.GetActiveCustomer(HttpContext);
            if (supplier == null) return Json(new { customerResult = string.Empty });
            var result = _bokaRepository.CustomerSearch(resourceGroupId, searchString, searchType, loadOrUnlodValue, customerSearcType, supplier.CustomerNo, SiteUser.UserName ?? string.Empty);
            return result != null ? Json(new { customerResult = result }) : Json(new { customerResult = string.Empty });
        }
        
    }
}
