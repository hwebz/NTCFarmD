using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks.BlockWidths;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Blocks
{
    [ContentType(DisplayName = "Information List Block", GUID = "ad76da6a-abe8-4ba1-b379-172bf1e32089", Description = "Information List Block")]
    public class InformationListBlock : BlockData, ITwoColumnContainer, IOneColumnContainer, ITwoColumnWidth, IOneColumnWidth
    {
        [Display(GroupName = BlockGroupNames.BlockContent, Order = 10)]
        [CultureSpecific]
        public virtual string Headline { get; set; }

        [Display(GroupName = BlockGroupNames.BlockSetting, Order = 20)]
        [CultureSpecific]
        [Range(0, int.MaxValue)]
        public virtual int NumberOfItems { get; set; }

        [Display(GroupName = BlockGroupNames.BlockSetting, Order = 30)]
        [CultureSpecific]
        [AllowedTypes(typeof(InformationArchivePage))]
        public virtual ContentReference ArchivePage { get; set; }

        [Display(GroupName = BlockGroupNames.BlockSetting, Order = 40)]
        [CultureSpecific]
        public virtual string ArchiveLinkText { get; set; }
    }
}
