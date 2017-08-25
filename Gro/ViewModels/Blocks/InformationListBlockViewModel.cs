using System.Collections.Generic;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.ContentTypes.Pages;

namespace Gro.ViewModels.Blocks
{
    public class InformationListBlockViewModel
    {
        public InformationListBlock CurrentBlock { get; set; }
        public IEnumerable<InformationPage> RecentInformationPages { get; set; }
    }
}
