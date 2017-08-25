using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.MyProfile;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.MyProfile;
using System.Threading.Tasks;
using Gro.Core.DataModels.Security;

namespace Gro.Controllers.Pages.MyProfile
{
    public class MyCustomersController : SiteControllerBase<MyCustomersPage>
    {
        private readonly ISecurityRepository _securityRepository;
        private readonly IOrganizationUserRepository _orgUserRepo;

        public MyCustomersController(
            ISecurityRepository securityRepository,
            IUserManagementService usersManagementService,
            IOrganizationUserRepository orgUserRepo) : base(usersManagementService)
        {
            _securityRepository = securityRepository;
            _orgUserRepo = orgUserRepo;
        }

        /// <summary>
        /// Match user's set of roles to a profile
        /// </summary>
        /// <returns>Profile description</returns>
        private async Task<string> MatchUserProfileAsync(IEnumerable<UserRole> userRoles, IEnumerable<Profile> profiles)
        {
            var userRolesString = string.Join(",", userRoles.Select(ur => ur.Roleid).OrderBy(id => id));

            foreach (var profile in profiles)
            {
                var rolesOfProfile = await _securityRepository.GetRolesOfProfileAsync(profile.Id);
                var profileRolesString = string.Join(",", rolesOfProfile.Select(r => r.RoleId).OrderBy(rid => rid));
                if (userRolesString == profileRolesString)
                {
                    return profile.Description;
                }
            }

            return "Custom";
        }

        [HttpGet]
        public async Task<ActionResult> Index(MyCustomersPage currentPage)
        {
            if (SiteUser == null)
            {
                return View("~/Views/MyProfile/MyCustomers.cshtml", new MyCustomersViewModel(currentPage));
            }

            var organization = UserManager.GetActiveCustomer(HttpContext);

            var allRoles = _securityRepository.GetAllRoles().Where(r => !r.RoleName.EndsWith("_w"));
            var userRoles = await _orgUserRepo.GetUserCustomerRolesAsync(SiteUser.UserName, organization.CustomerNo);
            var customerRoles = userRoles.Where(ur => ur.Sysrole == false);

            var userRightsInCustomer = (from role in allRoles
                                        where role.RoleName != "CustomerOwner"
                                        let customerRole = customerRoles.FirstOrDefault(r => r.RoleName.StartsWith(role.RoleName))
                                        select new UserRoleInfoViewModel
                                        {
                                            RoleName = role.RoleName,
                                            RoleDescription = role.RoleDescription,
                                            RoleId = role.RoleId,
                                            HasRole = customerRole != null,
                                            HasFullControl = customerRole?.RoleName?.EndsWith("_w") == true
                                        });

            var profiles = await _securityRepository.GetRoleProfilesAsync();
            var roleProfileDescription = await MatchUserProfileAsync(userRoles, profiles);
            ViewData["RoleProfile"] = (roleProfileDescription == "Custom" ? "Anpassad profil" : roleProfileDescription);

            var model = new MyCustomersViewModel(currentPage)
            {
                CurrentOrganization = organization,
                UserRightsInCustomer = userRightsInCustomer
            };
            return View("~/Views/MyProfile/MyCustomers.cshtml", model);
        }
    }
}
