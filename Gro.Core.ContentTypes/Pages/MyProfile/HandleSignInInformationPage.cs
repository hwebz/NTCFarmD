using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages.MyProfile
{
    [ContentType(DisplayName = "Handle SignIn Information Page", GUID = "29D4D148-981E-4649-BCF0-AD80860FC0CF")]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-settings.png")]
    public class HandleSignInInformationPage : NonServicePageBase, IAccountSettingPage
    {
    }
}
