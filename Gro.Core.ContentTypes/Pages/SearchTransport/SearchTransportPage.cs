using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.SearchTransport
{
    [ContentType(DisplayName = "Search Transport Page", GUID = "dd2a58af-4581-42ae-a69e-d2eef2637a76", Description = "")]
    public class SearchTransportPage : SitePageBase
    {
        [Display(Order = 30, Name = "Right Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(PushBlock) })]
        public virtual ContentArea RightContent { get; set; }
    }
}