using Gro.Infrastructure.Data.RequestService;
using System.Threading.Tasks;

namespace Gro.Infrastructure.Data
{
    public static class WsRequestModule
    {
        private const int DELAY_TIME = 600;

        public static async Task WaitForCompletion(this WSRequestService wsRequestService, PersonService.WSSession wsSession, long requestId)
        {
            var request = await wsRequestService.getRequestAsync(new WSSession
            {
                sessionID = wsSession.sessionID,
                clientSession = wsSession.clientSession
            }, requestId);
            var processStateString = request.processStateString;

            var times = 0;
            while (processStateString != "Completed" && times < 10)
            {
                //poll
                await Task.Delay(DELAY_TIME);
                request = await wsRequestService.getRequestAsync(new WSSession
                {
                    sessionID = wsSession.sessionID,
                    clientSession = wsSession.clientSession
                }, requestId);
                processStateString = request.processStateString;
                times++;
            }
        }
    }
}
