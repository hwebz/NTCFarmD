using System.Collections.Generic;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.ContentTypes.Pages;

namespace Gro.ViewModels.Blocks
{
    public class NewsListBlockViewModel
    {
        public NewsListBlock CurrentBlock { get; set; }

        public IEnumerable<InformationPage> RecentInformationPages { get; set; }
    }
}
