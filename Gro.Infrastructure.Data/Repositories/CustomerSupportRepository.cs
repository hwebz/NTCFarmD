using Gro.Core.Interfaces;
using System.Threading.Tasks;
using Gro.Core.DataModels.CustomerSupport;
using Gro.Infrastructure.Data.Lm2CustomerSupportService;
using System.Linq;
using System.Collections.Generic;
using Gro.Core.DataModels.Organization;
using Gro.Infrastructure.Data.LegacyCustomerSupportService;
using Customer = Gro.Core.DataModels.CustomerSupport.Customer;
using Gro.Infrastructure.Data.Interceptors.Attributes;

namespace Gro.Infrastructure.Data.Repositories
{
    public class CustomerSupportRepository : ICustomerSupportRepository
    {
        private readonly ILM2CustomerSupportService _lm2Service;
        private readonly ICustomerSupportService _legacyService;
        private readonly TicketProvider _ticketProvider;

        private string Ticket => _ticketProvider.GetTicket();

        public CustomerSupportRepository(ILM2CustomerSupportService lm2Service, ICustomerSupportService legacyService, TicketProvider ticketProvider)
        {
            _lm2Service = lm2Service;
            _legacyService = legacyService;
            _ticketProvider = ticketProvider;
        }

        [Cache]
        public async Task<IEnumerable<CustomerInfo>> GetCustomersByOrganizationNumberAsync(string orgNumber, bool onlyNonActive)
        {
            var queryResult = await _lm2Service.GetCustomersByOrgNrAsync(new GetCustomersByOrgNrRequest
            {
                organisationNr = orgNumber,
                onlyNonActive = onlyNonActive
            });

            return queryResult.GetCustomersByOrgNrResult?.Select(r => new CustomerInfo
            {
                IsActive = r.Active,
                ActiveSpecified = r.ActiveSpecified,
                CustomerName = r.CustomerName,
                CustomerNumber = r.CustomerNr,
                Email = r.Email,
                OwnerName = r.Name,
                OrganizationNumber = r.OrganisationNr,
                UserId = r.UserId,
                UserIdSpecified = r.UserIdSpecified
            });
        }

        [CacheInvalidate("GetCustomersByOrganizationNumber_{customerInfo.OrganizationNumber}_True")]
        [CacheInvalidate("GetCustomersByOrganizationNumber_{customerInfo.OrganizationNumber}_False")]
        [CacheInvalidate("GetCustomersByOrganizationNumber__True")]
        [CacheInvalidate("GetCustomersByOrganizationNumber__False")]
        public async Task<int> SaveCustomerAsync(CustomerInfo customerInfo)
        {
            var result = await _lm2Service.SaveCustomerAsync(new SaveCustomerRequest
            {
                customer = new Lm2CustomerSupportService.Customer
                {
                    Active = customerInfo.IsActive,
                    CustomerName = customerInfo.CustomerName,
                    CustomerNr = customerInfo.CustomerNumber,
                    Email = customerInfo.Email,
                    OrganisationNr = customerInfo.OrganizationNumber,
                    UserId = customerInfo.UserId
                }
            });

            return result.SaveCustomerResult;
        }

        [Cache]
        public Task<Customer> GetCustomerByNumberAsync(string customerNumber)
        {
            return _legacyService.GetCustomerAsync(customerNumber, Ticket);
        }

        public int GetCustomerLM2Id(string customerNumber)
        {
            var customerResponse = _lm2Service.GetCustomerLM2Id(new GetCustomerLM2IdRequest(customerNumber, Ticket));
            return customerResponse?.GetCustomerLM2IdResult ?? 0;
        }

        public Task<CustomerInvoiceAddress> GetCustomerInvoiceAddress(string customerNumber)
        {
            return _legacyService.GetCustomerInvoiceAddressAsync(customerNumber, Ticket);
        }

        public Task<CustomerDeliveryAddress[]> GetCustomersDeliveryAddressesAsync(string customerNumber)
        {
            return _legacyService.GetCustomersDeliveryAddressesAsync(customerNumber, Ticket);
        }
    }
}
