using EPiServer.Core;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages.Messages
{
    [ContentType(GUID = "4320C28E-29FD-4B5C-9C08-EC04D9102196")]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-settings.png")]
    public class AdministerMessagePage : SitePageBase
    {
        public virtual XhtmlString StandardMessageHeader { get; set; }

        public virtual XhtmlString FreeMessageHeader { get; set; }
    }
}
