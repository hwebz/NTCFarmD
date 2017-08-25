using System.Collections.Generic;
using Gro.Core.ContentTypes.Pages.Organization;

namespace Gro.ViewModels.Pages.Organization
{
    public class HandleDeliveryAddressesViewModel : PageViewModel<HandleDeliveryAddressPage>
    {
        public HandleDeliveryAddressesViewModel(HandleDeliveryAddressPage currentPage) : base(currentPage)
        {
            DeliveryAddresses = new List<AddressViewModel>();
            EditingDeliveryAddress = null;
        }

        public  IList<AddressViewModel> DeliveryAddresses { get; set; }
        
        public  SingleDeliveryAddressViewModel EditingDeliveryAddress { get; set; }

        public bool IsEditing => EditingDeliveryAddress != null;

        public bool HasBeenUpdated { get; set; }

        public bool HasBeenDeleted { get; set; }

        public bool UpdateSuccess { get; set; }
        public bool HasBeenAdded { get; set; }
    }
}