using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks.BlockWidths;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Blocks
{
    [ContentType(GUID = "D2283D7A-4DB6-4F05-8235-28DF34B2A624")]
    public class InformationBlock : BlockData, IOneColumnContainer, IOneColumnWidth
    {
        [Display(GroupName = BlockGroupNames.BlockContent, Order = 10)]
        [CultureSpecific]
        [Required]
        public virtual string Heading { get; set; }

        [Display(GroupName = BlockGroupNames.BlockContent, Order = 20)]
        [CultureSpecific]
        public virtual XhtmlString Body { get; set; }


    }
}
