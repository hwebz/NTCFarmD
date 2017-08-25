using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using System.ComponentModel.DataAnnotations;

namespace Gro.Core.ContentTypes.Pages.StartPages
{
    public abstract class TwoColumnServiceStartPage : ServiceStartPage
    {
        [Display(Order = 20, Name = "Left Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(ITwoColumnContainer) })]
        public virtual ContentArea LeftContent { get; set; }


        [Display(Order = 30, Name = "Right Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(IOneColumnContainer) })]
        public virtual ContentArea RightContent { get; set; }
    }
}
