using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages
{
    [ContentType(DisplayName = "Contact Form Page", GUID = "A9AFE9AB-0506-4165-9435-A27E67222B1D")]
    [AvailableContentTypes(Availability.None)]
    public class ContactFormPage : SitePageBase
    {
        [CultureSpecific]
        public virtual XhtmlString InstructionWithLogin { get; set; }

        [CultureSpecific]
        public virtual XhtmlString InstructionWithoutLogin { get; set; }

        [AllowedTypes(typeof(ContactBlock), typeof(PushBlock))]
        public virtual ContentArea RightContent { get; set; }

        [CultureSpecific]
        public virtual string TextLink { get; set; }
    }
}
