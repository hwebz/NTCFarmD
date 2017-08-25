using Gro.Core.DataModels.Contacts;
using Gro.Core.DataModels.Security;
using System.Threading.Tasks;

namespace Gro.Core.Interfaces
{
    public interface IBookingContactRepository
    {
        Task<SalesPerson[]> GetAllSalesMenAsync(string category);

        Task<SalesPerson[]> SearchSalesMenAsync(string category, string query);

        Task<GarageWorkshop[]> GetGaragesAsync(CustomerBasicInfo customer, string city);

        Task<SalesPerson[]> GetSalemenByCityAsync(string city);

        Task<bool> SetGarageStarAsync(CustomerBasicInfo customer, string city);

        Task<bool> RemoveGarageStarAsync(CustomerBasicInfo customer, string city);

        Task<string> GetStarredGarageAsync(CustomerBasicInfo customer);

        Task<string> RequestBooking(string machineModel, string machineSerialNumber, string machineRegistration,
            string message, CustomerBasicInfo customer, GarageWorkshop workshop,string userName, string userEmail, string userPhone,
            string ownerEmail, string garageEmail, bool sendCopy);
    }
}
