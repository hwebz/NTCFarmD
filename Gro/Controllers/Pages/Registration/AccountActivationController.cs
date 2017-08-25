using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.Registration;
using Gro.Core.Interfaces;
using Gro.ViewModels;
using Gro.Core.DataModels.Security;
using EPiServer.Web.Mvc;
using Gro.Constants;
using Gro.Helpers;
using Gro.Infrastructure.Data;
using Gro.Infrastructure.Data.EmailService;
using System.Text.RegularExpressions;
using log4net;
using Gro.ViewModels.Registration;
using Gro.Core.ContentTypes.Pages.MyProfile;

namespace Gro.Controllers.Pages.Registration
{
    public class AccountActivationController : PageController<AccountActivationPage>
    {
        private readonly ISecurityRepository _securityRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly IOrganizationUserRepository _orgUserRepo;
        private readonly IOrganizationRepository _orgRepo;
        private readonly IEmailService _emailService;
        private readonly TicketProvider _ticketProvider;
        private readonly IUserManagementService _userManager;
        private readonly ICustomerSupportRepository _customerSupportRepo;

        private static readonly Regex CustomerNumberRegex = new Regex(@"^\d+$");
        private static readonly Regex OrganizationNumberRegex = new Regex(@"^\d{10}$");
        private static readonly Regex PrivateFirmRegex = new Regex(@"^\d{2}[0-1]\d{7}$");
        private static readonly Regex NonPrivateFirmRegex = new Regex(@"^\d{2}[2-9]\d{7}$");

        private const string organizationNumberViewKey = "organizationNumber";
        private const string customerNumberViewKey = "customerNumber";

        private readonly ILog _logger = LogManager.GetLogger(nameof(AccountActivationController));

        public AccountActivationController(
            ISecurityRepository securityRepository,
            IUserManagementService usersManagementService,
            IAccountRepository accountRepo,
            IOrganizationUserRepository orgUserRepo,
            IOrganizationRepository orgRepo,
            IEmailService emailService,
            IUserManagementService userManager,
            ICustomerSupportRepository customerSupportRepo,
            TicketProvider ticketProvider)
        {
            _securityRepo = securityRepository;
            _accountRepo = accountRepo;
            _orgUserRepo = orgUserRepo;
            _orgRepo = orgRepo;
            _emailService = emailService;
            _ticketProvider = ticketProvider;
            _userManager = userManager;
            _customerSupportRepo = customerSupportRepo;
        }

        public ActionResult Index(AccountActivationPage currentPage)
            => View("~/Views/Registration/Activation/Verification.cshtml", new PageViewModel<AccountActivationPage>(currentPage));

        [HttpPost]
        public async Task<ActionResult> Index(AccountActivationPage currentPage, string customerNumber, string organizationNumber)
        {
            switch (Request.Form["Action"])
            {
                default:
                    break;
                case "PrivateFirm":
                    return await PrivateFirm(currentPage, PrivateFirmActivationForm.FromRequest(Request));
                case "NonPrivateFirm":
                    return await NonPrivateFirm(currentPage, NonPrivateFirmActivationForm.FromRequest(Request));
            }

            var organisationPureNumber = organizationNumber.Trim().Replace("-", "");
            ViewData[customerNumberViewKey] = customerNumber = customerNumber.Trim();
            ViewData[organizationNumberViewKey] = organizationNumber = organizationNumber.Trim();
            var formatValid = true;
            if (!CustomerNumberRegex.IsMatch(customerNumber))
            {
                formatValid = false;
                ViewData["errorCustomer"] = "Kundnummeret kan bara bestå av siffror";
            }
            if (!OrganizationNumberRegex.IsMatch(organisationPureNumber))
            {
                formatValid = false;
                ViewData["errorOrganisation"] = "Organisationsnumret måste bestå av 10 sifrror";
            }

            if (!formatValid)
            {
                return View("~/Views/Registration/Activation/Verification.cshtml", new PageViewModel<AccountActivationPage>(currentPage));
            }

            var matchCode = await _securityRepo.MatchCustomerNumberAndOrganizationNumberAsync(customerNumber, organizationNumber);

#if DEBUG
            matchCode = CustomerCheckCode.SoleTrader;
#endif

            switch (matchCode)
            {
                case CustomerCheckCode.CustomerNumberNotMatch:
                    {
                        ViewData["errorCode"] = "Felkod: 1";
                        return VerificationError(currentPage);
                        //ViewData["errorCustomer"] = "Kundnumret du har angett finns inte registrerat hos Lantmännen för det organisationsnummer du har angett";
                        //ViewData["errorOrganisation"] = "Organisationsnumret du har angett stämmer inte överens med kundnumret du har angett";
                    }
                case CustomerCheckCode.CustomerNumberNotExist:
                    {
                        ViewData["errorCode"] = "Felkod: 2";
                        return VerificationError(currentPage);
                        //ViewData["errorCustomer"] = "Kundnumret finns inte registrerat hos Lantmännen.";
                    }
                case CustomerCheckCode.CustomerNumberActivated:
                    {
                        ViewData["errorCode"] = "Felkod: 3";
                        return VerificationError(currentPage);
                        //ViewData["errorCustomer"] = "Kundnumret och organisationsnumret  du har angett är redan aktiverat för ett konto i LM\xB2. ";
                        //ViewData["errorOrganisation"] = "Organisationsnumret och kundnumret du har angett är redan aktiverat för ett konto i LM\xB2.";
                    }
                case CustomerCheckCode.Underkund:
                    {
                        ViewData["errorCode"] = "Felkod: 4";
                        return VerificationError(currentPage);
                    }
                    //Case If proxy customer/Underkund tries to activate customer number
            }

            ViewData[customerNumberViewKey] = customerNumber;
            ViewData[organizationNumberViewKey] = organizationNumber;

            //if (!NonPrivateFirmRegex.IsMatch(organisationPureNumber))
            //{
            return View("~/Views/Registration/Activation/ChooseBankIdPage.cshtml", new PageViewModel<AccountActivationPage>(currentPage));
            //}

            //            var existingRegistration = await _orgRepo.GetExistingRegistrationAsync(customerNumber, organisationPureNumber);
            //#if DEBUG
            //#else
            //            if (existingRegistration == null) return new HttpStatusCodeResult(400);
            //#endif
            //            ViewData["email"] = existingRegistration?.User_Email ?? existingRegistration?.Contact_Email;
            //            ViewData["firstName"] = existingRegistration?.User_FirstName;
            //            ViewData["lastName"] = existingRegistration?.User_LastName;
            //            ViewData["customerName"] = existingRegistration?.Customer_Name;
            //return View("~/Views/Registration/Activation/NonPrivateFirm.cshtml", new PageViewModel<AccountActivationPage>(currentPage));
        }

        [HttpGet]
        public async Task<ActionResult> ActivationForm(AccountActivationPage currentPage, string customerNumber, string organizationNumber)
        {
            var organisationPureNumber = organizationNumber.Trim().Replace("-", "");
            ViewData[customerNumberViewKey] = customerNumber = customerNumber.Trim();
            ViewData[organizationNumberViewKey] = organizationNumber.Trim();
            ViewData["serialNumber"] = HttpContext.GetSerialNumber();
            if (!CustomerNumberRegex.IsMatch(customerNumber) || !OrganizationNumberRegex.IsMatch(organisationPureNumber))
            {
                return new HttpStatusCodeResult(400);
            }
            var existingRegistration = await _orgRepo.GetExistingRegistrationAsync(customerNumber, organisationPureNumber);
#if DEBUG
#else
            if (existingRegistration == null) return new HttpStatusCodeResult(400);
#endif
            ViewData["firstName"] = existingRegistration?.User_FirstName;
            ViewData["lastName"] = existingRegistration?.User_LastName;
            ViewData["email"] = existingRegistration?.Customer_Email ?? existingRegistration?.User_Email;
            ViewData["customerName"] = existingRegistration?.Customer_Name;

            if (PrivateFirmRegex.IsMatch(organisationPureNumber))
            {
                return View("~/Views/Registration/Activation/PrivateFirm.cshtml",
                    new PageViewModel<AccountActivationPage>(currentPage));
            }
            if (NonPrivateFirmRegex.IsMatch(organisationPureNumber))
            {
                return View("~/Views/Registration/Activation/NonPrivateFirm.cshtml",
                    new PageViewModel<AccountActivationPage>(currentPage));
            }

            return new HttpStatusCodeResult(400);
        }

        private async Task<ActionResult> PrivateFirm(AccountActivationPage currentPage, PrivateFirmActivationForm viewModel)
        {
            if (!TryValidateModel(viewModel) || string.IsNullOrWhiteSpace(viewModel.SerialNumber))
            {
                _logger.Error("ModelState is invalid");
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                _logger.Info(message);
                return new HttpStatusCodeResult(400);
            }

            var userId = await _securityRepo.GetPersonObjectIdByNameAsync(viewModel.Email);
            if (!string.IsNullOrWhiteSpace(userId))
            {
                ViewData[customerNumberViewKey] = viewModel.CustomerNumber;
                ViewData[organizationNumberViewKey] = viewModel.OrganizationNumber;
                ViewData["firstName"] = viewModel?.FirstName;
                ViewData["lastName"] = viewModel?.LastName;
                ViewData["email"] = viewModel?.Email;
                ViewData["emailError"] = $"Det finns redan en användare registrerad på den här e-postadressen";
                return View("~/Views/Registration/Activation/PrivateFirm.cshtml", new PageViewModel<AccountActivationPage>(currentPage));
            }

            var matchCode = await _securityRepo.MatchCustomerNumberAndOrganizationNumberAsync(viewModel.CustomerNumber, viewModel.OrganizationNumber);
            switch (matchCode)
            {
                case CustomerCheckCode.CustomerNumberActivated:
                    //conflict
                    _logger.Error("Customer number has already been activated");
                    return new HttpStatusCodeResult(409);
                case CustomerCheckCode.CustomerNumberNotExist:
                case CustomerCheckCode.CustomerNumberNotMatch:
                    _logger.Error("Customer number is invalid or does not match");
                    return new HttpStatusCodeResult(400);
            }

            var existingRegistration = await _orgRepo.GetExistingRegistrationAsync(viewModel.CustomerNumber, viewModel.OrganizationNumber);
            if (existingRegistration == null)
            {
                _logger.Error("No existing registration found");
                return new HttpStatusCodeResult(400);
            }

            string personNumber;
            if (!IsBankIdMatchingRegistration(viewModel.SerialNumber, viewModel.OrganizationNumber, out personNumber))
            {
                _logger.Error($"existing registration {viewModel.OrganizationNumber} did not match serial number {viewModel.SerialNumber}");
                return new HttpStatusCodeResult(400);
            }

            var newUser = await _accountRepo.CreateUserAsync(viewModel.FirstName, viewModel.LastName, viewModel.Telephone, viewModel.Mobilephone,
                viewModel.Email, string.Empty, string.Empty, string.Empty, personNumber, viewModel.CustomerNumber, false);

            ViewData["email"] = newUser.Email;

            var ownerRoles = _securityRepo.GetRolesOfProfileAsync("Admin").Result.Select(x => x.RoleId.ToString()).ToArray();

            await _orgUserRepo.UpdateUserCustomerRolesAsync(newUser.UserName, new CustomerBasicInfo
            {
                CustomerName = string.Empty,
                CustomerNo = viewModel.CustomerNumber,
            }, ownerRoles);

            await SendActivationEmail(newUser);

            //let the user accept the agreement
            AcceptUserAgreement(newUser, currentPage.UserAgreementPageReference);

            ViewData["reference"] = nameof(PrivateFirm);
            return View("~/Views/Registration/Activation/Finish.cshtml", new PageViewModel<AccountActivationPage>(currentPage));
        }

        private async Task<ActionResult> NonPrivateFirm(AccountActivationPage currentPage, NonPrivateFirmActivationForm viewModel)
        {
            if (!TryValidateModel(viewModel) || string.IsNullOrWhiteSpace(viewModel.SerialNumber))
            {
                _logger.Error("ModelState is invalid");
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                _logger.Info(message);
                return new HttpStatusCodeResult(400);
            }

            var userId = await _securityRepo.GetPersonObjectIdByNameAsync(viewModel.Email);
            if (!string.IsNullOrWhiteSpace(userId))
            {
                ViewData[customerNumberViewKey] = viewModel.CustomerNumber;
                ViewData[organizationNumberViewKey] = viewModel.OrganizationNumber;
                ViewData["firstName"] = viewModel?.FirstName;
                ViewData["lastName"] = viewModel?.LastName;
                ViewData["email"] = viewModel?.Email;
                ViewData["emailError"] = $"Det finns redan en användare registrerad på den här e-postadressen";
                return View("~/Views/Registration/Activation/NonPrivateFirm.cshtml", new PageViewModel<AccountActivationPage>(currentPage));
            }

            var matchCode = await _securityRepo.MatchCustomerNumberAndOrganizationNumberAsync(viewModel.CustomerNumber, viewModel.OrganizationNumber);
            switch (matchCode)
            {
                case CustomerCheckCode.CustomerNumberActivated:
                    //conflict
                    _logger.Error("Customer is already activated");
                    return new HttpStatusCodeResult(409);
                case CustomerCheckCode.CustomerNumberNotExist:
                case CustomerCheckCode.CustomerNumberNotMatch:
                    _logger.Error("Customer numbers mismatched");
                    return new HttpStatusCodeResult(400);
            }

            var existingRegistration = await _orgRepo.GetExistingRegistrationAsync(viewModel.CustomerNumber, viewModel.OrganizationNumber);
            if (existingRegistration == null)
            {
                _logger.Error("No existing registration found");
                return new HttpStatusCodeResult(400);
            }

            existingRegistration.User_SocialSecurity = viewModel.SerialNumber;
            existingRegistration.User_FirstName = viewModel.FirstName;
            existingRegistration.User_LastName = viewModel.LastName;
            existingRegistration.Contact_Email = viewModel.Email;
            existingRegistration.User_Email = viewModel.Email;
            existingRegistration.Customer_Email = viewModel.Email;
            existingRegistration.Contact_Phone = existingRegistration.User_Phone = viewModel.Telephone;
            existingRegistration.Contact_Mobile = existingRegistration.User_Mobile = viewModel.Mobilephone;

            var newUser = await _accountRepo.CreateUserAsync(viewModel.FirstName, viewModel.LastName, viewModel.Telephone, viewModel.Mobilephone,
                viewModel.Email, string.Empty, string.Empty, string.Empty, viewModel.SerialNumber, string.Empty, false);

            ViewData["email"] = newUser.Email;
            _logger.Info($"Created user:{newUser.Email}:{newUser.UserId}:{newUser.UserName}");
            AcceptUserAgreement(newUser, currentPage.UserAgreementPageReference);

            //send the user activation email
            await SendActivationEmail(newUser);

            //send registration files to the backend
            await _orgRepo.SaveCustomerRegistrationAsync(existingRegistration);

            ViewData["reference"] = nameof(NonPrivateFirm);
            return View("~/Views/Registration/Activation/Finish.cshtml", new PageViewModel<AccountActivationPage>(currentPage));
        }

        private void AcceptUserAgreement(UserCore user, UserAgreementsPage userAgreementPage)
        {
            _userManager.UpdateInsertUserAccepts(userAgreementPage.TermId, userAgreementPage.Version, user.UserId);
        }

        /// <summary>
        /// Create a password, associate it with user and send an activation email to that user
        /// </summary>
        private async Task SendActivationEmail(UserCore newUser)
        {
            var password = RandomPassword.Generate();
            await _securityRepo.ChangePasswordAsync(newUser.UserName, password);
            ViewData["password"] = password;
            ViewData["link"] = ConfigurationManager.AppSettings["domainUrl"];

            var emailBody = this.RenderPartialViewToString("~/Views/Registration/Activation/ConfirmationEmailTemplate.cshtml",
                ViewData);

            _logger.Info($"Sending activation email to {newUser.Email}");
            await _emailService.SendMailAsync(Email.LantmannenFromAddress, new[] { newUser.Email },
                    new string[0], "Välkommen till LM" + "\xB2", emailBody, _ticketProvider.GetTicket())
                .ConfigureAwait(false);
        }

        private ActionResult VerificationError(AccountActivationPage currentPage)
            => View("~/Views/Registration/Activation/Verification.cshtml", new PageViewModel<AccountActivationPage>(currentPage));

        private static bool IsBankIdMatchingRegistration(string serialNumber, string orgNumber, out string personNumber)
        {
            if (serialNumber.Length < 2)
            {
                personNumber = null;
                return false;
            }

            var existingOrgNr = orgNumber?.Replace("-", "") ?? string.Empty;
            var serialNumberMatch = serialNumber?.Substring(2);
            personNumber = string.IsNullOrWhiteSpace(serialNumberMatch) ? existingOrgNr : serialNumber;
            // To test without bankid add existingOrgNr or add Serialnumber header in request.
            return serialNumberMatch == existingOrgNr;
        }
    }
}
