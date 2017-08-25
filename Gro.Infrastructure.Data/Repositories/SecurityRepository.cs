using System;
using System.Linq;
using System.Web.Security;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.SecurityService;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gro.Infrastructure.Data.Interceptors.Attributes;
using Gro.Infrastructure.Data.PersonService;
using Gro.Core.DataModels.Security;
using Gro.Infrastructure.Data.RequestService;

namespace Gro.Infrastructure.Data.Repositories
{
    public class SecurityRepository : ISecurityRepository
    {
        private readonly ISecurityService _securityService;
        private readonly WSPersonService _personService;
        private readonly WSRequestService _wsRequestService;
        private readonly TicketProvider _ticketProvider;

        private string _ticket;
        private string Ticket => _ticket ?? (_ticket = _ticketProvider.GetTicket());

        public SecurityRepository(ISecurityService securityService,
            WSPersonService personService,
            WSRequestService wsRequestService,
            TicketProvider ticketProvider)
        {
            _securityService = securityService;
            _personService = personService;
            _ticketProvider = ticketProvider;
            _wsRequestService = wsRequestService;
        }

        #region MembershipProvider

        public string GetPassword(string username, string answer)
            => _securityService.GetPassword(new RequestUser { UserId = username }, answer, Ticket);

        public bool ValidateUser(string username, string password)
            => _securityService.ValidateUser(new RequestUser { UserId = username, Password = password, KundNr = "" }, Ticket);

        public ResponseUser GetUser(string username, bool userIsOnline) => _securityService.GetUser(new RequestUser { UserId = username }, Ticket);

        public string GetUserNameByEmail(string email) => _securityService.GetUserNameByEmail(email, Ticket)?.UserName;

        public MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var response = _securityService.GetAllUsers(new GetAllUsersRequest(Ticket, pageIndex, pageSize));
            var userCollection = new MembershipUserCollection();

            foreach (var user in response.GetAllUsersResult)
            {
                userCollection.Add(new MembershipUser(
                    user.ProviderName, user.UserName, user.ProviderKey, user.Email, user.PassWordquestion,
                    user.Comment, user.IsActive, user.IsLocketOut,
                    user.CreationDate, user.LastLoginDate, user.LastActivityDate, user.LastPasswordChangedDate,
                    user.LastLockedOutDate));
            }

            totalRecords = response.totalRecords;
            return userCollection;
        }

        public async Task<ResponseUser> FindUserByEmailAsync(string email)
        {
            var userByEmail = await _securityService.FindUserByEmailAsync(email, Ticket);
            if (string.IsNullOrWhiteSpace(userByEmail?.UserName)) return null;

            var user = await _securityService.GetUserAsync(new RequestUser { UserId = userByEmail.UserName }, Ticket);
            return user;
        }

        public MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            var users = _securityService.FindUsersByName(new FindUsersByNameRequest
            {
                pageIndex = pageIndex,
                pageSize = pageSize,
                ticket = Ticket,
                usernameToMatch = usernameToMatch
            });

            totalRecords = users.totalRecords;
            var collection = new MembershipUserCollection();
            foreach (var user in users.FindUsersByNameResult)
            {
                collection.Add(new MembershipUser(user.ProviderName, user.Name, user.ProviderKey, user.Email,
                    user.PassWordquestion, user.Comment,
                    user.IsActive, user.IsLocketOut, user.CreationDate, user.LastLoginDate, user.LastActivityDate,
                    user.LastPasswordChangedDate,
                    user.LastLockedOutDate));
            }
            return collection;
        }

        public MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            var users = _securityService.FindUsersByEmail(new FindUsersByEmailRequest
            {
                pageSize = pageSize,
                pageIndex = pageIndex,
                ticket = Ticket,
                emailToMatch = emailToMatch
            });

            totalRecords = users.totalRecords;
            if (totalRecords == 0)
            {
                return new MembershipUserCollection();
            }

            var result = new MembershipUserCollection();
            foreach (var member in users.FindUsersByEmailResult.Select(u => new MembershipUser(
                u.ProviderName, u.UserName, u.ProviderKey, u.Email, u.PassWordquestion,
                u.Comment, u.IsActive, u.IsLocketOut, u.CreationDate, u.LastLoginDate, u.LastActivityDate,
                u.LastPasswordChangedDate, u.LastLockedOutDate)))
            {
                result.Add(member);
            }
            return result;
        }

        #endregion

        #region RoleProvider

        public bool IsUserInRole(string username, string roleName)
            => _securityService.IsUserInRole(new RequestUser { UserId = username }, roleName, Ticket);

        public bool DeleteRole(string roleName, bool throwOnPopulatedRole)
            => _securityService.DeleteRole(roleName, throwOnPopulatedRole, Ticket);

        public bool RoleExists(string roleName) => _securityService.RoleExists(roleName, Ticket);

        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            var users = usernames.Select(username => new RequestUser { UserId = username }).ToArray();
            var userRoles = roleNames.Select(r => new UserRole { RoleName = r }).ToArray();
            _securityService.RemoveUsersFromRoles(users, userRoles, Ticket);
        }

        public string[] GetUsersInRole(string roleName)
        {
            var users = _securityService.GetUsersInRole(roleName, Ticket);
            return users.Select(x => x.UserName).ToArray();
        }

        public Role[] GetAllRoles() => _securityService.GetAllRoles(Ticket);

        [Cache]
        public Task<Role[]> GetAllRolesAsync() => _securityService.GetAllRolesAsync(Ticket);

        [Cache]
        public UserRole[] GetRolesForUser(string username)
            => _securityService.GetRolesForUser(new RequestUser { UserId = username }, Ticket);

        public UserRole[] GetRolesForUserInOneCustomer(string customerNo, string username)
            => _securityService.GetRolesForUser(new RequestUser { KundNr = customerNo, UserId = username }, Ticket).Where(r => r.Sysrole == false).ToArray();

        public string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            var users = _securityService.FindUsersInRole(roleName, usernameToMatch, Ticket);
            return users.Select(x => x.UserName).ToArray();
        }

        #endregion

        public ResponseUser QueryUser(string userName) => _securityService.GetUser(new RequestUser()
        {
            UserId = userName,
        }, _ticket);

        public Task<ResponseUser> QueryUserAsync(string userName)
            => _securityService.GetUserAsync(new RequestUser { UserId = userName }, _ticket);

        #region UserManagerment

        public Task<bool> SaveUserPictureUrl(int userId, string pictureUrl)
            => _securityService.SaveUserPictureURLAsync(userId, pictureUrl);

        public Task<bool> DeleteUserPictureUrl(int userId)
            => _securityService.SaveUserPictureURLAsync(userId, string.Empty);

        #endregion

        #region IamServices

        public string GetSocialSecurityNumber(string username)
        {
            var personSession = _ticketProvider.GetWsSession();
            // Search for persons
            var persons = _personService.searchPersonsFromRoot(new searchPersonsFromRootRequest
            {
                session = personSession,
                filter = $"(uid={username})",
                attrList = new string[] { }
            });

            if (persons?.searchPersonsFromRootReturn == null || persons.searchPersonsFromRootReturn.Length == 0)
                return string.Empty;

            var attributes = persons.searchPersonsFromRootReturn[0].attributes;
            var personNumber = attributes.FirstOrDefault(a => a.name == "personnumber")?.values?[0];
            return personNumber;
        }

        public async Task<bool> UpdateSocialSecurityNumberAsync(string userName, string socialNumber)
        {
            var personSession = _ticketProvider.GetWsSession();
            // Search for persons
            var personDn = await GetPersonObjectIdByNameAsync(userName);

            if (string.IsNullOrEmpty(personDn)) return false;
            // Creates a attribute list that updates the user settings
            var attributes = new List<WSAttribute>();
            var wsAttr = new WSAttribute { name = IamUserAttributes.PersonNumber, values = new[] { socialNumber } };
            attributes.Add(wsAttr);

            var req = await _personService.modifyPersonAsync(new modifyPersonRequest
            {
                session = personSession,
                personDN = personDn,
                wsAttrs = attributes.ToArray()
            });
            return true;
        }

        public async Task ChangePasswordAsync(string userName, string newPassword)
        {
            var personSession = _ticketProvider.GetWsSession();
            // Search for persons
            var personDn = await GetPersonObjectIdByNameAsync(userName);

            if (string.IsNullOrEmpty(personDn)) throw new ArgumentNullException(nameof(userName), "User does not exist in IAM system");

            var req = await _personService.synchPasswordsAsync(new synchPasswordsRequest()
            {
                session = personSession,
                personDN = personDn,
                password = newPassword,
                notifyByMail = false
            });

            await _wsRequestService.WaitForCompletion(personSession, req.synchPasswordsReturn.requestId);
        }

        public async Task<string> GetPersonObjectIdByNameAsync(string userName)
        {
            var session = _ticketProvider.GetWsSession();
            var persons = await _personService.searchPersonsFromRootAsync(new searchPersonsFromRootRequest
            {
                session = session,
                filter = $"(uid={userName})",
                attrList = new string[] { }
            });

            return persons?.searchPersonsFromRootReturn?.FirstOrDefault()?.itimDN;
        }

        public string GetPersonObjectIdByName(string userName)
        {
            var session = _ticketProvider.GetWsSession();
            var persons = _personService.searchPersonsFromRoot(new searchPersonsFromRootRequest
            {
                session = session,
                filter = $"(uid={userName})",
                attrList = new string[] { }
            });

            return persons?.searchPersonsFromRootReturn?.FirstOrDefault()?.itimDN;
        }

        public Task<UserProfile[]> GetCustomerUsersByProfileAsync(int customerId)
            => _securityService.GetCustomerUsersByProfileAsync(customerId, Ticket);

        [Cache]
        public Task<Profile[]> GetRoleProfilesAsync() => _securityService.GetProfilesAsync(Ticket);

        [Cache]
        public Task<ProfileRole[]> GetRolesOfProfileAsync(string profileId)
            => _securityService.GetProfileRolesAsync(profileId, Ticket);

        public Task<User[]> GetUsersForCustomerAsync(int customerId)
            => _securityService.GetUsersForCustomerAsync(customerId, Ticket);

        #endregion

        public async Task<CustomerCheckCode> MatchCustomerNumberAndOrganizationNumberAsync(string customerNo, string orgNo)
        {
            var result = await _securityService.CheckSSNAgainstOrgNoAsync(customerNo, orgNo, _ticket);
            return (CustomerCheckCode)result;
        }

        public Company GetExistingCompany(string customerNo, string orgNo) => _securityService.GetExistingCustomer(customerNo, orgNo, _ticket);

        public Task<string> GeneratePasswordGuidAsync(string userName) => _securityService.GetPasswordGuidAsync(new RequestUser
        {
            UserId = userName
        }, _ticketProvider.GetTicket());

        public Task<bool> CheckPasswordGuidAsync(string userName, string guid)
            => _securityService.CheckPasswordGuidAsync(new RequestUser { UserId = userName }, guid, _ticketProvider.GetTicket());

        //just generate a new one
        public Task InvalidatePasswordGuid(string userName)
            => _securityService.GetPasswordGuidAsync(new RequestUser { UserId = userName }, _ticketProvider.GetTicket());
    }
}
