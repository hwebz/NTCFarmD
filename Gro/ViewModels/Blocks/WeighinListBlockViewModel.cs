using Gro.Core.ContentTypes.Blocks;
using Gro.Core.DataModels.Grain;
using System.Collections.Generic;

namespace Gro.ViewModels.Blocks
{
    public class WeighinListBlockViewModel
    {
        public WeighinListBlock CurrentBlock { get; set; }

        public List<Deliverys> Deliveries { get; set; }
    }
}
