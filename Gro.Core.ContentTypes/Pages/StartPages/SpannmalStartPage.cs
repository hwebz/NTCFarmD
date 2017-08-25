using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Utils;
using System.ComponentModel.DataAnnotations;

namespace Gro.Core.ContentTypes.Pages.StartPages
{
    [ContentType(DisplayName = "Spannmal Start Page", GUID = "{DC9E55A9-A337-410F-B039-6A4038E09AFE}",
        AvailableInEditMode = true)]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-02.png")]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] { typeof(InformationArchivePage), typeof(IFramePage), typeof(ArticlePage), typeof(FolderPage) })]
    public class SpannmalStartPage : TwoColumnServiceStartPage
    {
        [Display(Order = 21, Name = "Left sub Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(IOneColumnContainer) })]
        public virtual ContentArea LeftSubColumn { get; set; }

        [Display(Order = 22, Name = "Right sub Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(IOneColumnContainer) })]
        public virtual ContentArea RightSubColumn { get; set; }
    }
}
