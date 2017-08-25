using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.ContentTypes.Pages.Registration;

namespace Gro.Core.ContentTypes.Pages.StartPages
{
    [ContentType(DisplayName = "Start Page", GUID = "{BF46BAD9-8803-4B52-949C-F54D447E4373}", AvailableInEditMode = true)]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-02.png")]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] { typeof(InformationArchivePage), typeof(IFramePage), typeof(ServiceStartPage), typeof(ArticlePage), typeof(FolderPage),
            typeof(AppPageBase), typeof(RegistrationHomePage) })]
    public class StartPage : SitePageBase
    {
        [Display(Order = 100, GroupName = GroupNames.SiteSettings)]
        [CultureSpecific]
        [Required]
        [AllowedTypes(typeof(SettingsPage))]
        public virtual PageReference SettingsPage { get; set; }

        [Display(Order = 20, Name = "Left Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(ITwoColumnContainer) })]
        [CultureSpecific]
        public virtual ContentArea LeftContent { get; set; }

        [Display(Order = 20, Name = "Right Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(IOneColumnContainer) })]
        [CultureSpecific]
        public virtual ContentArea RightContent { get; set; }
    }
}
