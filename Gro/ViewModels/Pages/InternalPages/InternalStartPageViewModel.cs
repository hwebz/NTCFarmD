using Gro.Core.ContentTypes.Pages.InternalPages;
using Gro.Core.DataModels.CustomerSupport;
using System.Collections.Generic;

namespace Gro.ViewModels.Pages.InternalPages
{
    public class InternalStartPageViewModel : PageViewModel<InternalStartPage>
    {
        public IEnumerable<CustomerInfo> Customers { get; set; }

        public InternalStartPageViewModel(InternalStartPage page, IEnumerable<CustomerInfo> customers) : base(page)
        {
            Customers = customers ?? new CustomerInfo[0];
        }
    }
}
