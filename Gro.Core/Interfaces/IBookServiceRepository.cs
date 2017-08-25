using Gro.Core.DataModels.BookService;
using Gro.Core.DataModels.Security;
using System.Threading.Tasks;

namespace Gro.Core.Interfaces
{
    public interface IBookServiceRepository
    {
        Task<string> BookingService(BookServiceInfo bookservice);
        Task<User> GetEmailByOwnerUser(int ownerId, int customerId);

        Task<GarageInfo> GetGarageInfo(string customerNumber, int organisationId, string ticket);
    }
}
