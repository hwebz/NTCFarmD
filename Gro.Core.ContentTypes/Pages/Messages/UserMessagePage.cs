using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages.Messages
{
    [ContentType(GUID = "18682F6A-3B7C-4CF8-BCB9-8CEF9DEE1B5E")]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-settings.png")]
    public class UserMessagePage : SitePageBase
    {
    }
}
