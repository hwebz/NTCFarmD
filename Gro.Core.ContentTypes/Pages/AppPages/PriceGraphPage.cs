using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.AppPages
{
    [ContentType(DisplayName = "PriceGraphPage", GUID = "ee8f8cf3-9a52-4e3d-a963-8d810e4c4c01", Description = "")]
    public class PriceGraphPage : AppPageBase
    {
        public virtual string GraphTitle { get; set; }
    }
}
