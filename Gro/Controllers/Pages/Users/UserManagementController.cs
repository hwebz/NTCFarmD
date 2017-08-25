using System.Linq;
using System.Web.Mvc;
using EPiServer;
using Gro.Business.Services.Users;
using Gro.Core.DataModels.Security;
using Gro.Core.Interfaces;
using Gro.ViewModels.Users;
using Gro.Helpers;

namespace Gro.Controllers.Pages.Users
{
    public class UserManagementController : Controller
    {
        private readonly IUserManagementService _usersManagementService;
        private readonly IOrganizationUserRepository _orgUserRepo;

        public UserManagementController(IContentRepository contentRepository,
            IUserManagementService usersManagementService, IOrganizationUserRepository orgUserRepo)
        {
            _usersManagementService = usersManagementService;
            _orgUserRepo = orgUserRepo;
        }

        public ActionResult RenderCustomerNumbers()
        {
            var userName = _usersManagementService.GetSiteUser(HttpContext)?.UserName;
            if (string.IsNullOrWhiteSpace(userName))
            {
                return PartialView("_CustomerNumbers", new CustomerListViewModel(null, null));
            }

            var activeCustomer = _usersManagementService.GetActiveCustomer(HttpContext);
            var organizationList = _orgUserRepo.GetOrganizationsOfUser(userName);
            organizationList = organizationList ?? new CustomerBasicInfo[0];

            var activeCustomerInfor = organizationList.FirstOrDefault(x => x.CustomerNo == activeCustomer.CustomerNo);
            activeCustomerInfor = activeCustomerInfor ?? new CustomerBasicInfo();

            return PartialView("_CustomerNumbers", new CustomerListViewModel(activeCustomerInfor, organizationList));
        }

        [HttpPost]
        public ActionResult ChangeCustomer(string customerNumber)
        {
            _usersManagementService.CustomerExistsForUser(HttpContext, customerNumber);
            var url = Request.UrlReferrer?.AbsoluteUri;

            return Redirect(url);
        }

        [Route("api/customer-user/change-customer")]
        [HttpPost]
        public ActionResult UpdateCustomer(string customerNumber, string referenceLink)
        {
            var customer = _usersManagementService.CustomerExistsForUser(HttpContext, customerNumber);
            if (customer == null) return new HttpStatusCodeResult(400);

            var siteUser = _usersManagementService.GetSiteUser(HttpContext);
            siteUser.ActiveCustomerNumber = customer.CustomerNo;
            this.SetUserSession(siteUser);
            return Redirect(referenceLink);
        }

        [Route("api/term-of-use/accept")]
        public ActionResult UpdateInsertUserAccepts(string term, int version)
        {
            if (string.IsNullOrEmpty(term) || version <= 0) return Content(true.ToString());

            var currentUser = _usersManagementService.GetSiteUser(HttpContext);
            if (currentUser == null) return Content(true.ToString());
            var updatedResult = _usersManagementService.UpdateInsertUserAccepts(term, version, currentUser.UserId);

            return Content(updatedResult.ToString());
        }

        [Route("api/term-of-use/force-log-out")]
        public ActionResult ForceLogOut()
        {
            this.SignoutSiteUser();
            return Content("");
        }
    }
}
