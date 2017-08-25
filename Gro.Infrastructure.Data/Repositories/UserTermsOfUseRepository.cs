using System;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.UserTermsOfUseService;

namespace Gro.Infrastructure.Data.Repositories
{
    public class UserTermsOfUseRepository : IUserTermsOfUseRepository
    {
        private readonly TicketProvider _ticketProvider;
        private readonly IUserTermsOfUseService _userTermsOfUseService;
        private string _ticket;

        public UserTermsOfUseRepository(IUserTermsOfUseService userTermsOfUseService, TicketProvider ticketProvider)
        {
            _userTermsOfUseService = userTermsOfUseService;
            _ticketProvider = ticketProvider;
        }

        private string Ticket => _ticket ?? (_ticket = _ticketProvider.GetTicket());

        public bool CheckUserAccepts(int userId, string termIdentity)
            => _userTermsOfUseService.CheckUserAccepts(userId, Ticket, termIdentity);

        public bool UpdateInsertTermOfUse(int newVersion, string userAgreementIdentity)
        {
            if (newVersion <= 0 || string.IsNullOrEmpty(userAgreementIdentity))
            {
                return false;
            }
            return _userTermsOfUseService.UpdateInsertTermsOfUse(userAgreementIdentity, newVersion, DateTime.Now, Ticket);
        }

        public bool InsertUpdateUserAccepts(int userId, string term, int version)
            => _userTermsOfUseService.UpdateInsertUserAccepts(userId, term, version, DateTime.Now, Ticket);

        public bool CheckTerm(string termId, int version) => _userTermsOfUseService.CheckTerms(termId, version, Ticket);
    }
}
