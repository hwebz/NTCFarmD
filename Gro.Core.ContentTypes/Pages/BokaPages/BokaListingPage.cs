using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.BokaPages
{
    [ContentType(DisplayName = "Boka Listing Page", GUID = "d5e7dcce-be68-499e-88fc-4dc80002cdd6", Description = "")]
    public class BokaListingPage : SitePageBase
    {
        [Display(Order = 10, Name = "Right Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(PushBlock) })]
        public virtual ContentArea RightContent { get; set; }
    }
}