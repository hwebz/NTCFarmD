using Gro.Core.DataModels.Security;
using System.Threading.Tasks;

namespace Gro.Core.Interfaces
{
    public interface IOrganizationUserRepository
    {
        /// <summary>
        /// Get the list of organizations that the user is in
        /// </summary>
        /// <param name="userName">CGI UserName</param>
        /// <param name="refreshCache">Refresh the cache?</param>
        CustomerBasicInfo[] GetOrganizationsOfUser(string userName, bool refreshCache = false);

        Task<User[]> GetUsersOfOrganizationAsync(CustomerBasicInfo organization);

        Task UpdateUserCustomerRolesAsync(string userName, CustomerBasicInfo customer, string[] roleIds);

        Task AddUserToOrganizationAsync(UserCore user, CustomerBasicInfo customer, string[] roleIds = null);

        /// <summary>
        /// Get the list of organizations that the user is in
        /// </summary>
        /// <param name="userName">CGI UserName</param>
        Task<CustomerBasicInfo[]> GetOrganizationsOfUserAsync(string userName);

        Task RemoveUserFromOrganizationsAsync(UserCore user, CustomerBasicInfo customer);

        Task<UserRole[]> GetUserCustomerRolesAsync(string userName, string organizationNumber);

        UserRole[] GetUserCustomerRoles(string userName, string organizationNumber);

        Task<OrganizationUser[]> GetOrganizationUsersByProfileAsync(CustomerBasicInfo customer);

        Task<User> GetOwnerAsync(CustomerBasicInfo customer);
        Task<User> GetOwnerAsync(int customerId);
    }
}
