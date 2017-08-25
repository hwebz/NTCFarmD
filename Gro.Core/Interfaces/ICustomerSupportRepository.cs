using Gro.Core.DataModels.CustomerSupport;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gro.Core.DataModels.Organization;

namespace Gro.Core.Interfaces
{
    public interface ICustomerSupportRepository
    {
        /// <summary>
        /// Get a list of customers by organization number
        /// If <para name="orgNumber"></para> is empty and <para name="onlyNonActive"> is true, then return all pending customers
        /// </summary> 
        Task<IEnumerable<CustomerInfo>> GetCustomersByOrganizationNumberAsync(string orgNumber, bool onlyNonActive);

        /// <summary>
        /// Save a customer to the system
        /// </summary>
        Task<int> SaveCustomerAsync(CustomerInfo customerInfo);

        /// <summary>
        /// Get a customer by 
        /// </summary>
        /// <param name="customerNumber"></param>
        /// <returns></returns>
        Task<Customer> GetCustomerByNumberAsync(string customerNumber);

        int GetCustomerLM2Id(string customerNumber);
        Task<CustomerInvoiceAddress> GetCustomerInvoiceAddress(string customerNumber);

        Task<CustomerDeliveryAddress[]> GetCustomersDeliveryAddressesAsync(string customerNumber);
    }
}
