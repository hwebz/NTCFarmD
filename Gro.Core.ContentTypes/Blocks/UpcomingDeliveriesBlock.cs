using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks.BlockWidths;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;

namespace Gro.Core.ContentTypes.Blocks
{
    [ContentType(DisplayName = "Upcoming Deliveries Block", GUID = "CE6BF174-40E8-47F7-8445-E64298333FF5",
        Description = "Upcoming Deliveries block")]
    public class UpcomingDeliveriesBlock : BlockData, ITwoColumnContainer, ITwoColumnWidth
    {
        [Display(Order = 20, Name = "Maximum Delivery Items", GroupName = SystemTabNames.Settings)]
        [CultureSpecific]
        [Range(1, int.MaxValue)]
        public virtual int MaximumOfDeliveyItems { get; set; }
    }
}
