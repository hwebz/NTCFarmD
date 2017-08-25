using System.Collections.Generic;
using Gro.Business.Paging;
using Gro.Core.ContentTypes.Pages.AppPages;
using Gro.Core.DataModels.DeliveryNoteDtos;

namespace Gro.ViewModels.Pages.AppPages.DeliveryNote
{
    public class DeliveryNotePageViewModel : PageViewModel<DeliveryNotePage>
    {
        public DeliveryNotePageViewModel(DeliveryNotePage currentPage) : base(currentPage)
        {
        }

        public IEnumerable<FoljesedelResponse> ListDeliveries { get; set; }
        public Pager Pager { get; set; }
    }
}
