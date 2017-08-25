using EPiServer.Core;
using Gro.Core.DataModels.Security;
using System.Threading.Tasks;
using System.Web;

namespace Gro.Business.Services.Users
{
    public interface IUserManagementService
    {
        CustomerBasicInfo GetActiveCustomer(HttpContextBase httpContext, bool refresh = false);

        CustomerBasicInfo GetActiveCustomer(SiteUser user, bool refresh = false);

        CustomerBasicInfo CustomerExistsForUser(HttpContextBase httpContext, string customerNumber);

        SiteUser GetSiteUser(HttpContextBase httpContext);

        bool UpdateInsertUserAccepts(string term, int version, int userId);

        bool UpdateTermsOfUseVersion(int newVersion, string userAgreementIdentity);

        Task<SiteUser> PasswordSignInAsync(string userName, string password);

        Task ActivateAccount(string userName);

        Task<bool> UpdateUserInfoAsync(string personNumber, string firstName, string lastName, string telephone, string mobilephone,
            string email, string street, string zip, string city);

        Task<SiteUser> QuerySiteUserAsync(string userName);

        bool IsUserPasswordValid(string password);

        bool ContainRolesHavingAccessToPage(IContent page, UserRole[] userRoles, bool isOwner = false);

        bool CheckTerm(string termId, int version);

        string GeneratePassword();
        void UpdateInternalCustomerNumber (HttpContextBase httpContext, CustomerBasicInfo customer);
        CustomerBasicInfo GetInternalCustomerNumber(HttpContextBase httpContext);
        bool EndInternalCustomerSession(HttpContextBase httpContext);
    }
}
