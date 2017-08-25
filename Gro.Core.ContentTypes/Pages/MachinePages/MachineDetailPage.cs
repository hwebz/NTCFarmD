using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.ContentTypes.Pages.BookService;
using EPiServer.Core;

namespace Gro.Core.ContentTypes.Pages.MachinePages
{
    [ContentType(DisplayName = "Machine Detail Page", GUID = "9B43334D-BB8E-412C-901D-63F61050D312",
         Description = "Page show single machine")]
    public class MachineDetailPage : SitePageBase
    {
        [Display(Name = "", GroupName = GroupNames.PushBannerSetting, Order = 20)]
        public virtual PushBlock PushBanner { get; set; }

        [Display(Name = "Book Service Page", GroupName = SystemTabNames.Content, Order = 30)]
        [CultureSpecific]
        [AllowedTypes(typeof(BookServicePilotenPage))]
        public virtual ContentReference BookServicePage { get; set; }
    }
}