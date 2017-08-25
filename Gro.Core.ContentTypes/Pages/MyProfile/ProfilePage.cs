using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages.MyProfile
{
    [ContentType(DisplayName = "Edit Profile Page", GUID = "5A9D82DD-6CCB-4F91-8FE5-498EA263AC7C")]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-settings.png")]
    public class ProfilePage : NonServicePageBase, IAccountSettingPage
    {
    }
}
