using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.AppPages
{
    [ContentType(DisplayName = "Delivery Note Page", GUID = "C6B11503-C7F0-4BAD-8B0B-41693112BFB6", Description = "")]
    [AvailableContentTypes(Availability.None)]
    public class DeliveryNotePage : AppPageBase, ITwoColumnContainer
    {
        [Display(Order = 20, Name = "Preample", GroupName = SystemTabNames.Content)]
        public virtual XhtmlString Preample { get; set; }

        [Display(Order = 30, Name = "Item count", GroupName = SystemTabNames.Content)]
        [Range(1, int.MaxValue)]
        public virtual int ItemCount { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            ItemCount = 0;
        }
    }
}
