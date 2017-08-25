using Gro.Core.ContentTypes.Pages.Contacts;
using Gro.Core.DataModels.Contacts;
using System.Collections.Generic;

namespace Gro.ViewModels.Contacts
{
    public class PersonSearchPageViewModel : PageViewModel<SalesPersonPage>
    {
        public PersonSearchPageViewModel(SalesPersonPage page, IEnumerable<SalesPerson> salesMen) : base(page)
        {
            SalesMen = salesMen;
        }

        public IEnumerable<SalesPerson> SalesMen { get; set; }
    }

    public class SalePersonDetailPageViewModel : PageViewModel<SalesPersonPage>
    {
        public SalesPerson SalesPerson { get; set; }

        public SalePersonDetailPageViewModel(SalesPersonPage page, SalesPerson salesPerson) : base(page)
        {
            SalesPerson = salesPerson;
        }
    }
}
