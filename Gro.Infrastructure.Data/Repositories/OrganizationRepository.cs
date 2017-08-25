using System.Threading.Tasks;
using Gro.Core.DataModels.Organization;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.SecurityService;
using Gro.Core.DataModels.Security;
using Gro.Infrastructure.Data.PersonService;
using Gro.Infrastructure.Data.OrganisationService;

namespace Gro.Infrastructure.Data.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ILM2OrganisationService _organisationService;
        private readonly TicketProvider _ticketProvider;
        private readonly ISecurityService _securityService;

        private string _ticket;
        private string Ticket => _ticket ?? (_ticket = _ticketProvider.GetTicket());

        public OrganizationRepository(ILM2OrganisationService organisationService, ISecurityService securityService,
            TicketProvider ticketProvider, WSPersonService personService)
        {
            _organisationService = organisationService;
            _ticketProvider = ticketProvider;
            _securityService = securityService;
        }

        public OrganisationInformation GetCompanyInformation(int organizationId)
            => _organisationService.GetOrganisation(organizationId, Ticket);

        public bool UpdateCompanyInformation(OrganisationInformation companyInformation)
            => _organisationService.UpdateOrganisation(companyInformation, Ticket);

        public BusinessProfile GetBusinessProfile(int customerId)
        {
            var profileResponse = _organisationService.GetOrganisationProfile(customerId, Ticket);
            return new BusinessProfile
            {
                Id = profileResponse.Id,
                Rows = profileResponse.Rows,
                Name = profileResponse.Name,
                CustomerId = profileResponse.CustomerId
            };
        }

        public bool UpdateBusinessProfile(BusinessProfile profile, string customerNumber) => _organisationService.SaveOrganisationProfile(new CustomerProfile
        {
            CustomerId = profile.CustomerId,
            Id = profile.Id,
            Name = profile.Name,
            Rows = profile.Rows
        }, customerNumber, Ticket);

        public Task<bool> SaveOrganizationPictureUrlAsync(int customerId, string pictureUrl)
            => _securityService.SaveCustomerPictureURLAsync(customerId, pictureUrl);

        public Task<bool> DeleteOrganizationPictureUrl(int customerId)
            => _securityService.SaveCustomerPictureURLAsync(customerId, string.Empty);

        public CustomerBasicInfo[] GetOrganizationsOfUserNoCache(string userName)
            => _securityService.GetCustomersForUser(new RequestUser { UserId = userName }, _ticket);

        public Task<CustomerDeliveryAddress[]> GetCustomersDeliveryAddressesAsync(int customerId)
            => _organisationService.GetCustomersDeliveryAddressesAsync(customerId, Ticket);

        public bool UpdateDeliveryAddress(CustomerDeliveryAddress customerDeliveryAddress)
            => _organisationService.UpdateDeliveryAddress(customerDeliveryAddress, Ticket);

        public bool DeleteDeliveryAddress(int customerId, string addressNumber)
            => _organisationService.DeleteDeliveryAddress(customerId, addressNumber, Ticket);

        public Task<DeliveryReceiver[]> GetDeliveryAddressReceiversAsync(int organisationId, string addressNr)
            => _organisationService.GetDeliveryAddressReceiversAsync(organisationId, addressNr);

        public Task<bool> DeleteDeliveryAddressReceiversAsync(int organisationId, int[] userIds, string addressNr)
            => _organisationService.DeleteDeliveryAddressReceiversAsync(organisationId, userIds, addressNr);

        public Task<bool> CreateDeliveryAddressReceiversAsync(int organisationId, int[] userIds, string addressNr)
            => _organisationService.CreateDeliveryAddressReceiversAsync(organisationId, userIds, addressNr);

        public Task<CustomerDeliveryAddress> CreateDeliveryAddressAsync(int customerId, string street, string zipCode,
                string city, string phone, string cellphone, string latidtude, string longitude, string directions, SiloItem[] silos)
            => _organisationService.CreateDeliveryAddressAsync(customerId, street, zipCode, city, phone, cellphone,
                latidtude, longitude, silos, directions, Ticket);

        public Task<bool> ContactCustomerServiceAsync(string subject, string message, bool sendCopy,
            string userEmailAddress, string name, string customerNumber) => _organisationService.ContactCustomerServiceAsync(subject, message, sendCopy, userEmailAddress, Ticket, name, customerNumber);

        public Task<CustomerRegistration> GetExistingRegistrationAsync(string customerNumber, string organizationNumber)
            => _organisationService.GetCustomerRegistrationContainerAsync(customerNumber, organizationNumber, Ticket);

        public Task<bool> SaveCustomerRegistrationAsync(CustomerRegistration customerRegistration)
            => _organisationService.SaveCustomerRegistrationAsync(customerRegistration, Ticket);
    }
}
