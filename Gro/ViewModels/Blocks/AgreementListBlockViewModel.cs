using Gro.Core.ContentTypes.Blocks;
using Gro.Core.DataModels.Grain;
using System.Collections.Generic;

namespace Gro.ViewModels.Blocks
{
    public class AgreementListBlockViewModel
    {
        public AgreementListBlock CurrentBlock { get; set; }

        public List<AgreementsDeliverys> ListAgreementsDeliverys { get; set; }
    }
}
