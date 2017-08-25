using Gro.Core.DataModels.Security;
using System.Threading.Tasks;

namespace Gro.Core.Interfaces
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Sign in with username and password
        /// </summary>
        /// <returns>User object including session</returns>
        Task<UserCore> PasswordSigninAsync(string userName, string password);

        /// <summary>
        /// Update user information
        /// </summary>
        /// <returns>Boolean indicating results</returns>
        Task<bool> UpdateUserInfoAsync(string personDn, string firstName, string lastName, string telephone, string mobilephone,
            string email, string street, string zip, string city);

        /// <summary>
        /// Query the user information based on userName
        /// </summary>
        Task<UserCore> QueryUserCoreAsync(string userName);

        Task<UserCore> CreateUserAsync(string firstName, string lastName, string telephone, string mobilephone,
            string email, string street, string zip, string city, string personNumber = "", string customerNumber = "", bool suspend = true);

        Task ActivateUserAsync(string userName);

        Task InactivateUserAsync(string userName);
    }
}
