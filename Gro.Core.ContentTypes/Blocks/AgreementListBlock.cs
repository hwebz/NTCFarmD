using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;

namespace Gro.Core.ContentTypes.Blocks
{
    [ContentType(DisplayName = "Agreement List Block", GUID = "933f1891-600c-43a3-b4b8-8e580807e3b5", Description = "Agreement List Block")]
    public class AgreementListBlock : BlockData, ITwoColumnContainer
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