using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages
{
    [ContentType(DisplayName = "Information Archive Page", GUID = "59DDCC7A-34E5-493B-A0E4-5D4515344CD4")]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] { typeof(InformationPage), typeof(IFramePage) })]
    public class InformationArchivePage : SitePageBase
    {
        [Display(GroupName = SystemTabNames.Content, Order = 50)]
        [CultureSpecific]
        public virtual string LatestPagesHeadline { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 60)]
        [CultureSpecific]
        public virtual string LatestPagesSeemoreText { get; set; }
    }
}