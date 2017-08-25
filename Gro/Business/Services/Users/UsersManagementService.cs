using System;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Security;
using Gro.Core.DataModels.Security;
using Gro.Core.Interfaces;
using Gro.Core.ContentTypes.Utils;
using System.Threading.Tasks;
using Gro.Helpers;
using System.Text.RegularExpressions;
using Gro.Business.DataProtection;
using Gro.Constants;

namespace Gro.Business.Services.Users
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IContentSecurityRepository _contentSecurityRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserTermsOfUseRepository _userTermsOfUseRepository;
        private readonly IOrganizationUserRepository _orgUserRepo;
        private readonly ITokenGenerator _tokenGenerator;

        private static readonly Regex PasswordRegex = new Regex(@"^(?=.*\d{2,})(?=.*[a-z])(?=.*[A-Z]).{8,}$");

        public UserManagementService(IAccountRepository accountRepository,
            IContentSecurityRepository contentSecurityRepository,
            IUserTermsOfUseRepository userTermsOfUseRepository,
            IOrganizationUserRepository orgUserRepo,
            ITokenGenerator tokenGenerator)
        {
            _accountRepository = accountRepository;
            _contentSecurityRepository = contentSecurityRepository;
            _userTermsOfUseRepository = userTermsOfUseRepository;
            _orgUserRepo = orgUserRepo;
            _tokenGenerator = tokenGenerator;
        }

        //public CustomerBasicInfo GetActiveCustomer(string userName)
        public CustomerBasicInfo GetActiveCustomer(HttpContextBase httpContext, bool refresh) => GetActiveCustomer(GetSiteUser(httpContext), refresh);

        public CustomerBasicInfo GetActiveCustomer(SiteUser user, bool refresh)
        {
            if (user == null) return null;

            //check and return right away

            var customerList = _orgUserRepo.GetOrganizationsOfUser(user.UserName, refresh);
            if (customerList == null || customerList.Length == 0)
            {
                return new CustomerBasicInfo();
            }

            var activeCustomer = customerList.FirstOrDefault(c => c.CustomerNo == user.ActiveCustomerNumber) ?? customerList[0];

            return activeCustomer;
        }

        public CustomerBasicInfo CustomerExistsForUser(HttpContextBase httpContext, string customerNumber)
        {
            var siteUser = GetSiteUser(httpContext);
            var customerList = _orgUserRepo.GetOrganizationsOfUser(siteUser.UserName);
            var customer = customerList.FirstOrDefault(c => c.CustomerNo == customerNumber);
            return customer;
        }

        public bool ContainRolesHavingAccessToPage(IContent page, UserRole[] userRoles, bool isOwner = false)
        {
            var pageSecurity = _contentSecurityRepository.Get(page.ContentLink).CreateWritableClone() as IContentSecurityDescriptor;
            if (pageSecurity == null) return false;

            var roleEntities = pageSecurity.Entries;
            //TODO: will update later when GetRolesForUser return list containing "Owner" role(if the user is owner of customerID)
            return roleEntities.Where(item => item.Access != AccessLevel.NoAccess && item.Access != AccessLevel.Undefined)
                .Any(item => item.Name.Equals("everyone", StringComparison.OrdinalIgnoreCase)
                    || userRoles.Any(x => x.RoleName == item.Name
                    || (isOwner && !string.IsNullOrEmpty(item.Name) && item.Name.ToLower().Contains("owner"))));
        }

        public bool CheckTerm(string termId, int version) => _userTermsOfUseRepository.CheckTerm(termId, version);

        public SiteUser GetSiteUser(HttpContextBase httpContext) => httpContext.GetSiteUser();

        public bool UpdateInsertUserAccepts(string term, int version, int userId)
        {
            var updatedResult = _userTermsOfUseRepository.InsertUpdateUserAccepts(userId, term, version);
            if (!updatedResult)
            {
                // retry to insert the term version and then re-insert new useraccept.
                var isExistedCurrentTerm = _userTermsOfUseRepository.CheckTerm(term, version);
                if (!isExistedCurrentTerm && UpdateTermsOfUseVersion(version, term))
                {
                    updatedResult = _userTermsOfUseRepository.InsertUpdateUserAccepts(userId, term, version);
                }
            }

            return updatedResult;
        }

        public bool UpdateTermsOfUseVersion(int newVersion, string userAgreementIdentity)
            => _userTermsOfUseRepository.UpdateInsertTermOfUse(newVersion, userAgreementIdentity);

        public async Task<SiteUser> PasswordSignInAsync(string userName, string password)
        {
            var userCore = await _accountRepository.PasswordSigninAsync(userName, password);
            return PopulateSiteUser(userCore);
        }

        public async Task<SiteUser> QuerySiteUserAsync(string userName)
        {
            var userCore = await _accountRepository.QueryUserCoreAsync(userName);
            return PopulateSiteUser(userCore);
        }

        public Task<bool> UpdateUserInfoAsync(string personNumber, string firstName, string lastName, string telephone,
                string mobilephone, string email, string street, string zip, string city)
            => _accountRepository.UpdateUserInfoAsync(personNumber, firstName, lastName, telephone, mobilephone, email, street, zip, city);

        public bool IsUserPasswordValid(string password) => PasswordRegex.IsMatch(password);

        public string GeneratePassword()
        {
            var isMatch = false;
            var password = string.Empty;
            while (!isMatch)
            {
                password = RandomPassword.Generate(8, 8);
                isMatch = PasswordRegex.IsMatch(password);
            }

            return password;
        }

        public void UpdateInternalCustomerNumber(HttpContextBase httpContext, CustomerBasicInfo customer)
        {
            if (httpContext?.Response?.Cookies == null || customer == null) return;
            var cookie = _tokenGenerator.Encrypt(customer);
            httpContext.SetCookie(Cookies.InternalActiveCustomer, cookie, true);

        }

        public CustomerBasicInfo GetInternalCustomerNumber(HttpContextBase httpContext)
        {
            if (httpContext.Request.Cookies.AllKeys?.Contains(Cookies.InternalActiveCustomer) != true) return null;

            var cookieValue = httpContext.Request.Cookies[Cookies.InternalActiveCustomer];
            try
            {
                var customer = _tokenGenerator.Decrypt<CustomerBasicInfo>(cookieValue?.Value);
                return customer;
            }
            catch (Exception ex) when (ex is CryptographicException || ex is ArgumentException)
            {
                return null;
            }
        }

        public bool EndInternalCustomerSession(HttpContextBase httpContext)
        {
            if (httpContext.Request.Cookies.AllKeys?.Contains(Cookies.InternalActiveCustomer) != true) return true;
            var cookieValue = httpContext.Request.Cookies[Cookies.InternalActiveCustomer];
            try
            {
                if (cookieValue == null) return false;
                cookieValue.Expires = DateTime.Now.AddDays(-1);
                httpContext.Response.SetCookie(cookieValue);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task ActivateAccount(string userName) => _accountRepository.ActivateUserAsync(userName);

        private SiteUser PopulateSiteUser(UserCore userCore)
        {
            if (userCore == null) return null;

            var agreementPage = ContentExtensions.GetUserAgreementPage();
            if (agreementPage == null) return new SiteUser(userCore);

            var userAcceptedAgreement = _userTermsOfUseRepository.CheckUserAccepts(userCore.UserId, agreementPage.TermId);

            return new SiteUser(userCore)
            {
                AcceptedAgreementVersion = userAcceptedAgreement ? agreementPage.Version : -1
            };
        }
    }
}
