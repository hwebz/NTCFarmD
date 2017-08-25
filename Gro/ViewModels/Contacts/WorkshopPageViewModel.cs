using Gro.Core.ContentTypes.Pages.Contacts;
using Gro.Core.DataModels.Contacts;
using System.Collections.Generic;

namespace Gro.ViewModels.Contacts
{
    public class WorkshopPageViewModel : PageViewModel<WorkshopPage>
    {
        public IEnumerable<GarageWorkshop> Workshops { get; set; }
        public WorkshopPageViewModel(WorkshopPage page, IEnumerable<GarageWorkshop> workshops) : base(page)
        {
            Workshops = workshops;
        }
    }

    public class WorkshopDetailPageViewModel : PageViewModel<WorkshopPage>
    {
        public GarageWorkshop Workshop { get; set; }
        public IEnumerable<SalesPerson> SalesMen { get; set; }

        public WorkshopDetailPageViewModel(WorkshopPage page) : base(page)
        {
        }
    }
}
