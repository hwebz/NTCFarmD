using EPiServer.Core;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.MachinePages
{
    [ContentType(DisplayName = "Add New Machine Page", GUID = "9ba4a58a-453a-4cd2-b385-d1675c81e892", Description = "Add New Machine Page")]
    public class MachineAddPage : SitePageBase
    {
        public virtual XhtmlString Instruction { get; set; }
    }
}
