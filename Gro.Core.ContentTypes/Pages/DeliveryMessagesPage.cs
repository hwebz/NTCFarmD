using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages
{
    [ContentType(DisplayName = "Delivery Messages Page", GUID = "20958378-37F0-4C7D-BC66-180EB5F5B2B9")]
    [AvailableContentTypes(Availability.None)]
    public class DeliveryMessagesPage : SitePageBase
    {
    }
}