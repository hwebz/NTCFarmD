using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Pages.BasePages;
using System.ComponentModel.DataAnnotations;

namespace Gro.Core.ContentTypes.Pages.AppPages.PurchasingAgreement
{
    [ContentType(DisplayName = "Price Alert Page", GUID = "59977002-a982-4ef5-bbb9-1df18bbda0b0", Description = "")]
    public class PriceAlertPage : SitePageBase, ITwoColumnContainer
    {
        [Display(Order = 10, Name = "Right Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(PushBlock) })]
        public virtual ContentArea RightContent { get; set; }
    }
}