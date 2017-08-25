using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.AppPages.PurchasingAgreement
{
    [ContentType(DisplayName = "Grain Price Page", GUID = "ff0036e4-8af9-4147-8f15-97b8db0f40fb", Description = "")]
    public class PriceGrainPage : SitePageBase, ITwoColumnContainer
    {
        [Display(Order = 10, Name = "Right Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(PushBlock) })]
        public virtual ContentArea RightContent { get; set; }
        [Display(Order = 20, Name = "Imformation", GroupName = SystemTabNames.Content)]
        public virtual XhtmlString Information { get; set; } 
    }
}