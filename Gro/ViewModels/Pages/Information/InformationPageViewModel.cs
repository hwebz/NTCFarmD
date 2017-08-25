using System.Collections.Generic;
using EPiServer.Core;
using Gro.Core.ContentTypes.Pages;
using Gro.ViewModels.Navigation;

namespace Gro.ViewModels.Pages.Information
{
    public class InformationPageViewModel : PageViewModel<InformationPage>
    {
        public InformationPageViewModel(InformationPage currentPage) : base(currentPage)
        {
            LatestInformationPages = new List<InformationPage>();
            RightNavigation = new List<NavigationItemModel>();
        }

        public string HeadlineForListLatestPages { get; set; }

        public IEnumerable<InformationPage> LatestInformationPages { get; set; }

        public ContentReference ArchivePage { get; set; }

        public IList<NavigationItemModel> RightNavigation { get; set; }

        public string SeemoreText { get; set; }
    }
}
