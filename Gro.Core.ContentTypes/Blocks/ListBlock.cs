using System.ComponentModel.DataAnnotations;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using Gro.Core.ContentTypes.Blocks.BlockWidths;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Utils;
using EPiServer.Core;

namespace Gro.Core.ContentTypes.Blocks
{
    [ContentType(DisplayName = "List Block", GUID = "8D730724-3C1E-49C0-9CCF-409AD1E66D8E", Description = "Storing a list of items")]
    public class ListBlock : BlockData, IOneColumnContainer, IOneColumnWidth
    {
        [Display(GroupName = BlockGroupNames.BlockContent, Order = 10)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        [Display(GroupName = BlockGroupNames.BlockSetting, Order = 20)]
        [CultureSpecific]
        public virtual LinkItemCollection Items { get; set; }
    }
}
