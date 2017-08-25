using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Security;
using Gro.Business.Services.Users;
using Gro.Core.Interfaces;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Gro.Business.Rendering
{
    public static class PageAccess
    {
        private static bool AdminHasAccess(HttpContextBase httpContext,
            IEnumerable<AccessControlEntry> accessControlEntries)
        {
            var userName = httpContext?.User?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userName)) return false;

            var roles = Roles.Providers["SqlServerRoleProvider"]?.GetRolesForUser(userName);
            return roles?.Any(r => accessControlEntries.Any(e => e.Name == r)) == true;
        }

        private static bool UserHasAccess(HttpContextBase httpContext, IUserManagementService userManager,
            IOrganizationUserRepository orgUserRepo,
            IEnumerable<AccessControlEntry> accessControlEntries)
        {
            var siteUser = userManager.GetSiteUser(httpContext);
            //if no user, false
            if (siteUser?.UserName == null) return false;

            var activeCustomer = userManager.GetActiveCustomer(httpContext);
            if (activeCustomer?.CustomerNo == null)
            {
                var defaultCustomerRoles = orgUserRepo.GetUserCustomerRoles(siteUser.UserName, "LBR") ??
                                new Core.DataModels.Security.UserRole[0];

                return defaultCustomerRoles.Any(ur => accessControlEntries.Any(e => e.Name == ur.RoleName));
            }

            var userRoles = orgUserRepo.GetUserCustomerRoles(siteUser.UserName, activeCustomer.CustomerNo) ??
                            new Core.DataModels.Security.UserRole[0];

            return userRoles.Any(ur => accessControlEntries.Any(e => e.Name == ur.RoleName));
        }

        public static bool CanAccessPage(IUserManagementService userManager, IOrganizationUserRepository orgUserRepo,
            PageData page, HttpContextBase httpContext, AccessLevel accessLevel = AccessLevel.Read)
        {
            var allowedRoles = page.ACL.Entries.Where(e => (e.Access & accessLevel) != AccessLevel.NoAccess).ToArray();
            if (allowedRoles.Any(e => e.Name == "Everyone")) return true;

            var adminHasAccess = AdminHasAccess(httpContext, allowedRoles);
            var userHasAccess = UserHasAccess(httpContext, userManager, orgUserRepo, allowedRoles);

            return adminHasAccess || userHasAccess;
        }
    }
}
