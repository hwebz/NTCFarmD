using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.Organization
{
    [ContentType(DisplayName = "Handle Delivery Addresses Page", GUID = "DB3B6CB9-90E2-42D4-81A6-8D9EC034CA00", Description = "")]
    public class HandleDeliveryAddressPage : NonServicePageBase
    {
        [UIHint(UIHint.LongString)]
        [CultureSpecific]
        public virtual string InstructionForEditting { get; set; }
    }
}
