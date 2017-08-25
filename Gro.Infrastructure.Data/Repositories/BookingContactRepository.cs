using Gro.Core.Interfaces;
using System.Threading.Tasks;
using Gro.Core.DataModels.Contacts;
using Gro.Infrastructure.Data.PurchasingMobileService;
using System.Linq;
using Gro.Core.DataModels.Security;
using Gro.Infrastructure.Data.Interceptors.Attributes;

namespace Gro.Infrastructure.Data.Repositories
{
    public class BookingContactRepository : IBookingContactRepository
    {
        private readonly IPurchasingMobileService _purchasingMobileService;
        private readonly TicketProvider _ticketProvider;

        public BookingContactRepository(IPurchasingMobileService purchasingMobileService, TicketProvider ticketProvider)
        {
            _purchasingMobileService = purchasingMobileService;
            _ticketProvider = ticketProvider;
        }

        public async Task<SalesPerson[]> GetAllSalesMenAsync(string category)
        {
            var salesMen = await _purchasingMobileService.GetSalesMen_FarmdayAsync(new GetSalesMen_FarmdayRequest
            {
                ticket = _ticketProvider.GetTicket(),
                caegory = category
            });

            return salesMen.GetSalesMen_FarmdayResult.Select(SalePersonFromSaleMan).ToArray();
        }

        public async Task<SalesPerson[]> SearchSalesMenAsync(string category, string query)
        {
            var salesMen = await _purchasingMobileService.SearchSalesman_FarmdayAsync(new SearchSalesman_FarmdayRequest
            {
                ticket = _ticketProvider.GetTicket(),
                caegory = category,
                searchString = query
            });

            return salesMen.SearchSalesman_FarmdayResult.Select(SalePersonFromSaleMan).ToArray();
        }

        [Cache(Key = "GetGarages_{customer.CustomerNo}_{city}")]
        public async Task<GarageWorkshop[]> GetGaragesAsync(CustomerBasicInfo customer, string city)
        {
            var result = await _purchasingMobileService.GetGaragesAsync(new GetGaragesRequest
            {
                searchString = city,
                ticket = _ticketProvider.GetTicket(),
                organisationId = customer?.CustomerId ?? 0,
                customerNumber = customer?.CustomerNo
            });

            return result.GetGaragesResult.Select(x => new GarageWorkshop
                {
                    Address = x.address,
                    City = x.city,
                    LmStar = x.lmStar,
                    OwnStar = x.ownStar
                })
                .ToArray();
        }

        public async Task<SalesPerson[]> GetSalemenByCityAsync(string city)
        {
            var result = await _purchasingMobileService.GetGarageAsync(new GetGarageRequest
            {
                city = city,
                ticket = _ticketProvider.GetTicket()
            });

            return result.GetGarageResult.Select(SalePersonFromSaleMan).ToArray();
        }

        private static SalesPerson SalePersonFromSaleMan(Salesman salesman) => new SalesPerson
        {
            Address = salesman.address,
            Cellphone = salesman.cellphone,
            City = salesman.city,
            Description = salesman.description,
            Division = salesman.division,
            Email = salesman.email,
            Id = salesman.id,
            Name = salesman.name,
            Phone = salesman.phone,
            Picttype = salesman.picttype,
            Picturl = salesman.picturl,
            Surname = salesman.surname,
            Team = salesman.team,
            Zipcode = salesman.zipcode,
            Fax = salesman.Fax
        };

        [CacheInvalidate("GetStarredGarage_{customer.CustomerNo}")]
        public async Task<bool> SetGarageStarAsync(CustomerBasicInfo customer, string city)
        {
            var setStartResult = await _purchasingMobileService.SetStarGarageAsync(new SetStarGarageRequest
            {
                city = city,
                customerNumber = customer.CustomerNo,
                organisationId = customer.CustomerId,
                ticket = _ticketProvider.GetTicket()
            });

            return setStartResult.SetStarGarageResult;
        }

        [CacheInvalidate("GetStarredGarage_{customer.CustomerNo}")]
        public async Task<bool> RemoveGarageStarAsync(CustomerBasicInfo customer, string city)
        {
            var removeStarResult = await _purchasingMobileService.RemoveStarGarageAsync(new RemoveStarGarageRequest
            {
                city = city,
                customerNumber = customer.CustomerNo,
                organisationId = customer.CustomerId,
                ticket = _ticketProvider.GetTicket()
            });

            return removeStarResult.RemoveStarGarageResult;
        }

        /// <summary>
        /// Get the starred city of customer
        /// </summary>
        [Cache(Key = "GetStarredGarage_{customer.CustomerNo}")]
        public async Task<string> GetStarredGarageAsync(CustomerBasicInfo customer)
        {
            var response = await _purchasingMobileService.GetStarGarageAsync(new GetStarGarageRequest
            {
                ticket = _ticketProvider.GetTicket(),
                customerNumber = customer.CustomerNo,
                organisationId = customer.CustomerId
            });

            return response?.GetStarGarageResult;
        }

        public async Task<string> RequestBooking(string machineModel, string machineSerialNumber, string machineRegistration, string message,
            CustomerBasicInfo customer, GarageWorkshop workshop, string userName, string userEmail, string userPhone,
            string ownerEmail, string garageEmail, bool sendCopy)
        {
            var result = await _purchasingMobileService.BookServiceAsync(new BookServiceRequest
            {
                ownerEmail = ownerEmail,
                userEmail = userEmail,
                sendCopy = sendCopy,
                ticket = _ticketProvider.GetTicket(),
                bookingInfo = new GarageBooking
                {
                    CustomerName = customer.CustomerName,
                    customerNumber = customer.CustomerNo,
                    garagecity = workshop.City,
                    garageemail = garageEmail,
                    markmodel = machineModel,
                    message = message,
                    organisationId = customer.CustomerId.ToString(),
                    PhoneNr = userPhone,
                    registrationno = machineRegistration,
                    serialno = machineSerialNumber,
                    UserName = userName
                }
            });

            return result?.BookServiceResult;
        }
    }
}
