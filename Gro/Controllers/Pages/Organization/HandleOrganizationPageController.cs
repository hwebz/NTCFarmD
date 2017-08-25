using Gro.Core.ContentTypes.Pages.Organization;
using Gro.Core.Interfaces;
using Gro.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gro.Helpers;
using EPiServer;
using Gro.Infrastructure.Data.EmailService;
using Gro.ViewModels.Organization;
using Gro.Constants;
using System.Collections.Generic;
using Gro.Infrastructure.Data;
using Gro.Core.DataModels.Security;
using Gro.Business.Rendering;

namespace Gro.Controllers.Pages.Organization
{
    public class HandleOrganizationPageController : SiteControllerBase<HandleOrganizationUserPage>
    {
        private readonly ISecurityRepository _securityRepository;
        private readonly IOrganizationUserRepository _orgUserRepo;
        private readonly IEmailService _emailService;
        private readonly IContentRepository _contentRepo;
        private readonly TicketProvider _ticketProvider;

        public HandleOrganizationPageController(ISecurityRepository securityRepository, IContentRepository contentRepo,
            IEmailService emailService, IOrganizationUserRepository orgUserRepo, TicketProvider ticketProvider)
        {
            _securityRepository = securityRepository;
            _orgUserRepo = orgUserRepo;
            _emailService = emailService;
            _ticketProvider = ticketProvider;
            _contentRepo = contentRepo;
        }

        private ActionResult PageView(HandleOrganizationUserPage currentPage)
        {
#if DEBUG
            ViewData["serialNumber"] = "1321312";
#else
            ViewData["serialNumber"] = SiteUser.SerialNumber;
#endif
            var viewModel = new PageViewModel<HandleOrganizationUserPage>(currentPage);
            var customer = UserManager.GetActiveCustomer(HttpContext);
            ViewData["currentOrganization"] = customer?.CustomerName ?? string.Empty;
            ViewData["contentId"] = currentPage.ContentGuid.ToString();
            return View("~/Views/Organization/HandleOrganizationUserPage.cshtml", viewModel);
        }

        private async Task<RoleProfileViewModel> FindMatchedProfileAsync(string roleIds)
        {
            var profiles = await this.GetRolesAndProfiles(_securityRepository);
            var matchedProfile = profiles.FirstOrDefault(p => string.Join(",", p.ProfileRoles.Select(r => r.RoleId)) == roleIds);
            return matchedProfile;
        }

        private async Task SendChangedRoleEmailAsync(string email, string roleIds, CustomerBasicInfo customerName, string userName)
        {
            var matchedProfile = await FindMatchedProfileAsync(roleIds);

            var profileTitle = matchedProfile?.Description ?? "Anpassad profil";
            var emailBody = this.RenderPartialViewToString("~/Views/Organization/Email_UserRoleChanged.cshtml", new Dictionary<string, string>
            {
                {"profileTitle", profileTitle},
                {"customerName", customerName.CustomerName},
                {"organizationNumber", customerName.CustomerNo},
                {"userName", userName}
            });

            await _emailService.SendMailAsync(Email.LantmannenFromAddress, new[] { email },
                new string[0], $"Mail till användare för {customerName}: t och i LM\xB2", emailBody, _ticketProvider.GetTicket());
        }

        private async Task<ActionResult> UpdateUserRolesAsync(HandleOrganizationUserPage currentPage, string userName,
            string roles)
        {
            var existingUser = await UserManager.QuerySiteUserAsync(userName);
            if (existingUser == null) return new HttpStatusCodeResult(400);

            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);
            var userCustomers = await _orgUserRepo.GetOrganizationsOfUserAsync(existingUser.UserName);

            if (userCustomers?.Any(c => c.CustomerNo == activeCustomer.CustomerNo) != true)
            {
                return new HttpStatusCodeResult(400);
            }

            await _orgUserRepo.UpdateUserCustomerRolesAsync(existingUser.UserName, activeCustomer, roles.Split(','));
            TempData["reference"] = "UserRolesChanged";

            await SendChangedRoleEmailAsync(existingUser.Email, roles, activeCustomer, existingUser.Name);

            // store roles and profiles in ViewData
            await Task.WhenAll(this.GetAllRolesTask(_securityRepository), this.GetRolesAndProfiles(_securityRepository));

            return PageView(currentPage);
        }

        //remove user from org
        private async Task<ActionResult> RemoveUserFromOrganizationAsync(HandleOrganizationUserPage currentPage,
            string userName)
        {
            var existingUser = await UserManager.QuerySiteUserAsync(userName);
            if (existingUser == null) return new HttpStatusCodeResult(400);

            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);
            var userCustomers = await _orgUserRepo.GetOrganizationsOfUserAsync(existingUser.UserName);

            if (userCustomers?.Any(c => c.CustomerNo == activeCustomer.CustomerNo) != true) return new HttpStatusCodeResult(400);

            await _orgUserRepo.RemoveUserFromOrganizationsAsync(existingUser, activeCustomer);

            TempData["reference"] = "UserRemoved";
            // store roles and profiles in ViewData
            await Task.WhenAll(this.GetAllRolesTask(_securityRepository), this.GetRolesAndProfiles(_securityRepository));
            return PageView(currentPage);
        }

        [HttpGet]
        public async Task<ActionResult> Index(HandleOrganizationUserPage currentPage)
        {
            // store roles and profiles in ViewData
            await Task.WhenAll(this.GetAllRolesTask(_securityRepository), this.GetRolesAndProfiles(_securityRepository));
            return PageView(currentPage);
        }

        //post
        [HttpPost]
        public async Task<ActionResult> Index(HandleOrganizationUserPage currentPage, string action, string userName,
            string roles)
        {
            if (action == "updateRoles") return await UpdateUserRolesAsync(currentPage, userName, roles);

            return await RemoveUserFromOrganizationAsync(currentPage, userName);
        }

        [Route("api/organization/get-users")]
        [HttpGet]
        public async Task<ActionResult> GetUsersOfThisOrganizationAsync(string pageId)
        {
            if (string.IsNullOrWhiteSpace(pageId)) return new HttpStatusCodeResult(400);
            var page = _contentRepo.Get<HandleOrganizationUserPage>(new System.Guid(pageId));
            if (page == null) return new HttpStatusCodeResult(400);

            if (!PageAccess.CanAccessPage(UserManager, _orgUserRepo, page, HttpContext))
            {
                return new HttpStatusCodeResult(401);
            }

            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);
            var orgUsersByProfiles = await _orgUserRepo.GetOrganizationUsersByProfileAsync(activeCustomer);

            var userAndRoles = orgUsersByProfiles.Select(user => new
            {
                email = user.Email,
                name = user.Name,
                mobile = user.Mobile,
                phone = user.Phone,
                profilePicUrl = string.IsNullOrWhiteSpace(user?.ProfilePicUrl) ? "/Static/images/user-avatar__default.jpg" : user.ProfilePicUrl,
                userName = user.UserName,
                roles = user.Roles,
                lockedOut = user.LockedOut,
                roleProfileId = user?.RoleProfileId ?? string.Empty,
                roleProfileName = user?.RoleProfileName ?? string.Empty
            });

            return Json(userAndRoles, JsonRequestBehavior.AllowGet);
        }
    }
}
