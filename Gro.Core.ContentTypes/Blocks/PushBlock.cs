using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Gro.Core.ContentTypes.Blocks.BlockWidths;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Business.UIDescriptors;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.ContentTypes.Business;
using EPiServer.SpecializedProperties;

namespace Gro.Core.ContentTypes.Blocks
{
    [ContentType(DisplayName = "Push Block", GUID = "AFEF3D1C-1B28-4A86-845F-EEEA2404D7FE", Description = "Block with image  and link")]
    public class PushBlock : BlockData, IOneColumnContainer, IOneColumnWidth, ITwoColumnWidth, ITwoColumnContainer
    {
        [Display(Name = "Image", GroupName = BlockGroupNames.BlockContent, Order = 10)]
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }

        [Display(Name = "Link", GroupName = BlockGroupNames.BlockSetting, Order = 20)]
        [ScaffoldColumn(false)]
        public virtual Url Link { get; set; }

        [Display(Name = "TextLink", GroupName = BlockGroupNames.BlockSetting, Order = 30)]
        [ScaffoldColumn(false)]
        public virtual string TextLink { get; set; }

        [Display(Name = "BlockLink", GroupName = BlockGroupNames.BlockContent, Order = 31)]
        [LinkItemCollectionLimit(Max = 1, Min = 0)]
        public virtual LinkItemCollection BlockLink { get; set; }

        [Display(Name = "Color", GroupName = BlockGroupNames.BlockSetting, Order = 40)]
        [SelectOne(SelectionFactoryType = typeof(PushBlockColorSelectionFactory))]
        [CultureSpecific]
        public virtual string Color { get; set; }
    }
}
