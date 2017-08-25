using EPiServer.Core;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.InternalPages
{
    [ContentType(DisplayName = "Internal Start Page", GUID = "3987ac86-de1b-4ad5-93d8-578d74cb6f01", Description = "")]
    public class InternalStartPage : SitePageBase
    {
        public virtual ContentArea RightArea { get; set; }
        public virtual ContentArea BottomArea { get; set; }
    }
}
