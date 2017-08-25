using Gro.Core.Interfaces;
using System;
using System.Threading.Tasks;
using Gro.Core.DataModels.Security;
using Gro.Infrastructure.Data.SecurityService;
using Gro.Infrastructure.Data.PersonService;
using System.Collections.Generic;
using System.Linq;
using Gro.Infrastructure.Data.Interceptors.Attributes;
using Gro.Infrastructure.Data.RequestService;

namespace Gro.Infrastructure.Data.Repositories
{
    public class OrganizationUserRepository : IOrganizationUserRepository
    {
        private readonly TicketProvider _ticketProvider;
        private readonly ISecurityService _securityService;
        private readonly WSPersonService _personService;
        private readonly WSRequestService _wsRequestService;

        private string Ticket => _ticketProvider.GetTicket();

        private PersonService.WSSession WsSession => _ticketProvider.GetWsSession();

        public OrganizationUserRepository(
            ISecurityService securityService,
            WSPersonService personService,
            WSRequestService wsRequestService,
            TicketProvider ticketProvider)
        {
            _ticketProvider = ticketProvider;
            _securityService = securityService;
            _personService = personService;
            _wsRequestService = wsRequestService;
        }

        [Cache]
        [RefreshCacheParam("refreshCache")]
        public CustomerBasicInfo[] GetOrganizationsOfUser(string userName, bool refreshCache)
            => _securityService.GetCustomersForUser(new RequestUser { UserId = userName }, Ticket);

        [Cache]
        public Task<User[]> GetUsersOfOrganizationAsync(CustomerBasicInfo organization)
            => _securityService.GetUsersForCustomerAsync(organization.CustomerId, Ticket);

        [Cache]
        public Task<CustomerBasicInfo[]> GetOrganizationsOfUserAsync(string userName)
            => _securityService.GetCustomersForUserAsync(new RequestUser { UserId = userName }, Ticket);

        [CacheInvalidate("GetOrganizationsOfUser_{user.UserName}")]
        [CacheInvalidate("GetUsersOfOrganization_{user.UserName}")]
        public async Task AddUserToOrganizationAsync(UserCore user, CustomerBasicInfo customer, string[] roleIds = null)
        {
            if (roleIds == null) roleIds = new string[0];

            var personSearch = _personService.searchPersonsFromRoot(new searchPersonsFromRootRequest
            {
                session = WsSession,
                filter = $"(uid={user.UserName})",
                attrList = new string[] { }
            });

            var person = personSearch?.searchPersonsFromRootReturn?.FirstOrDefault();
            if (person == null) throw new ArgumentNullException(nameof(user));

            var userOrganizations = person.attributes?
                                        .FirstOrDefault(a => a.name == "lmorg")?.values?
                                        .Where(r => r != customer.CustomerNo)
                                        .ToList() ?? new List<string>();

            userOrganizations.Add(customer.CustomerNo);
            var newOrgs = userOrganizations.ToArray();

            var addOrgResponse = await _personService.modifyPersonAsync(new modifyPersonRequest
            {
                session = _ticketProvider.GetWsSession(),
                personDN = person.itimDN,
                wsAttrs = new[] { new WSAttribute { name = "lmorg", values = newOrgs } }
            });

            await _wsRequestService.WaitForCompletion(WsSession, addOrgResponse.modifyPersonReturn.requestId);

            var userOrgRoles = person.attributes?
                                   .FirstOrDefault(a => a.name == "lmorgroles")?.values?
                                   .Where(r => !r.StartsWith($"{customer.CustomerNo};"))
                                   //filter out all roles already in the org
                                   .ToList() ?? new List<string>();


            userOrgRoles.AddRange(roleIds.Select(rid => $"{customer.CustomerNo};{rid}"));
            var newRoles = userOrgRoles.ToArray();

            var addRoleResponse = await _personService.modifyPersonAsync(new modifyPersonRequest
            {
                session = _ticketProvider.GetWsSession(),
                personDN = person.itimDN,
                wsAttrs = new[] { new WSAttribute { name = "lmorgroles", values = newRoles } }
            });
            await _wsRequestService.WaitForCompletion(WsSession, addRoleResponse.modifyPersonReturn.requestId);
        }

        [Cache]
        public Task<UserRole[]> GetUserCustomerRolesAsync(string userName, string organizationNumber)
            => _securityService.GetRolesForUserAsync(new RequestUser { UserId = userName, KundNr = organizationNumber }, Ticket);

        [Cache]
        public UserRole[] GetUserCustomerRoles(string userName, string organizationNumber)
            => _securityService.GetRolesForUser(new RequestUser { UserId = userName, KundNr = organizationNumber }, Ticket);

        [CacheInvalidate("GetUserCustomerRoles_{userName}_{customer.CustomerNo}")]
        public async Task UpdateUserCustomerRolesAsync(string userName, CustomerBasicInfo customer, string[] roleIds)
        {
            //update the user's lmorg attribute
            var session = _ticketProvider.GetWsSession();
            var personSearch = _personService.searchPersonsFromRoot(new searchPersonsFromRootRequest
            {
                session = session,
                filter = $"(uid={userName})",
                attrList = new string[0]
            });

            var person = personSearch.searchPersonsFromRootReturn.FirstOrDefault();
            if (person == null) throw new ArgumentNullException(nameof(userName));

            var userOrgRoles = person.attributes
                                   .FirstOrDefault(a => a.name == "lmorgroles")?.values?
                                   .Where(r => !r.StartsWith($"{customer.CustomerNo};"))
                                   .ToList() ?? new List<string>();

            userOrgRoles.AddRange(roleIds.Select(rid => $"{customer.CustomerNo};{rid}"));
            var newRoles = userOrgRoles.ToArray();
            var updateOrganizationResult = await _personService.modifyPersonAsync(new modifyPersonRequest
            {
                session = _ticketProvider.GetWsSession(),
                personDN = person.itimDN,
                wsAttrs = new[] { new WSAttribute { name = "lmorgroles", values = newRoles } }
            });

            await _wsRequestService.WaitForCompletion(WsSession, updateOrganizationResult.modifyPersonReturn.requestId);
        }

        [CacheInvalidate("GetUserCustomerRoles_{user.UserName}_{customer.CustomerNo}")]
        [CacheInvalidate("GetOrganizationsOfUser_{user.UserName}")]
        public async Task RemoveUserFromOrganizationsAsync(UserCore user, CustomerBasicInfo customer)
        {
            //check for owner
            if (customer.OwnerUserId == user.UserId)
            {
                throw new InvalidOperationException($"Cannot remove user {user.UserName} because it is the owner of organization {customer.CustomerNo}");
            }

            //update the user's lmorg attribute
            var session = _ticketProvider.GetWsSession();
            var personSearch = await _personService.searchPersonsFromRootAsync(new searchPersonsFromRootRequest
            {
                session = session,
                filter = $"(uid={user.UserName})",
                attrList = new string[] { }
            });

            var person = personSearch?.searchPersonsFromRootReturn?.FirstOrDefault();
            if (person == null) throw new ArgumentNullException(nameof(user));

            var userOrgRoles = person.attributes?
                                   .FirstOrDefault(a => a.name == "lmorgroles")?.values?
                                   .Where(r => !r.StartsWith($"{customer.CustomerNo};"))?
                                   .ToArray() ?? new string[0];

            var removeRoleResponse = await _personService.modifyPersonAsync(new modifyPersonRequest
            {
                session = _ticketProvider.GetWsSession(),
                personDN = person.itimDN,
                wsAttrs = new[] { new WSAttribute { name = "lmorgroles", values = userOrgRoles } }
            });

            await _wsRequestService.WaitForCompletion(_ticketProvider.GetWsSession(), removeRoleResponse.modifyPersonReturn.requestId);

            var userOrganizations = person.attributes?
                                        .FirstOrDefault(a => a.name == "lmorg")?.values?
                                        .Where(r => r != customer.CustomerNo)?
                                        .ToArray() ?? new string[0];

            var removeOrgResponse = await _personService.modifyPersonAsync(new modifyPersonRequest
            {
                session = _ticketProvider.GetWsSession(),
                personDN = person.itimDN,
                wsAttrs = new[] { new WSAttribute { name = "lmorg", values = userOrganizations } }
            });
            await _wsRequestService.WaitForCompletion(_ticketProvider.GetWsSession(), removeOrgResponse.modifyPersonReturn.requestId);
        }

        public async Task<User> GetOwnerAsync(CustomerBasicInfo customer)
        {
            var users = await _securityService.GetCustomerUsersByProfileAsync(customer.CustomerId, Ticket);
            var customerUsers = await _securityService.GetUsersForCustomerAsync(customer.CustomerId, Ticket);
            var owner = users.FirstOrDefault(u => u.Profile == "Owner");
            if (owner == null) return null;

            var ownerUser = customerUsers.FirstOrDefault(c => c.Id == owner.UserId);
            return ownerUser;
        }

        public async Task<User> GetOwnerAsync(int customerId)
        {
            var allUserProfiles = await _securityService.GetCustomerUsersByProfileAsync(customerId, Ticket);
            allUserProfiles = allUserProfiles ?? new UserProfile[0];
            var ownerProfile = allUserProfiles.FirstOrDefault(x => string.Equals(x.ProfileId, "Owner", StringComparison.OrdinalIgnoreCase));
            if (ownerProfile == null) return null;

            var users = await _securityService.GetUsersForCustomerAsync(customerId, Ticket);
            users = users ?? new User[0];
            var owner = users.FirstOrDefault(u => u.Id == ownerProfile.UserId);
            return owner;
        }

        public async Task<OrganizationUser[]> GetOrganizationUsersByProfileAsync(CustomerBasicInfo customer)
        {
            var orgUsers = await GetUsersOfOrganizationAsync(customer);
            var usersByProfiles = await _securityService.GetCustomerUsersByProfileAsync(customer.CustomerId, Ticket);
            var userAndRoles = await Task.WhenAll(orgUsers.Select(async user =>
            {
                var roles = await GetUserCustomerRolesAsync(user.UserId, customer.CustomerNo);
                var roleProfile = usersByProfiles.FirstOrDefault(p => p.UserId == user.Id);
                return new OrganizationUser
                {
                    Email = user.Email,
                    Name = user.Name,
                    Mobile = user.Mobile,
                    Phone = user.Phone,
                    ProfilePicUrl = user.ProfilePicUrl,
                    UserName = user.UserId,
                    Roles = roles,
                    LockedOut = user.IsLocketOut,
                    RoleProfileId = roleProfile?.ProfileId ?? string.Empty,
                    RoleProfileName = roleProfile?.Profile ?? string.Empty,
                    Userid = user.Id
                };
            }));

            return userAndRoles;
        }
    }
}
