using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.BokaPages
{
    [ContentType(DisplayName = "Boka Page", GUID = "417bbea8-a2f0-4c46-b4df-5c7a2d4d8bf9", Description = "")]
    public class BokaPage : SitePageBase
    {
        [Display(Order = 10, Name = "Right Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(PushBlock) })]
        public virtual ContentArea RightContent { get; set; }
        [Display(Order = 20, GroupName = SystemTabNames.Content)]
        public virtual XhtmlString Introduction { get; set; }
    }
}