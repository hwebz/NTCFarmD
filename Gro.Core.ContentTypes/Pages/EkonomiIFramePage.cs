using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;
using System.ComponentModel;

namespace Gro.Core.ContentTypes.Pages
{
    [ContentType(GUID = "43a4e751-9079-45dd-bbdb-a5c8562338ed", AvailableInEditMode = true,
        DisplayName = "Ekonomi IFrame Page")]
    public class EkonomiIFramePage : SitePageBase
    {
        [Display(GroupName = SystemTabNames.Content, Order = 310, Name = "IFrame url")]
        public virtual string IFrameUrl { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 330, Name = "Enable navigation")]
        [DefaultValue(false)]
        public virtual bool EnableNavigation { get; set; }
    }
}