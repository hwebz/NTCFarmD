using System;
using System.Threading.Tasks;
using Gro.Core.DataModels.Security;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.PersonService;
using Gro.Infrastructure.Data.RequestService;

namespace Gro.Infrastructure.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly WSPersonService _personService;
        private readonly ISecurityRepository _securityRepo;
        private readonly WSRequestService _wsRequestService;
        private readonly TicketProvider _ticketProvider;

        public AccountRepository(WSPersonService personService, ISecurityRepository securityRepo, WSRequestService wsRequestService, TicketProvider ticketProvider)
        {
            _personService = personService;
            _securityRepo = securityRepo;
            _ticketProvider = ticketProvider;
            _wsRequestService = wsRequestService;
        }

        private PersonService.WSSession WsSession => _ticketProvider.GetWsSession();

        private static WSAttribute CreateWsAttribute(string name, string value) => new WSAttribute
        {
            name = name,
            values = new[] { value }
        };

        public async Task<UserCore> QueryUserCoreAsync(string userName)
        {
            var cgiUser = await _securityRepo.QueryUserAsync(userName);
            if (cgiUser == null) return null;

            var itimUserDn = await _securityRepo.GetPersonObjectIdByNameAsync(userName);

            if (string.IsNullOrWhiteSpace(itimUserDn)) return null;

            return new UserCore
            {
                UserId = cgiUser.DbUserId,
                UserName = userName,
                FirstName = cgiUser.FirstName,
                LastName = cgiUser.LastName,
                PersonDn = itimUserDn,
                ProfilePicUrl = cgiUser.ProfilePicUrl,
                Email = cgiUser.Email
            };
        }

        public async Task<UserCore> PasswordSigninAsync(string userName, string password)
        {
            var authenticated = _securityRepo.ValidateUser(userName, password);
            if (!authenticated) return null;

            //get site user data
            var user = await QueryUserCoreAsync(userName);
            return user;
        }

        public async Task<bool> UpdateUserInfoAsync(string personDn, string firstName, string lastName,
            string telephone, string mobilephone,
            string email, string street, string zip, string city)
        {
            //"mobile,usertype,givenname,postaladdress,cn,erglobalid,l,erroles,uid,erpersonstatus,mail,erparent,postalcode,objectclass,sn,personnumber"
            //var person = await _personService.lookupPersonAsync(_wsSession, personNumber);

            var updateResult = await _personService.modifyPersonAsync(new modifyPersonRequest
            {
                personDN = personDn,
                session = WsSession,
                //date = DateTime.Now,
                wsAttrs = new[]
                {
                    CreateWsAttribute(IamUserAttributes.FirstName, firstName),
                    CreateWsAttribute(IamUserAttributes.Surname, lastName),
                    CreateWsAttribute(IamUserAttributes.Telephone, telephone),
                    CreateWsAttribute(IamUserAttributes.CellPhone, mobilephone),
                    CreateWsAttribute(IamUserAttributes.Email, email),
                    CreateWsAttribute(IamUserAttributes.Address, street),
                    CreateWsAttribute(IamUserAttributes.ZipCode, zip),
                    CreateWsAttribute(IamUserAttributes.City, city),
                }
            });

            await _wsRequestService.WaitForCompletion(WsSession, updateResult.modifyPersonReturn.requestId);
            return updateResult.modifyPersonReturn.status == 0;
        }

        public async Task<UserCore> CreateUserAsync(string firstName, string lastName, string telephone,
            string mobilephone, string email, string street, string zip, string city, string personNumber, string customerNumber, bool suspense)
        {
            var userName = email;
            var existingUser = await _securityRepo.GetPersonObjectIdByNameAsync(userName);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"UserName {userName} is used");
            }

            var attributes = new[]
            {
                //use email as uid
                CreateWsAttribute(IamUserAttributes.UserName, userName),
                CreateWsAttribute(IamUserAttributes.FirstName, firstName),
                CreateWsAttribute(IamUserAttributes.Surname, lastName),
                CreateWsAttribute(IamUserAttributes.Name, $"{firstName} {lastName}"),
                CreateWsAttribute(IamUserAttributes.Address, $"{street} {city} {zip}"),
                CreateWsAttribute(IamUserAttributes.ZipCode, zip),
                CreateWsAttribute(IamUserAttributes.City, city),
                CreateWsAttribute(IamUserAttributes.Email, email),
                CreateWsAttribute(IamUserAttributes.CellPhone, mobilephone),
                CreateWsAttribute(IamUserAttributes.Telephone, telephone),
                CreateWsAttribute(IamUserAttributes.PersonNumber, personNumber),
                CreateWsAttribute(IamUserAttributes.Org, customerNumber),
                // Has always to be provided with "external" for all end users.
                CreateWsAttribute("usertype", "external"),
                CreateWsAttribute(IamUserAttributes.MinaSidorCunr, customerNumber)
            };

            var person = new WSPerson
            {
                name = $"{firstName} {lastName}",
                profileName = "LMPerson",
                itimDN = $"uid={userName}",
                attributes = attributes,
                select = false
            };

            var result = await _personService.createPersonAsync(new createPersonRequest
            {
                session = WsSession,
                wsPerson = person
            });

            await _wsRequestService.WaitForCompletion(WsSession, result.createPersonReturn.requestId);

            if (result.createPersonReturn.status != 0)
            {
                throw new ApplicationException(result.createPersonReturn.statusString);
            }

            var itimUserDn = await _securityRepo.GetPersonObjectIdByNameAsync(userName);
            //suspense this person

            if (suspense)
            {
                var suspendResponse = await _personService.suspendPersonAsync(WsSession, itimUserDn);
                await _wsRequestService.WaitForCompletion(WsSession, suspendResponse.requestId);
            }

            return new UserCore
            {
                FirstName = firstName,
                LastName = lastName,
                PersonDn = itimUserDn,
                UserName = userName,
                Email = email
            };
        }

        public async Task ActivateUserAsync(string userName)
        {
            var itimUserDn = await _securityRepo.GetPersonObjectIdByNameAsync(userName);
            var result = await _personService.restorePersonAsync(new restorePersonRequest
            {
                personDN = itimUserDn,
                session = WsSession,
                restoreAccounts = true
            });

            await _wsRequestService.WaitForCompletion(WsSession, result.restorePersonReturn.requestId);
        }

        public async Task InactivateUserAsync(string userName)
        {
            var itimUserDn = await _securityRepo.GetPersonObjectIdByNameAsync(userName);
            await _personService.suspendPersonAsync(_ticketProvider.GetWsSession(), itimUserDn);
        }
    }
}
