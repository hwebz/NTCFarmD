using System.Collections.Generic;
using System.Linq;
using Gro.Core.ContentTypes.Pages.AppPages;
using Gro.Core.DataModels.AgreementDtos;

namespace Gro.ViewModels.Pages.AppPages.Agreement
{
    public class AgreementPageViewModel : PageViewModel<AgreementPage>
    {
        public AgreementPageViewModel(AgreementPage currentPage) : base(currentPage)
        { }

        public IEnumerable<IGrouping<int, Core.DataModels.AgreementDtos.Agreement>> ListAgreementsByYears { get; set; }

        public IEnumerable<IGrouping<int, SeedAssurance>> ListSeedAgreementsByYears { get; set; }

        public List<DryAgreement> ListDryAgreements { get; set; }

        public IEnumerable<IGrouping<int, Core.DataModels.AgreementDtos.Agreement>> ListFarmingAgreements { get; set; }
    }

    public class AgreementAnalyzeViewModel
    {
        // list ordered analysis.ProvNr
        public IEnumerable<string> AnalysisProvNrs { get; set; }

        public Dictionary<string, List<string>> DictResultByAnalyskod { get; set; }

        public string AgreementNumber { get; set; }
    }
}
