using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks.BlockWidths;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Blocks
{
    [ContentType(DisplayName = "News List Block", GUID = "918CFAA9-DE38-42CB-B3A2-18A47F21C555", Description = "Listing the latest news")]
    public class NewsListBlock : BlockData, ITwoColumnContainer, IOneColumnContainer, ITwoColumnWidth, IOneColumnWidth
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
