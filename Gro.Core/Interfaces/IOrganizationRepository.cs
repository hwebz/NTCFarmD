using Gro.Core.DataModels.Organization;
using System.Threading.Tasks;

namespace Gro.Core.Interfaces
{
    public interface IOrganizationRepository
    {
        OrganisationInformation GetCompanyInformation(int organizationId);
        bool UpdateCompanyInformation(OrganisationInformation companyInformation);
        BusinessProfile GetBusinessProfile(int customerId);
        bool UpdateBusinessProfile(BusinessProfile profile, string customerNumber);
        Task<bool> SaveOrganizationPictureUrlAsync(int customerId, string pictureUrl);
        Task<bool> DeleteOrganizationPictureUrl(int customerId);
        Task<CustomerDeliveryAddress[]> GetCustomersDeliveryAddressesAsync(int customerId);
        bool UpdateDeliveryAddress(CustomerDeliveryAddress customerDeliveryAddress);
        bool DeleteDeliveryAddress(int customerId, string addressNumber);
        Task<DeliveryReceiver[]> GetDeliveryAddressReceiversAsync(int organisationId, string addressNr);
        Task<bool> DeleteDeliveryAddressReceiversAsync(int organisationId, int[] userIds, string addressNr);
        Task<bool> CreateDeliveryAddressReceiversAsync(int organisationId, int[] userIds, string addressNr);
        Task<CustomerDeliveryAddress> CreateDeliveryAddressAsync(int customerId, string street, string zipCode,
            string city, string phone, string cellphone, string latidtude, string longitude, string directions, SiloItem[] silos);
        Task<bool> ContactCustomerServiceAsync(string subject, string message, bool sendCopy, string userEmailAddress,
            string name, string customerNumber);

        Task<CustomerRegistration> GetExistingRegistrationAsync(string customerNumber, string organizationNumber);

        /// <summary>
        /// Save a customer registration for activation
        /// </summary>
        Task<bool> SaveCustomerRegistrationAsync(CustomerRegistration customerRegistration);
    }
}
