using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages
{
    [ContentType(DisplayName = "Contact Page", GUID = "1B9F54E3-77EA-4AA8-9F45-4BB0EFA07817")]
    public class ContactPage : SitePageBase
    {
        [Display(Order = 15, GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        [Required]
        public virtual string Gatuadress { get; set; }

        [Display(Order = 20, GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        [Required]
        public virtual string Postadress { get; set; }

        [Display(Order = 30, GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        public virtual string Telephone { get; set; }

        [Display(Order = 40, GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        [UIHint(UIHint.LongString)]
        public virtual string OpenHours { get; set; }

        [Display(Order = 50, GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        public virtual Url Email { get; set; }

        [Display(Order = 60, GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        public virtual LinkItemCollection Website { get; set; }

        [Display(Order = 70, GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        [UIHint(UIHint.LongString)]
        public virtual XhtmlString BillingAddress { get; set; }

        [Display(Order = 80, GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        public virtual string OrganisationNumber { get; set; }

        [Display(Order = 90, GroupName = SystemTabNames.Content)]
        public virtual XhtmlString Introduction { get; set; }

        [Display(Order = 100, GroupName = SystemTabNames.Content)]
        public virtual ContentArea RightContent { get; set; }
    }
}
