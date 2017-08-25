using Gro.Core.DataModels.AgreementDtos;
using System.Collections.Generic;
using Gro.Core.ContentTypes.Blocks;

namespace Gro.ViewModels.Blocks
{
    public class OpenAgreementsBlockViewModel
    {
        public OpenAgreementsBlockViewModel(OpenAgreementBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }

        public readonly OpenAgreementBlock CurrentBlock;

        public IEnumerable<Agreement> OpenGrainAgreements { get; set; }

        public IEnumerable<SeedAssurance> OpenSeedAgreements { get; set; }
    }
}
