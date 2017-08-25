using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.AppPages
{
    [ContentType(DisplayName = "Calculate Delivery Fee Page", GUID = "69399AB6-12A0-4778-9513-E1EB7DE57BB3", Description = "")]
    [AvailableContentTypes(Availability.None)]
    public class CalculateDeliveryFeePage : AppPageBase
    {
    }
}
