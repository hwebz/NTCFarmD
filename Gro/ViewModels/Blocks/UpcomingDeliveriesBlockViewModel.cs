using System.Collections.Generic;
using EPiServer.Core;

namespace Gro.ViewModels.Blocks
{
    public class UpcomingDeliveriesBlockViewModel 
    {
        public UpcomingDeliveriesBlockViewModel() 
        {
            //TODO: need to be specified later.
            DeliveryPlanPage = ContentReference.EmptyReference;
            DeliveriesFromCustomer = new List<DeliveryCard>();
            DeliveriesToCustomer = new List<DeliveryCard>();
        }

        public List<DeliveryCard> DeliveriesFromCustomer { get; set; }
        public List<DeliveryCard> DeliveriesToCustomer { get; set; }
        public string LastUpdated { get; set; }

        /// <summary>
        /// Has not exist yet. need to be specify
        /// </summary>
        public ContentReference DeliveryPlanPage { get; set; }
    }

    public class DeliveryCard
    {
        public string CategoryIconUrl { get; set; }
        public string PlannedDeliveryDate { get; set; }
        public string ItemName { get; set; }
        public string OrderNo { get; set; }

        public int OrderLine { get; set; }
        public double OrderQuantity { get; set; }

        public string Container { get; set; }
        public string Warehouse { get; set; }

        public bool IsFromCustomer { get; set; }
        public bool DeliveryDatePassed { get; set; }

        public bool HasDelayedToReceive { get; set; }
    }
}
