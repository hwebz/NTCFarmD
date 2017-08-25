using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Gro.Core.ContentTypes.Pages.AppPages.PurchasingAgreement;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Pages.SearchTransport;

namespace Gro.Core.ContentTypes.Pages
{
    [ContentType(DisplayName = "ArticlePage", GUID = "6377d72e-23dd-4370-b582-c8959104edd7", Description = "")]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] { typeof(IFramePage), typeof(AppPageBase), typeof(SearchTransportPage), typeof(PurchasingAgreementPage) })]
    public class ArticlePage : SitePageBase
    {
        [Display(Order = 20, Name = "Preamble", GroupName = SystemTabNames.Content)]
        [UIHint(UIHint.LongString)]
        public virtual string Preamble { get; set; }

        [Display(Order = 30, Name = "Body", GroupName = SystemTabNames.Content)]
        public virtual XhtmlString Body { get; set; }
    }
}