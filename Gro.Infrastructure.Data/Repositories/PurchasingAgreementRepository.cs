using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Gro.Core.DataModels.PurchasingAgreements;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.PurchasingAgreementService;

namespace Gro.Infrastructure.Data.Repositories
{
    public class PurchasingAgreementRepository : IPurchasingAgreementRepository
    {
        private readonly IPurchasingAgreementService _service;
        private readonly TicketProvider _ticketProvider;

        public PurchasingAgreementRepository(IPurchasingAgreementService service, TicketProvider ticketProvider)
        {
            _service = service;
            _ticketProvider = ticketProvider;
        }

        private string Ticket => _ticketProvider.GetTicket();

        public async Task<Customer> GetCustomerAsync(string customerId)
            => await _service.GetCustomerAsync(customerId, Ticket);

        public async Task<PriceArea[]> GetPriceAreasAsync() =>await _service.GetPriceAreasAsync(Ticket);
       
        public async Task<string> GetPriceAreaDescAsync(string priceAreaId)
            => await _service.GetPriceAreaDescrAsync(priceAreaId);

        public async Task<PriceArea> GetSelectedPriceArea(string customerId)
        {
            var customer = await GetCustomerAsync(customerId);
            var priceAreaName = await GetPriceAreaDescAsync(customer.CustPriceAreaId);
            return new PriceArea
            {
                PriceAreaId = customer.CustPriceAreaId,
                PriceAreaName = priceAreaName
            };
        }

        //public async Task<PricePeriod[]> GetPricePeriodsForStorageAgreementsAsync(string customerId, string areaId)
        //    => await _service.GetPricePeriodsForStorageAgreementsAsync(customerId, areaId, Ticket);

        public async Task<StorageAgreement[]> GetStorageAgreementsForPriceProtectionAsync(string customerId)
            => await _service.GetStorageAgreementsForPriceProtectionAsync(customerId, Ticket);

        //public async Task<PricePeriod[]> GetPricePeriodsForPriceProtectingStorageAgreementAsync(string areaId,
        //    string productItemId, string grainType)
        //    =>
        //        await
        //            _service.GetPricePeriodsForPriceProtectingStorageAgreementAsync(areaId, productItemId, grainType,
        //                Ticket);

        public async Task<AgreementResponse> SaveStorageAgreementAsync(StorageAgreement agreement)
            => await _service.SaveStorageAgreementAsync(agreement, Ticket);

        public async Task<AgreementResponse> SavePriceProtectedStorageAgreementAsync( PriceProtectStorageAgreement agreement)
            => await _service.SavePriceProtectedStorageAgreementAsync(agreement, Ticket);

        public async Task<AgreementResponse> SaveSpotAndForwardAgreementAsync(SpotAndForwardAgreement agreement)
            => await _service.SaveSpotAndForwardAgreementAsync(agreement, Ticket);

        public async Task<AgreementResponse> SavePoolAgreement(PoolAgreement agreement)
            => await _service.SavePoolAgreementAsync(agreement, Ticket);

        public async Task<bool> SaveCustomerFavoritePriceAreaAsync(string customerId, string priceAreaId)
            => await _service.SaveCustomerFavoritePriceAreaAsync(customerId, priceAreaId, Ticket);

        //public async Task<PricePeriod[]> GetPricePeriodsForPoolAgreementsAsync(
        //    string customerId, string priceAreaId)
        //    => await _service.GetPricePeriodsForPoolAgreementsAsync(customerId, priceAreaId, Ticket);

        //public async Task<PricePeriod[]>
        //    GetPricePeriodsForSpotAndForwardAgreementsAsync(string customerId, string priceAreaId)
        //    => await _service.GetPricePeriodsForSpotAndForwardAgreementsAsync(customerId, priceAreaId, Ticket);

        //public async Task<bool> UpdateCustomerFavoriteProductitem(string customerId, string priceAreaId,
        //    string priceType,
        //    string productItemId, string grainType, bool addFavorite)
        //    =>
        //        await
        //            _service.UpdateCustomerFavoriteProductitemAsync(customerId, priceAreaId, priceType, productItemId,
        //                grainType, addFavorite, Ticket);

        public async Task<PriceWatch[]> GetPriceWatchAsync(string customerId, string ticket)
        {
            return await _service.GetPriceWatchAsync(customerId, ticket);
        }

        public async Task<bool> DeletePriceWatchAsync(int id, string ticket)
        {
            return await _service.DeletePriceWatchAsync(id, ticket);
        }

        public async Task<PricePeriod[]> GetPricePeriodsGrainPriceAsync(string priceAreaId)
        {
            return await _service.GetPricePeriodsGrainPriceAsync(priceAreaId, Ticket);
        }

        public async Task<DryingAgreement> GetDryingAgreementAsync()
        {
            return await _service.GetDryingAgreementAsync(Ticket);
        }

        public async Task<string> SaveDryingAgreementAsync(string customerId)
        {
            return await _service.SaveDryingAgreementAsync(customerId, Ticket);
        }

        public DryingAgreement GetDryingAgreement()
        {
            return _service.GetDryingAgreement(Ticket);
        }

        public Task<StorageAgreement> GetStorageAgreementValuesForPriceProtectionAsync(string agreementId)
        {
            return _service.GetStorageAgreementValuesForPriceProtectionAsync(agreementId, Ticket);
        }

        public Task<Dictionary<string, string>> GetPeriodsPriceProtectingStorageAgreementAsync(string priceAreaId,
            string productItemId, string grainType)
        {
            return _service.GetPeriodsPriceProtectingStorageAgreementAsync(priceAreaId, productItemId, grainType, Ticket);
        }

        public Task<Dictionary<string, string>> GetPeriodsPoolAgreementAsync(string priceAreaId)
        {
            return _service.GetPeriodsPoolAgreementAsync(priceAreaId, Ticket);
        }

        public Task<Dictionary<string, string>> GetPeriodsSpotAndForwardAgreementAsync(string priceAreaId)
        {
            return _service.GetPeriodsSpotAndForwardAgreementAsync(priceAreaId, Ticket);
        }

        public Task<Dictionary<string, string>> GetPeriodsStorageAgreementAsync(string priceAreaId)
        {
            return _service.GetPeriodsStorageAgreementAsync(priceAreaId, Ticket);
        }

        public Product[] GetProductsSpotAndForwardAgreement(string priceAreaId)
        {
            return _service.GetProductsSpotAndForwardAgreement(priceAreaId, Ticket);
        }

        public Task<Product[]> GetGrainTypesSpotAndForwardAgreementAsync(string productItemId, string priceAreaId)
        {
            return _service.GetGrainTypesSpotAndForwardAgreementAsync(productItemId, priceAreaId, Ticket);
        }

        public Product[] GetGrainTypesSpotAndForwardAgreement(string productItemId, string priceAreaId)
        {
            return _service.GetGrainTypesSpotAndForwardAgreement(productItemId, priceAreaId, Ticket);
            
        }
        
        public Task<Product[]> GetProductsPoolAgreementAsync(string priceAgreaId)
        {
            return _service.GetProductsPoolAgreementAsync(priceAgreaId, Ticket);
        }

        public Task<Product[]> GetGrainTypesPoolAgreementAsync(string priceAgreaId, string productItemId)
        {
            return _service.GetGrainTypesPoolAgreementAsync(productItemId, priceAgreaId, Ticket);
        }

        public Task<Dictionary<int, string>> GetModesOfDeliveryPoolAgreementAsync()
        {
            return _service.GetModesOfDeliveryPoolAgreementAsync(Ticket);
        }

        public Task<Product[]> GetProductsStorageAgreementAsync(string priceAgreaId)
        {
            return _service.GetProductsStorageAgreementAsync(priceAgreaId, Ticket);
        }

        public Task<Product[]> GetGrainTypesStorageAgreementAsync(string priceAgreaId, string productItemId)
        {
            return _service.GetGrainTypesStorageAgreementAsync(priceAgreaId, productItemId, Ticket);
        }

        public Task<Dictionary<int, string>> GetModesOfDeliveryStorageAgreementAsync()
        {
            return _service.GetModesOfDeliveryStorageAgreementAsync(Ticket);
        }

        public Customer GetCustomer(string customerId)
        {
            return _service.GetCustomer(customerId, Ticket);
        }
    }
}