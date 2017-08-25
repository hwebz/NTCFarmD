using System.Collections.Generic;
using Gro.Core.ContentTypes.Pages.AppPages;

namespace Gro.ViewModels.Pages.AppPages.WeighIn
{
    public class WeighInPageViewModel : PageViewModel<WeighInPage>
    {
        public WeighInPageViewModel(WeighInPage page) : base(page)
        {
        }

        public IEnumerable<Core.DataModels.WeighInDtos.WeighIn> WeighIns { get; set; }

        public IEnumerable<AgreementSumViewModel> AgreementSums { get; set; }

        public IEnumerable<CustomerSelectViewModel> CustomerSelection { get; set; }

        public int CurrentYear { get; set; }
    }
}
