using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.Organization
{
    [ContentType(DisplayName = "Business Profile Page", GUID = "B262BD6C-BE76-44C3-AFA4-407C6309D07E", Description = "")]
    public class BusinessProfilePage : NonServicePageBase
    {
        [Display(Order = 20, Name = "Preamble", GroupName = SystemTabNames.Content)]
        [UIHint(UIHint.LongString)]
        public virtual string Preamble { get; set; }
    }
}
