namespace Gro.ViewModels.Pages.Organization
{
    public class SingleDeliveryAddressViewModel
    {
        public SingleDeliveryAddressViewModel()
        {
            
        }
        public SingleDeliveryAddressViewModel(bool isEdit)
        {
            DeliveryAddress= new AddressViewModel();
            IsEdit = isEdit;
        }

        public AddressViewModel DeliveryAddress { get; set; }

        public string CustomerName { get; set; }

        public bool IsEdit { get; set; }
        public string Instruction { get; set; }

        public bool CanDelete { get; set; }
    }
}
