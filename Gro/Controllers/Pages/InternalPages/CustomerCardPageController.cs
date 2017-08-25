using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gro.Core.ContentTypes.Pages.InternalPages;
using Gro.Core.DataModels.Organization;
using Gro.Core.DataModels.Security;
using Gro.Core.Interfaces;
using Gro.Helpers;
using Gro.ViewModels.Pages.InternalPages;
using Gro.ViewModels.Pages.Organization;
using System.Configuration;
using Gro.ViewModels.Organization;
using System.Linq;
using Gro.Constants;
using Gro.Infrastructure.Data.EmailService;
using Gro.Infrastructure.Data;
using Gro.Business.DataProtection;
using Newtonsoft.Json;

namespace Gro.Controllers.Pages.InternalPages
{
    public class CustomerCardPageController : SiteControllerBase<CustomerCardPage>
    {
        private readonly IOrganizationRepository _organizationRepo;
        private readonly IOrganizationUserRepository _orgUserRepo;
        private readonly ISecurityRepository _securityRepository;
        private readonly ICustomerSupportRepository _customerSupportRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly IEmailService _emailService;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly string _ticket;

        public CustomerCardPageController(
            IOrganizationRepository organizationRepository,
            ISecurityRepository securityRepository,
            IOrganizationUserRepository organizationUserRepository,
            ICustomerSupportRepository customerSupportRepository,
            IEmailService emailService,
            ITokenGenerator tokenGenerator,
            IAccountRepository accountRepo,
            TicketProvider ticketProvider)
        {
            _organizationRepo = organizationRepository;
            _securityRepository = securityRepository;
            _orgUserRepo = organizationUserRepository;
            _customerSupportRepo = customerSupportRepository;
            _emailService = emailService;
            _accountRepo = accountRepo;
            _tokenGenerator = tokenGenerator;
            _ticket = ticketProvider.GetTicket();
        }

        public async Task<ActionResult> Index(CustomerCardPage currentPage, string customerNumber)
        {
            if (string.IsNullOrWhiteSpace(customerNumber)) return new HttpStatusCodeResult(404);

            var model = new CustomerCardPageViewModel(currentPage);
            var customer = string.IsNullOrEmpty(customerNumber) ? null : await _customerSupportRepo.GetCustomerByNumberAsync(customerNumber);
            if (customer == null)
            {
                return View("~/Views/InternalPages/CustomerCardPage/Index.cshtml", model);
            }

            var customerId = _customerSupportRepo.GetCustomerLM2Id(customerNumber);
            var customerBasicInfor = new CustomerBasicInfo {CustomerNo = customerNumber, CustomerId = customerId};

            model.Customer = customer;
            model.InvoiceAddress = await _customerSupportRepo.GetCustomerInvoiceAddress(customerNumber);
            model.DeliveryAddresses = await GetDeliveryAddress(customerId, customerNumber);
            model.Owner = await _orgUserRepo.GetOwnerAsync(customerBasicInfor);

            var users = await _orgUserRepo.GetOrganizationUsersByProfileAsync(customerBasicInfor);
            ViewData["users"] = JsonConvert.SerializeObject(users);

            await Task.WhenAll(this.GetAllRolesTask(_securityRepository), this.GetRolesAndProfiles(_securityRepository));
            ViewData["customerNumber"] = customerNumber;
            return View("~/Views/InternalPages/CustomerCardPage/Index.cshtml", model);
        }

        private async Task<IList<AddressViewModel>> GetDeliveryAddress(int customerId, string customerNumber)
        {
            var result = new List<AddressViewModel>();

            var customerAdresses = customerId > 0
                ? await _organizationRepo.GetCustomersDeliveryAddressesAsync(customerId)
                : await _customerSupportRepo.GetCustomersDeliveryAddressesAsync(customerNumber);
            customerAdresses = customerAdresses ?? new CustomerDeliveryAddress[0];

            foreach (var address in customerAdresses)
            {
                var listNotifcationReceivers = await _organizationRepo.GetDeliveryAddressReceiversAsync(customerId, address.AddressNumber);
                result.Add(OrganizationViewHelper.PopulateAdressModels(address, listNotifcationReceivers, null));
            }
            return result;
        }

        [Route("api/customer-card/users-of-customers/{customerNumber}")]
        public async Task<JsonResult> GetUsers(string customerNumber)
        {
            var customerId = _customerSupportRepo.GetCustomerLM2Id(customerNumber);
            var users = await _orgUserRepo.GetOrganizationUsersByProfileAsync(new CustomerBasicInfo
            {
                CustomerId = customerId,
                CustomerNo = customerNumber
            });
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [Route("api/customer-card/remove-user")]
        [HttpPost]
        public async Task<JsonResult> RemoveUserFromCustomer(string userName, string customerNo)
        {
            var existingUser = await UserManager.QuerySiteUserAsync(userName);
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(customerNo))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            var customer = new CustomerBasicInfo {CustomerNo = customerNo};
            bool isRemoved;
            try
            {
                await _orgUserRepo.RemoveUserFromOrganizationsAsync(existingUser, customer);
                isRemoved = true;
            }
            catch (Exception)
            {
                isRemoved = false;
            }
            return Json(isRemoved, JsonRequestBehavior.AllowGet);
        }

        [Route("api/customer-card/update-roles")]
        [HttpPost]
        public async Task<ActionResult> UpdateUserRoles(string userName, string roles, string customerNumber)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(roles) || string.IsNullOrWhiteSpace(customerNumber))
            {
                return new HttpStatusCodeResult(400);
            }

            var user = await UserManager.QuerySiteUserAsync(userName);
            var roleIds = roles.Split(',');
            var customer = await _customerSupportRepo.GetCustomerByNumberAsync(customerNumber);
            if (customer == null) return new HttpStatusCodeResult(400);

            var cbi = new CustomerBasicInfo
            {
                CustomerName = customer.CustomerName,
                CustomerNo = customer.CustomerNumber
            };

            await _orgUserRepo.UpdateUserCustomerRolesAsync(userName, cbi, roleIds);
            await SendChangedRoleEmailAsync(user.UserName, roles, cbi, user.UserName);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("api/customer-card/add-existing/{customerNumber}")]
        public async Task<ActionResult> AddExistingUserToOrganizationAsync(string userName, string roleIds, string customerNumber)
        {
            var user = await UserManager.QuerySiteUserAsync(userName);
            if (user == null) return new HttpStatusCodeResult(400);

            var roles = roleIds.Split(',');
            var customer = await _customerSupportRepo.GetCustomerByNumberAsync(customerNumber);
            await _orgUserRepo.AddUserToOrganizationAsync(user, new CustomerBasicInfo
            {
                CustomerName = customer.CustomerName,
                CustomerNo = customerNumber
            }, roles);

            //fire and forget email
            var link = $"{ConfigurationManager.AppSettings["domainUrl"]}";
            await SendEmailToExistingUserAsync(user.Email, roleIds, link, customer.CustomerName);

            return Json(true);
        }

        [HttpPost]
        [Route("api/customer-card/add-new/{customerNumber}")]
        public async Task<ActionResult> AddNewUserToOrganizationAsync(AddUserToOrganizationFormViewModel viewModel, string customerNumber)
        {
            var existingUser = await UserManager.QuerySiteUserAsync(viewModel.Email);
            if (existingUser != null) return new HttpStatusCodeResult(400);

            var user = await _accountRepo.CreateUserAsync(viewModel.FirstName, viewModel.LastName, viewModel.Telephone,
                viewModel.Mobile, viewModel.Email, string.Empty, string.Empty, string.Empty);

            if (user == null) throw new ApplicationException("CreateNewFailed");

            var customer = await _customerSupportRepo.GetCustomerByNumberAsync(customerNumber);
            var roles = viewModel.Roles.Split(',');
            await _orgUserRepo.AddUserToOrganizationAsync(user, new CustomerBasicInfo
            {
                CustomerName = customer.CustomerName,
                CustomerNo = customerNumber
            }, roles);

            var guid = await _securityRepository.GeneratePasswordGuidAsync(user.UserName);

            var resetPasswordToken = _tokenGenerator.Encrypt(new ResetPasswordConfirmationData
            {
                GuidString = guid,
                UserName = viewModel.Email
            });

            //fire and forget email
            //var host = Request.Url?.Host + (Request?.Url?.IsDefaultPort == true ? "" : ":" + Request?.Url?.Port);
            var link = $"{ConfigurationManager.AppSettings["publicSitePrefix"]}/p/resetpassword?payload={resetPasswordToken}";
            await SendEmailToNewUserAsync(viewModel.Email, viewModel.Roles, link, customer.CustomerName);

            return Json(true);
        }

        private async Task SendEmailToExistingUserAsync(string email, string roleIds, string loginLink, string organizationName)
        {
            var matchedProfile = await FindMatchedProfileAsync(roleIds);

            var profileTitle = matchedProfile?.Description ?? "Anpassad profil";
            var emailBody = this.RenderPartialViewToString("~/Views/Organization/Email_ExistingUserToOrg.cshtml", new Dictionary<string, string>
            {
                {"profileTitle", profileTitle},
                {"loginLink", loginLink},
                {"organizationName", organizationName}
            });

            await Task.Run(() => _emailService.SendMailAsync(Email.LantmannenFromAddress, new[] {email},
                new string[0], $"Mail till ny användare för Kund: ${organizationName} (AnvändareID existerar redan)", emailBody, _ticket));
        }

        private async Task SendChangedRoleEmailAsync(string email, string roleIds, CustomerBasicInfo customer, string userName)
        {
            var matchedProfile = await FindMatchedProfileAsync(roleIds);

            var profileTitle = matchedProfile?.Description ?? "Anpassad profil";
            var emailBody = this.RenderPartialViewToString("~/Views/Organization/Email_UserRoleChanged.cshtml", new Dictionary<string, string>
            {
                {"profileTitle", profileTitle},
                {"organizationName", customer.CustomerName},
                {"organizationNumber", customer.CustomerNo},
                {"userName", userName}
            });

            await _emailService.SendMailAsync(Email.LantmannenFromAddress, new[] {email},
                new string[0], $"Mail till användare för {customer}: t och i LM\xB2", emailBody, _ticket);
        }

        private async Task SendEmailToNewUserAsync(string email, string roleIds, string resetPasswordLink, string organizationName)
        {
            var matchedProfile = await FindMatchedProfileAsync(roleIds);

            var profileTitle = matchedProfile?.Description ?? "Anpassad profil";
            var emailBody = this.RenderPartialViewToString("~/Views/Organization/Email_NewUserToOrg.cshtml", new Dictionary<string, string>
            {
                {"profileTitle", profileTitle},
                {"resetPasswordLink", resetPasswordLink},
                {"organizationName", organizationName}
            });

            await Task.Run(() => _emailService.SendMailAsync(Email.LantmannenFromAddress, new[] {email},
                new string[0], $"Mail till ny användare för Kund : {organizationName}", emailBody, _ticket));
        }

        private async Task<RoleProfileViewModel> FindMatchedProfileAsync(string roleIds)
        {
            var profiles = await this.GetRolesAndProfiles(_securityRepository);
            var matchedProfile = profiles.FirstOrDefault(p => string.Join(",", p.ProfileRoles.Select(r => r.RoleId)) == roleIds);
            return matchedProfile;
        }
    }
}