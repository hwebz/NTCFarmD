using System.Collections.Generic;
using Gro.Core.ContentTypes.Pages.AppPages;
using Gro.Core.DataModels.PriceGraph;

namespace Gro.ViewModels.Pages.AppPages.PriceGraph
{
    public class PriceGraphPageViewModel : PageViewModel<PriceGraphPage>
    {
        public PriceGraphPageViewModel(PriceGraphPage page) : base(page)
        {
        }

        public PriceGraphDisplay GraphDisplay { get; set; }

        public Dictionary<string, int> PeriodOptions { get; set; }
    }
}
