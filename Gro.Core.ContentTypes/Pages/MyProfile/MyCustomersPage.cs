using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages.MyProfile
{
    [ContentType(DisplayName = "My Customer Page", GUID = "A5FBED1D-67A0-4A89-8D0E-C37DC0DE1FEB")]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-settings.png")]
    public class MyCustomersPage : NonServicePageBase, IAccountSettingPage
    {

    }
}
