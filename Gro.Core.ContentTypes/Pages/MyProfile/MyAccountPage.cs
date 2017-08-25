using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages.MyProfile
{
    [ContentType(DisplayName = "My Profile Page", GUID = "FA96C99E-121F-419F-B918-23CDDB3D7034")]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-settings.png")]
    [AvailableContentTypes(Include = new []{ typeof(IAccountSettingPage), typeof(FolderPage)})]
    public class MyAccountPage : SitePageBase
    {
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        [CultureSpecific]
        public virtual LinkItemCollection MyProfileLinks { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        [CultureSpecific]
        public virtual LinkItemCollection MyCompanyLinks { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        [CultureSpecific]
        public virtual LinkItemCollection UserAndOrganizationLinks { get; set; }
    }
}
