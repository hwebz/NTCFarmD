using System.Collections.Generic;
using Gro.Core.ContentTypes.Pages.InternalPages;
using Gro.Core.DataModels.CustomerSupport;
using Gro.Core.DataModels.Organization;
using Gro.Core.DataModels.Security;
using Gro.ViewModels.Pages.Organization;

namespace Gro.ViewModels.Pages.InternalPages
{
    public class CustomerCardPageViewModel : PageViewModel<CustomerCardPage>
    {
        public CustomerCardPageViewModel(CustomerCardPage currentPage) : base(currentPage)
        {
        }
        public IList<AddressViewModel> DeliveryAddresses { get; set; }

        public User Owner { get; set; }

        public CustomerInvoiceAddress InvoiceAddress { get; set; }
        
        //public OrganizationUser[] Users { get; set; }
        
        public Customer Customer { get; set; }

        public bool IsFromUserCard { get; set; }
    }
}