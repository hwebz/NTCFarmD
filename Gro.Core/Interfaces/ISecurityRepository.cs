using Gro.Core.DataModels.Security;
using System.Threading.Tasks;
using System.Web.Security;

namespace Gro.Core.Interfaces
{
    public interface ISecurityRepository
    {
        /// <summary>
        /// Generate a password guid for user
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <returns>Generated Guid</returns>
        Task<string> GeneratePasswordGuidAsync(string userName);

        /// <summary>
        /// Check if password guid is valid and invalidate it if true
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <param name="guid">Guid</param>
        Task<bool> CheckPasswordGuidAsync(string userName, string guid);

        /// <summary>
        /// Invalidate a password guid
        /// </summary>
        /// <param name="userName">User's username</param>
        Task InvalidatePasswordGuid(string userName);

        MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords);
        Task<ResponseUser> FindUserByEmailAsync(string email);
        MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords);
        string[] FindUsersInRole(string roleName, string usernameToMatch);
        Role[] GetAllRoles();
        Task<Role[]> GetAllRolesAsync();
        UserRole[] GetRolesForUser(string username);
        MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords);
        string GetPassword(string username, string answer);
        string GetUserNameByEmail(string email);
        string[] GetUsersInRole(string roleName);
        bool IsUserInRole(string username, string roleName);
        void RemoveUsersFromRoles(string[] usernames, string[] roleNames);
        bool RoleExists(string roleName);
        bool ValidateUser(string username, string password);
        Task<ResponseUser> QueryUserAsync(string userName);
        ResponseUser QueryUser(string userName);
        Task<bool> SaveUserPictureUrl(int userId, string pictureUrl);
        Task<bool> DeleteUserPictureUrl(int userId);
        Task<bool> UpdateSocialSecurityNumberAsync(string username, string socialNumber);

        Task ChangePasswordAsync(string userName, string newPassword);

        string GetSocialSecurityNumber(string username);
        Task<string> GetPersonObjectIdByNameAsync(string userName);
        string GetPersonObjectIdByName(string userName);
        Task<UserProfile[]> GetCustomerUsersByProfileAsync(int customerId);
        Task<Profile[]> GetRoleProfilesAsync();
        Task<ProfileRole[]> GetRolesOfProfileAsync(string profileId);
        Task<User[]> GetUsersForCustomerAsync(int customerId);
        Task<CustomerCheckCode> MatchCustomerNumberAndOrganizationNumberAsync(string customerNo, string orgNo);
        Company GetExistingCompany(string customerNo, string orgNo);
    }
}
