using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Pages.MachinePages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages.StartPages
{
    [ContentType(DisplayName = "Maskin Start Page", GUID = "{82C22F2F-808C-447B-9E14-42781E671F98}",
        AvailableInEditMode = true)]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-02.png")]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] {typeof(InformationArchivePage), typeof(IFramePage), typeof(ArticlePage), typeof(FolderPage), typeof(MachineDetailPage), typeof(MachineAddPage)})]
    public class MaskinStartPage : ServiceStartPage
    {
        [Display(Name = "Machine Detail Page", GroupName = SystemTabNames.Content, Order = 30)]
        [CultureSpecific]
        [AllowedTypes(typeof(MachineDetailPage))]
        public virtual ContentReference MachineDetailPage { get; set; }

        [Display(Name = "Add Machine Page", GroupName = SystemTabNames.Content, Order = 40)]
        [CultureSpecific]
        [AllowedTypes(typeof(MachineAddPage))]
        public virtual ContentReference AddMachinePage { get; set; }

        [Display(Order = 50, Name = "Banner Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(typeof(IOneColumnContainer))]
        public virtual ContentArea BannerContent { get; set; }
    }
}
