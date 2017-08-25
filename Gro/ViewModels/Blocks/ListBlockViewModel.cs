using System.Collections.Generic;
using Gro.Business;
using Gro.Core.ContentTypes.Blocks;

namespace Gro.ViewModels.Blocks
{
    public class ListBlockViewModel
    {
        public ListBlockViewModel( ListBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }
        public ListBlock CurrentBlock { get; set; }

        public IEnumerable<GroLinkItem> LinkItems { get; set; }
    }
}