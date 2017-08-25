using Gro.Core.ContentTypes.Pages.Organization;

namespace Gro.ViewModels.Pages.Organization
{
    public class DeliveryAddressViewModel : PageViewModel<AddDeliveryAddressPage>
    {
        public DeliveryAddressViewModel(AddDeliveryAddressPage currentPage) : base(currentPage)
        {
            DeliveryAddressModel= new SingleDeliveryAddressViewModel(false);
        }
        public SingleDeliveryAddressViewModel DeliveryAddressModel { get; set; }
    }
}