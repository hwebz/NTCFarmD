using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.BokaPages;
using Gro.Core.DataModels.Boka.ListingBoka;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.Boka;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Controllers.Pages.Boka
{
    [RoutePrefix("api/listing-booking")]
    public class ListBookingsController : SiteControllerBase<BokaListingPage>
    {

        private readonly IBokaRepository _bokaRepository;
        private readonly IUserManagementService _usersManagementService;

        private static SettingsPage SettingPage => ContentExtensions.GetSettingsPage();

        public ListBookingsController(IBokaRepository bokaRepository, IUserManagementService usersManagementService)
        {
            _bokaRepository = bokaRepository;
            _usersManagementService = usersManagementService;
        }

        public ActionResult Index(BokaListingPage currentPage)
        {
            var supplier = _usersManagementService.GetActiveCustomer(HttpContext);
            if (supplier == null)
            {
                var model = new BokaListingPageView(currentPage)
                {
                    Customer = new CustomerInfo()
                };

                return View("~/Views/Boka/BokaListing.cshtml", model);
            }
            var resourceList = _bokaRepository.GetResourceGroupList(true);

            var customerInfoList = _bokaRepository.GetCustomers(supplier.CustomerNo, string.Empty, SettingPage.IsInternal, SiteUser.UserName ?? string.Empty);
            var customerInfor = new CustomerInfo();
            if (customerInfoList?.Count >= 1)
            {
                customerInfor.OwnerEmail = customerInfoList[0].Email;
                customerInfor.CustomerNo = customerInfoList[0].CustomerNo;
                customerInfor.CustomerName = customerInfoList[0].Name;
                customerInfor.PhoneNumber = customerInfoList[0].MobileNo;
            }

            var bokaListingPage = new BokaListingPageView(currentPage)
            {
                ResourceItemList = resourceList,
                Customer = customerInfor
            };

            return View("~/Views/Boka/BokaListing.cshtml", bokaListingPage);
        }
        [Route("SearchBookings")]
        [HttpPost]
        public ActionResult SearchBookings(RequestSearchBookings request)
        {
            var supplier = _usersManagementService.GetActiveCustomer(HttpContext);
            if (supplier == null) return Json(new { resultSearchBookings = string.Empty });

            DateTime fromDateTime;
            DateTime toDateTime;

            var result = new List<SearchBookingsDto>();
            if (DateTime.TryParse(request.FromDate, out fromDateTime) && DateTime.TryParse(request.ToDate, out toDateTime))
            {
                if ((toDateTime - fromDateTime).TotalDays <= 7)
                {
                    result = _bokaRepository.SearchBookings(request);
                }
            }

            var groupBookings = result?
                    .GroupBy(m => new { m.FromDate })
                    .ToDictionary(s => s.Key.FromDate, t => t.ToList());
            return PartialView("~/Views/Boka/_TableSearchBookingResult.cshtml", groupBookings);
        }
    }
}
