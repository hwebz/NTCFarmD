using Gro.Infrastructure.Data.Caching;
using Gro.Infrastructure.Data.SecurityService;
using Gro.Infrastructure.Data.SessionService;
using System;

namespace Gro.Infrastructure.Data
{
    public class TicketProvider
    {
        private readonly ISecurityService _securityService;
        private readonly IMemoryCache _cache;

        private readonly string _ticketUserName;
        private readonly string _ticketPassword;
        private readonly string _ticketPrivateKey;
        private readonly string _wsAdminName;
        private readonly string _wsAdminPassword;
        private readonly WSSessionService _sessionService;

        private const string WsSession = nameof(WsSession);

        public TicketProvider(ISecurityService securityService, IMemoryCache cache, WSSessionService sessionService,
            string ticketUserName,
            string ticketPassword,
            string ticketPrivateKey,
            string wsAdminName,
            string wsAdminPassword)
        {
            _securityService = securityService;
            _cache = cache;
            _sessionService = sessionService;
            _ticketUserName = ticketUserName;
            _ticketPassword = ticketPassword;
            _ticketPrivateKey = ticketPrivateKey;
            _wsAdminName = wsAdminName;
            _wsAdminPassword = wsAdminPassword;
        }

        private static string GetTicketCacheKey(string ticketUserName, string ticketPassword, string ticketPrivateKey)
            => $"{ticketUserName}_{ticketPassword}_{ticketPrivateKey}";

        public string GetTicket()
        {
            object ticket;
            var cacheKey = GetTicketCacheKey(_ticketUserName, _ticketPassword, _ticketPrivateKey);
            if (_cache.TryGetValue(cacheKey, out ticket) && ticket is string)
            {
                return (string)ticket;
            }

            string ticketQueryResult = null;
            try
            {
                ticketQueryResult = _securityService.GetTicket(_ticketUserName, _ticketPassword, _ticketPrivateKey);
            }
            catch
            {
                // ignored
            }

            if (!string.IsNullOrWhiteSpace(ticketQueryResult))
            {
                _cache.CreateOrSet(cacheKey, ticketQueryResult, new CacheOptions
                {
                    Strategy = CacheStrategy.Sliding,
                    // refresh ticket once a day?
                    SlidingExpirationTime = TimeSpan.FromDays(1)
                });
            }

            return ticketQueryResult;
        }

        public PersonService.WSSession GetWsSession()
        {
            object session;
            if (_cache.TryGetValue(WsSession, out session))
            {
                var sessionSession = (WSSession)session;

                return new PersonService.WSSession
                {
                    clientSession = sessionSession.clientSession,
                    sessionID = sessionSession.sessionID,
                };
            }

            var sessionObject = _sessionService.login(_wsAdminName, _wsAdminPassword);
            if (sessionObject == null)
            {
                //login failed
                throw new ApplicationException("Login to Iam service failed");
            }

            _cache.CreateOrSet(WsSession, sessionObject, new CacheOptions
            {
                // refresh session once per five minute
                Strategy = CacheStrategy.Absolute,
                ExpirationTimeFromNow = TimeSpan.FromMinutes(5)
            });

            return new PersonService.WSSession
            {
                clientSession = sessionObject.clientSession,
                sessionID = sessionObject.sessionID,
            };
        }
    }
}
