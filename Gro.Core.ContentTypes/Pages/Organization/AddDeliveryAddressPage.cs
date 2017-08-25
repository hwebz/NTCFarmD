using System.ComponentModel.DataAnnotations;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.Organization
{
    [ContentType(DisplayName = "Add Delivery Address Page", GUID = "5A848BFF-5EAE-46E2-8D39-856C67F678D6", Description = "")]
    public class AddDeliveryAddressPage : NonServicePageBase
    {
        [UIHint(UIHint.LongString)]
        [CultureSpecific]
        public virtual string Instruction { get; set; }
    }
}

