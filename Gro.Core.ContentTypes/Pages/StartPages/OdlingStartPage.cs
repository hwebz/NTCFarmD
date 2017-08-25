using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages.StartPages
{
    [ContentType(DisplayName = "Odling Start Page", GUID = "{9F8211E3-2870-4A26-884C-63DFBA0CD0A1}", AvailableInEditMode = true)]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-02.png")]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] { typeof(InformationArchivePage), typeof(IFramePage), typeof(ArticlePage), typeof(FolderPage) })]
    public class OdlingStartPage : TwoColumnServiceStartPage
    {
        [Display(Order = 10, Name = "Left top Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(IOneColumnContainer) })]
        public virtual ContentArea LeftTopColumn { get; set; }

        [Display(Order = 10, Name = "Right top Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(IOneColumnContainer) })]
        public virtual ContentArea RightTopColumn { get; set; }

        [AllowedTypes(RestrictedTypes = new[] { typeof(NewsListBlock) })]
        public override ContentArea LeftContent { get; set; }

        [AllowedTypes(RestrictedTypes = new[] { typeof(NewsListBlock) })]
        public override ContentArea RightContent { get; set; }
    }
}
