using System.Collections.Generic;
using Gro.Core.ContentTypes.Pages;

namespace Gro.ViewModels.Pages.InformationArchive
{
    public class InformationArchivePageViewModel : PageViewModel<InformationArchivePage>
    {
        public InformationArchivePageViewModel(InformationArchivePage currentPage) : base(currentPage)
        {
            LatestInformationPages = new List<InformationPage>();
        }

        public IEnumerable<InformationPage> LatestInformationPages { get; set; }
    }
}
