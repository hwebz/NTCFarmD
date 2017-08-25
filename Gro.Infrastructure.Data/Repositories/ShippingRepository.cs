using System.Threading.Tasks;
using Gro.Core.DataModels.ShippingDtos;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.ShippingService;

namespace Gro.Infrastructure.Data.Repositories
{
    public class ShippingRepository : IShippingRepository
    {
        private readonly IFraktService _service;

        public ShippingRepository(IFraktService service)
        {
            _service = service;
        }

        public DeliveryFeeResponse GetDeliveryFee(DeliveryFeeRequest deliveryFeeRequest, string ticket)
            => _service.GetDeliveryFee(deliveryFeeRequest, ticket);

        public Task<DeliveryFeeResponse> GetDeliveryFeeAsync(DeliveryFeeRequest deliveryFeeRequest, string ticket)
            => _service.GetDeliveryFeeAsync(deliveryFeeRequest, ticket);
    }
}
