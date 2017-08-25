using System.Linq;
using System.Threading.Tasks;
using Gro.Core.ContentTypes.Pages.Organization;
using Gro.ViewModels;
using System.Web.Mvc;
using Gro.Business.Services.Users;
using Gro.Core.Interfaces;
using Gro.ViewModels.Organization;
using Gro.Core.DataModels.Security;
using EPiServer;
using Gro.Core.ContentTypes.Pages.StartPages;
using Gro.Core.ContentTypes.Pages;
using EPiServer.Core;
using Gro.Infrastructure.Data.EmailService;
using Gro.Business.DataProtection;
using System;
using Gro.Infrastructure.Data;
using Gro.Helpers;
using System.Collections.Generic;
using System.Configuration;
using Gro.Constants;

namespace Gro.Controllers.Pages.Organization
{
    public class AddUserToOrganizationController : SiteControllerBase<AddUserToOrganizationPage>
    {
        private readonly ISecurityRepository _securityRepository;
        private readonly IOrganizationUserRepository _orgUserRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly IContentRepository _contentRepo;
        private readonly IEmailService _emailService;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly string _ticket;

        public AddUserToOrganizationController(
            ISecurityRepository securityRepository,
            IOrganizationUserRepository orgUserRepo,
            IUserManagementService userManager,
            IAccountRepository accountRepo,
            IContentRepository contentRepo,
            IEmailService emailService,
            ITokenGenerator tokenGenerator,
            TicketProvider ticketProvider) : base(userManager)
        {
            _securityRepository = securityRepository;
            _orgUserRepo = orgUserRepo;
            _accountRepo = accountRepo;
            _contentRepo = contentRepo;
            _emailService = emailService;
            _tokenGenerator = tokenGenerator;
            _ticket = ticketProvider.GetTicket();
        }

        [HttpGet]
        public async Task<ActionResult> Index(AddUserToOrganizationPage currentPage)
        {
            // store roles and profiles in ViewData
            await Task.WhenAll(this.GetAllRolesTask(_securityRepository), this.GetRolesAndProfiles(_securityRepository));
            var viewModel = new PageViewModel<AddUserToOrganizationPage>(currentPage);
#if DEBUG
            ViewData["serialNumber"] = "1321312";
#else
            ViewData["serialNumber"] = SiteUser?.SerialNumber ?? string.Empty;
#endif
            return View("~/Views/Organization/AddUserToOrganization.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Index(AddUserToOrganizationFormViewModel viewModel)
        {
#if DEBUG
            SiteUser.SerialNumber = "1321312";
#endif
            //get active customer, which is the admin
            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);

            var existingUser = await _securityRepository.FindUserByEmailAsync(viewModel.Email);
            if (existingUser != null)
            {
                var organizationsOfUser = await _orgUserRepo.GetOrganizationsOfUserAsync(existingUser.UserName);
                if (organizationsOfUser.Any(o => o.CustomerId == activeCustomer?.CustomerId)) return new HttpStatusCodeResult(400);

                return await AddExistingUserToOrganizationAsync(existingUser.UserName, activeCustomer, viewModel);
            }

            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(SiteUser.SerialNumber)) return new HttpStatusCodeResult(400);
            return await AddNewUserToOrganizationAsync(activeCustomer, viewModel);
        }

        [Route("api/add-user-to-org/find-existing")]
        [HttpGet]
        public async Task<JsonResult> GetUserInfoAsync(string email)
        {
            var dbUser = await _securityRepository.FindUserByEmailAsync(email);
            if (dbUser == null) return Json(new { }, JsonRequestBehavior.AllowGet);

            var organizationsOfUser = await _orgUserRepo.GetOrganizationsOfUserAsync(dbUser.UserName);
            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);

            if (organizationsOfUser.Any(o => o.CustomerId == activeCustomer?.CustomerId))
            {
                //user already in customer
                //confict
                Response.StatusCode = 409;
                return Json(new {error = "UserExists"}, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                userId = dbUser.DbUserId,
                userName = dbUser.UserName,
                firstName = dbUser.FirstName,
                lastName = dbUser.LastName,
                telephone = dbUser.PhoneWork,
                mobile = dbUser.PhoneMobile,
                email = dbUser.Email
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<RoleProfileViewModel> FindMatchedProfileAsync(string roleIds)
        {
            var profiles = await this.GetRolesAndProfiles(_securityRepository);
            var matchedProfile = profiles.FirstOrDefault(p => string.Join(",", p.ProfileRoles.Select(r => r.RoleId)) == roleIds);
            return matchedProfile;
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

        private async Task<ActionResult> AddNewUserToOrganizationAsync(CustomerBasicInfo organization, AddUserToOrganizationFormViewModel viewModel)
        {
            var user = await _accountRepo.CreateUserAsync(viewModel.FirstName, viewModel.LastName, viewModel.Telephone,
                viewModel.Mobile, viewModel.Email, string.Empty, string.Empty, string.Empty);

            if (user == null) throw new ApplicationException("CreateNewFailed");

            var roles = viewModel.Roles.Split(',');
            await _orgUserRepo.AddUserToOrganizationAsync(user, organization, roles);

            var startPage = _contentRepo.Get<StartPage>(ContentReference.StartPage);
            var settingPage = _contentRepo.Get<SettingsPage>(startPage.SettingsPage);

            var guid = await _securityRepository.GeneratePasswordGuidAsync(user.UserName);

            var resetPasswordToken = _tokenGenerator.Encrypt(new ResetPasswordConfirmationData
            {
                GuidString = guid,
                UserName = viewModel.Email
            });

            //fire and forget email
            //var host = Request.Url?.Host + (Request?.Url?.IsDefaultPort == true ? "" : ":" + Request?.Url?.Port);
            var link = $"{ConfigurationManager.AppSettings["publicSitePrefix"]}/p/resetpassword?payload={resetPasswordToken}";
            await SendEmailToNewUserAsync(viewModel.Email, viewModel.Roles, link, organization.CustomerName);

            TempData["reference"] = "NewUserAdded";
            return RedirectToAction("Index", new {node = settingPage.HandleOrganizationUserPage});
        }

        private async Task<ActionResult> AddExistingUserToOrganizationAsync(string userName, CustomerBasicInfo organization,
            AddUserToOrganizationFormViewModel viewModel)
        {
            var user = await UserManager.QuerySiteUserAsync(userName);
            if (user == null) return new HttpStatusCodeResult(400);

            var roles = viewModel.Roles.Split(',');
            await _orgUserRepo.AddUserToOrganizationAsync(user, organization, roles);

            //fire and forget email
            var link = $"{ConfigurationManager.AppSettings["domainUrl"]}";
            await SendEmailToExistingUserAsync(viewModel.Email, viewModel.Roles, link, organization.CustomerName);

            var startPage = _contentRepo.Get<StartPage>(ContentReference.StartPage);
            var settingPage = _contentRepo.Get<SettingsPage>(startPage.SettingsPage);
            return RedirectToAction("Index", new {node = settingPage.HandleOrganizationUserPage});
        }
    }
}