using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages
{
    [ContentType(DisplayName = "Information Page", GUID = "8348557F-F8EA-45B9-8C21-63490A253681")]
    [AvailableContentTypes(Availability.None)]
    public class InformationPage : SitePageBase
    {
        [Display(Order = 20, Name = "Preamble", GroupName = SystemTabNames.Content)]
        [UIHint(UIHint.LongString)]
        public virtual string Preamble { get; set; }

        [Display(Order = 30, Name = "Body", GroupName = SystemTabNames.Content)]
        public virtual XhtmlString Body { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            VisibleInMenu = false;
        }
    }
}
