using Gro.Core.ContentTypes.Pages.AppPages;
using Gro.Core.DataModels.GrobarhetDtos;

namespace Gro.ViewModels.Pages.AppPages.Grobarhet
{
    public class GrobarhetPageViewModel : PageViewModel<GrobarhetPage>
    {
        public GrobarhetPageViewModel(GrobarhetPage page) : base(page)
        {
        }

        public GrobarhetResponse[] SearchItems { get; set; }
    }
}
