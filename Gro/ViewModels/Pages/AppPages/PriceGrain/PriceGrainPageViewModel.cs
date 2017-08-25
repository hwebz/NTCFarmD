using System.Collections.Generic;
using Gro.Core.ContentTypes.Pages.AppPages.PurchasingAgreement;
using Gro.Core.DataModels.PurchasingAgreements;

namespace Gro.ViewModels.Pages.AppPages.PriceGrain
{
    public class PriceGrainPageViewModel : PageViewModel<PriceGrainPage>
    {
        public PriceGrainPageViewModel(PriceGrainPage page) : base(page)
        {
            PriceAreaList = new List<PriceArea>();
            PricePeriodFirst = new List<PricePeriod>();
        }
        public List<PricePeriod> PricePeriodFirst { get; set; }
        public List<PriceArea> PriceAreaList { get; set; }
    }
}