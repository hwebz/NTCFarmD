using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.AppPages
{
    [ContentType(DisplayName = "WeighIn Page", GUID = "7C4C3074-794C-4ACB-B10C-19F34DE62C95", Description = "")]
    [AvailableContentTypes(Availability.None)]
    public class WeighInPage : AppPageBase
    {
        public virtual string OverViewDescription { get; set; }
    }
}
