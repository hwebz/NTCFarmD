using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using Gro.Business.Services.Users;
using Gro.Constants;
using Gro.Core.ContentTypes.Pages.StartPages;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.DataModels.CustomerSupport;
using Gro.Core.DataModels.Security;
using Gro.Core.Interfaces;
using Gro.ViewModels;

namespace Gro.Controllers.Pages.InternalPages
{
    public class InternalSearchController : Controller
    {
        private readonly ICustomerSupportRepository _customerSupportRepo;
        private readonly IContentRepository _contentRepo;
        private readonly IUserManagementService _userManagementService;

        public InternalSearchController(ICustomerSupportRepository customerSupportRepo, IContentRepository contentRepo, IUserManagementService userManagementService)
        {
            _customerSupportRepo = customerSupportRepo;
            _contentRepo = contentRepo;
            _userManagementService = userManagementService;
        }

        public ActionResult RenderSearchHeader()
        {
            var customer = _userManagementService.GetInternalCustomerNumber(HttpContext);
            if (customer != null)
            {
                TempData["internalCustomer"] = customer;
            }
            return PartialView("~/Views/InternalPages/_Internal_Search.cshtml");
        }

        [Route("internal-portal/close-customer-session")]
        public JsonResult CloseCustomerSession()
        {
            var isEnded = _userManagementService.EndInternalCustomerSession(HttpContext);
            return Json(isEnded, JsonRequestBehavior.AllowGet);
        }

        [Route("internal-portal/no-result")]
        public ActionResult NoResult(string customerNumber)
        {
            var model = GetDefaultPageViewModel();
            model.CurrentPage.Name = "Internal Search";
            TempData["noCustomerNumber"] = customerNumber;
            return View("~/Views/InternalPages/_NoSearchResult.cshtml", model);
        }

        private PageViewModel<StartPage> GetDefaultPageViewModel()
        {
            var startPage = _contentRepo.Get<StartPage>(ContentReference.StartPage);
            var startPageClone = (StartPage)startPage.CreateWritableClone();
            return new PageViewModel<StartPage>(startPageClone);
        }

        [Route("api/internal-portal/search")]
        [HttpPost]
        public async Task<ActionResult> UpdateCustomer(SearchOptions searchOption, string searchKey)
        {
            if (searchOption == SearchOptions.CustomerNumber && !string.IsNullOrEmpty(searchKey))
            {
                Customer customer;
                try
                {
                    customer = await _customerSupportRepo.GetCustomerByNumberAsync(searchKey);
                }
                catch (Exception)
                {
                    customer = null;
                }

                if (customer == null)
                {
                    return RedirectToAction("NoResult", new { customerNumber = searchKey });
                }

                // Save the customer as the [InternalActiveCustomer]
                _userManagementService.UpdateInternalCustomerNumber(HttpContext, new CustomerBasicInfo()
                {
                    CustomerNo = searchKey,
                    CustomerName = customer.CustomerName
                });

                var settingsPage = ContentExtensions.GetSettingsPage();
                var customerCardPage = settingsPage?.CustomerCardPage;
                if (customerCardPage == null)
                {
                    return RedirectToAction("NoResult", new { customerNumber = searchKey });
                }
                return RedirectToAction("Index", new { node = customerCardPage, customerNumber = searchKey });
            }

            return RedirectToAction("NoResult", new { customerNumber = searchKey });
        }

    }
}
