using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.Organization
{
    [ContentType(DisplayName = "Company Information Page", GUID = "E953A2EA-C067-4331-BFE4-820A5A0A8E41", Description = "")]
    public class CompanyInformationPage : NonServicePageBase
    {
        [Display(Order = 20, Name = "Preamble", GroupName = SystemTabNames.Content)]
        [UIHint(UIHint.LongString)]
        public virtual string Preamble { get; set; }
    }
}
