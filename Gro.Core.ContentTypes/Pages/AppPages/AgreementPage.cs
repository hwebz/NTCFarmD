using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.AppPages
{
    [ContentType(DisplayName = "Agreement Page", GUID = "6C29BDC2-652B-41E2-8E71-3BD835AFD4AC", Description = "")]
    [AvailableContentTypes(Availability.None)]
    public class AgreementPage : AppPageBase
    {
        public virtual string InterestTitle { get; set; }
    }
}
