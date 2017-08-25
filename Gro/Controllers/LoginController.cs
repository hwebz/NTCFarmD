using EPiServer;
using EPiServer.Security;
using EPiServer.Core;
using Gro.Business.DataProtection;
using Gro.Business.Services.Users;
using Gro.Constants;
using Gro.Core.ContentTypes.Pages.StartPages;
using Gro.Core.Interfaces;
using Gro.Helpers;
using Gro.Infrastructure.Data;
using Gro.Infrastructure.Data.EmailService;
using Gro.ViewModels;
using Gro.ViewModels.Users;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Configuration;
using System.ServiceModel;
using static System.Web.Security.FormsAuthentication;

namespace Gro.Controllers
{
    public class LoginController : Controller
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IContentRepository _contentRepo;
        private readonly IUserManagementService _userManager;
        private readonly IEmailService _emailService;
        private readonly ISecurityRepository _securityRepo;
        private readonly string _ticket;

        public LoginController(
            ITokenGenerator tokenGenerator,
            IContentRepository contentRepo,
            IUserManagementService userManager,
            IEmailService emailService,
            TicketProvider ticketProvider,
            ISecurityRepository securityRepo)
        {
            _tokenGenerator = tokenGenerator;
            _contentRepo = contentRepo;
            _userManager = userManager;
            _emailService = emailService;
            _securityRepo = securityRepo;
            _ticket = ticketProvider.GetTicket();
        }

        private static PageViewModel<StartPage> _startPageViewModel;
        private PageViewModel<StartPage> StartPageViewModel => _startPageViewModel ?? (_startPageViewModel = GetStartPageViewModel());

        private PageViewModel<StartPage> GetStartPageViewModel()
        {
            var startPage = _contentRepo.Get<StartPage>(ContentReference.StartPage);
            var clone = (StartPage)startPage.CreateWritableClone();
            return new PageViewModel<StartPage>(clone);
        }

        private ActionResult PageView(string title)
        {
            var model = StartPageViewModel;
            model.CurrentPage.Name = title;
            return View(model);
        }

        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> Index(LoginViewModel viewModel)
        {
            var siteUser = await _userManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password);
            if (siteUser == null)
            {
                ViewData["error"] = "Invalid login";
                return PageView("Login");
            }

            var activeCustomer = _userManager.GetActiveCustomer(siteUser);
            siteUser.ActiveCustomerNumber = activeCustomer.CustomerNo;
            this.SetUserSession(siteUser);

            //Logging in a specific user:
            AddCurrentUserToEpiLogin(viewModel.UserName);
            return Redirect(string.IsNullOrWhiteSpace(viewModel.RedirectUrl) ? "/" : viewModel.RedirectUrl);
        }

        private static void AddCurrentUserToEpiLogin(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return;

            PrincipalInfo.CurrentPrincipal = PrincipalInfo.CreatePrincipal(userName);
            SetAuthCookie(userName, false);
        }

        //GET p/lostpassword/
        [Route("p/LostPassword")]
        public ActionResult LostPassword() => PageView("Reset password");

        //POST p/SubmitForgotPasswordEmail
        [Route("p/SubmitForgotPasswordEmail")]
        [HttpPost]
        public async Task<ActionResult> SubmitForgotPasswordEmail(LostPasswordSubmissionViewModel viewModel)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(400, "InvalidEmail");

            ViewData["email"] = viewModel.Email;

            var userName = _securityRepo.GetUserNameByEmail(viewModel.Email);
            if (string.IsNullOrWhiteSpace(userName)) return PageView("Email sent");

            var guid = await _securityRepo.GeneratePasswordGuidAsync(userName);
            var user = await _userManager.QuerySiteUserAsync(userName);
            var resetPasswordToken = _tokenGenerator.Encrypt(new ResetPasswordConfirmationData
            {
                GuidString = guid,
                UserName = userName
            });

            ViewData["link"] = $"{ConfigurationManager.AppSettings["publicSitePrefix"]}/p/resetpassword?payload={resetPasswordToken}";
            ViewData["payload"] = resetPasswordToken;
            ViewData["userName"] = user.Name;

            var emailBody = this.RenderPartialViewToString("~/Views/MyProfile/ResetPasswordEmailTemplate.cshtml", ViewData);
            await _emailService.SendMailAsync(Email.LantmannenFromAddress, new[] { viewModel.Email },
                    new string[0], "Återställ lösenord i LM" + "\xB2", emailBody, _ticket)
                .ConfigureAwait(false);

            return PageView("Email sent");
        }

        //GET p/resetpassword
        [Route("p/ResetPassword")]
        [HttpGet]
        public ActionResult ResetPassword(string payload)
        {
            payload = payload?.Replace(' ', '+');
            ViewData["payload"] = payload;
            return PageView("Reset password");
        }

        //POST p/resetpassword
        [Route("p/ResetPassword")]
        [HttpPost]
        public async Task<ActionResult> ResetPassword(string password, string confirmPassword, string payload)
        {
            if (password != confirmPassword)
            {
                ViewData["error"] = "Detta lösenord stämmer inte överens med det första du angav.";
            }

            if (!_userManager.IsUserPasswordValid(password))
            {
                ViewData["error"] = "Lösenordet är inte av rätt typ. Se krav för nytt lösenord.";
            }

            if (ViewData["error"] != null)
            {
                ViewData["payload"] = payload;
                return PageView("Reset password");
            }

            payload = payload?.Replace(' ', '+');
            try
            {
                var data = _tokenGenerator.Decrypt<ResetPasswordConfirmationData>(payload);
                //check if the guid is still valid
                var checkGuidResult = await _securityRepo.CheckPasswordGuidAsync(data.UserName, data.GuidString);
                if (!checkGuidResult)
                {
                    return new HttpStatusCodeResult(400);
                }

                //activate user
                await _userManager.ActivateAccount(data.UserName);

                //reset now
                await _securityRepo.ChangePasswordAsync(data.UserName, password);
            }
            //pretend these security error didn't happen
            catch (CryptographicException) { }
            catch (ArgumentException) { }
            catch (FaultException ex) when (ex.Message.Contains("The new password cannot be the same"))
            {
                ViewData["error"] = "Lösenordet är inte av rätt typ. Se krav för nytt lösenord.";
                return PageView("Reset password");
            }

            // return success anyway
            return View("~/Views/Login/ResetPasswordSuccessful.cshtml", StartPageViewModel);
        }

        [HttpPost]
        [Route("p/Logout")]
        public ActionResult Logout(string logoutRedirect)
        {
            this.SignoutSiteUser();
            SignOut();
            return Redirect(logoutRedirect);
        }
    }
}
