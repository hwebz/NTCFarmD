using System.Threading.Tasks;
using Gro.Core.DataModels.ShippingDtos;

namespace Gro.Core.Interfaces
{ 
    public interface IShippingRepository
    {
        DeliveryFeeResponse GetDeliveryFee(DeliveryFeeRequest deliveryFeeRequest,string ticket);

        Task<DeliveryFeeResponse> GetDeliveryFeeAsync(DeliveryFeeRequest deliveryFeeRequest, string ticket);
    }
}