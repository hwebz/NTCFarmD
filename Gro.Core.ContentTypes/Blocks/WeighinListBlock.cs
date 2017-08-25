using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Blocks
{
    [ContentType(DisplayName = "Weigh-ins List Block", GUID = "7ded830a-1fa3-4d4c-bd76-32fefa996eee", Description = "Weigh-ins List Block")]
    public class WeighinListBlock : BlockData, IOneColumnContainer
    {
        [Display(GroupName = BlockGroupNames.BlockContent, Order = 10)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        [Display(GroupName = BlockGroupNames.BlockSetting, Order = 10)]
        [CultureSpecific]
        public virtual string LinkText { get; set; }

        [Display(GroupName = BlockGroupNames.BlockSetting, Order = 20)]
        [CultureSpecific]
        public virtual ContentReference ArchivePage { get; set; }
    }
}
