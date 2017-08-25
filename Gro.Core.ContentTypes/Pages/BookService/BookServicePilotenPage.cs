using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;

namespace Gro.Core.ContentTypes.Pages.BookService
{
    [ContentType(DisplayName = "Book Service PILOTEN Page", GUID = "39f11837-4a27-493c-bb66-69014442fca8", Description = "")]
    public class BookServicePilotenPage : SitePageBase
    {
        [Display(Order = 20, Name = "Banner Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(typeof(IOneColumnContainer))]
        public virtual ContentArea BannerContent { get; set; }
    }
}
