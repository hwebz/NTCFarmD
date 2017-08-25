using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages.MyProfile
{
    [ContentType(DisplayName = "Change Password Page", GUID = "63B6BB63-9AD0-46FA-8ED0-AD06CCD4DCDA")]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-settings.png")]
    public class ChangePasswordPage : NonServicePageBase, IAccountSettingPage
    {
    }
}
