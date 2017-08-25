using Gro.Core.ContentTypes.Pages.InternalPages;
using Gro.Core.DataModels.Security;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.InternalPages;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Gro.Controllers.Pages.InternalPages
{
    public class InternalStartPageController : SiteControllerBase<InternalStartPage>
    {
        private readonly ICustomerSupportRepository _customerSupportRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly ISecurityRepository _securityRepo;
        private readonly IOrganizationUserRepository _orgUserRepo;

        public InternalStartPageController(ICustomerSupportRepository customerSupportRepo, IAccountRepository accountRepo,
            ISecurityRepository securityRepo, IOrganizationUserRepository orgUserRepo)
        {
            _customerSupportRepo = customerSupportRepo;
            _accountRepo = accountRepo;
            _securityRepo = securityRepo;
            _orgUserRepo = orgUserRepo;
        }

        public async Task<ActionResult> Index(InternalStartPage currentPage, string query)
        {
            var customers = await _customerSupportRepo.GetCustomersByOrganizationNumberAsync(query, true);
            ViewData["query"] = query;
            var viewModel = new InternalStartPageViewModel(currentPage, customers);

            return View("~/Views/InternalPages/StartPage.cshtml", viewModel);
        }

        [Route("api/customer-support/update")]
        [HttpPost]
        public async Task<ActionResult> UpdateCustomer(string organizationNumber, string customerNumber, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(organizationNumber))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, $"{nameof(organizationNumber)} is required");
            }

            var findCustomerResult = await _customerSupportRepo.GetCustomersByOrganizationNumberAsync(organizationNumber, false);
            var customer = findCustomerResult?.FirstOrDefault(c => c.OrganizationNumber == organizationNumber);

            if (customer == null)
            {
                return Json(new { error = $"Could not find the specified organization number {organizationNumber}" });
            }
            if (isActive)
            {
                var verification = await _securityRepo.MatchCustomerNumberAndOrganizationNumberAsync(customerNumber, organizationNumber);
                if (verification == CustomerCheckCode.CustomerNumberNotMatch || verification == CustomerCheckCode.CustomerNumberNotExist)
                {
                    return Json(new { error = "Kunddata stämmer inte. Försök igen eller avvakta." });
                }

                customer.IsActive = true;
                customer.CustomerNumber = customerNumber;
                var user = await UserManager.QuerySiteUserAsync(customer.Email);
                if (user == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, $"Cannot find account {customer.Email}");
                }
                var saveCustomerResult = await _customerSupportRepo.SaveCustomerAsync(customer);

                if (saveCustomerResult == 0)
                {
                    //the new roles include admin's roles and the CustomerOwner role (1)
                    var adminRoles = await _securityRepo.GetRolesOfProfileAsync("Admin");
                    var ownerRoleIds = new string[adminRoles.Length + 1];
                    for (var i = 0; i < ownerRoleIds.Length; i++)
                    {
                        ownerRoleIds[i] = i == adminRoles.Length ? "1" : adminRoles[i].RoleId.ToString();
                    }

                    await _orgUserRepo.AddUserToOrganizationAsync(user, new CustomerBasicInfo
                    {
                        CustomerName = customer.CustomerName,
                        CustomerNo = customer.CustomerNumber,
                    }, ownerRoleIds);
                }
                else
                {
                    Response.StatusCode = 500;
                    return Json(new { error = $"Försök igen eller avvakta med aktiveringsprocessen för denna kund." });
                }
            }
            else
            {
                await _accountRepo.InactivateUserAsync(customer.Email);
            }

            return Json(true);
        }
    }
}
