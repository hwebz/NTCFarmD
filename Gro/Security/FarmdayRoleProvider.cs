using System.Web.Security;
using Gro.Core.Interfaces;
using System.Web.Mvc;
using System;
using System.Linq;

namespace Gro.Security
{
    public class FarmdayRoleProvider : RoleProvider
    {
        private readonly ISecurityRepository _securityRepository;

        public FarmdayRoleProvider()
        {
            _securityRepository = DependencyResolver.Current.GetService<ISecurityRepository>();
        }

        public override bool IsUserInRole(string username, string roleName)
            => _securityRepository.IsUserInRole(username, roleName);

        public override string[] GetRolesForUser(string username)
            => _securityRepository.GetRolesForUser(username).Select(r => r.RoleName).ToArray();

        public override void CreateRole(string roleName)
        {
            throw new NotSupportedException("Creating roles is not supported in admin mode");
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotSupportedException("Deleting roles is not supported in admin mode");
        }

        public override bool RoleExists(string roleName) => _securityRepository.RoleExists(roleName);

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException("Managing roles is not available in admin mode");
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException("Removing roles is not available in admin mode");
        }

        public override string[] GetUsersInRole(string roleName) => _securityRepository.GetUsersInRole(roleName);

        public override string[] GetAllRoles() => _securityRepository.GetAllRoles().Select(r => r.RoleName).ToArray();

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
            => _securityRepository.FindUsersInRole(roleName, usernameToMatch);

        public override string ApplicationName { get; set; }
    }
}
