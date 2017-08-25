using System;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.DataModels.MessageHubDtos;
using Gro.Core.DataModels.Security;

namespace Gro.ViewModels.Pages.Messages
{
    public class PlannedDeliveryPageViewModel : PageViewModel<DeliveryMessagesPage>
    {
        public PlannedDeliveryPageViewModel(DeliveryMessagesPage currentPage) : base(currentPage)
        {
            PlannedDeliveriesList = new PlannedDeliveries[0];
            LastUpdated=DateTime.Now;
        }

        public PlannedDeliveries[] PlannedDeliveriesList { get; set; }

        public CustomerBasicInfo ActiveCustomer { get; set; }

        public DateTime LastUpdated { get; set; }
    }

    public class PlannedDeliveriesMessageModel
    {
        public PlannedDeliveries[] PlannedDeliveriesList { get; set; }

        public string  CustomerName { get; set; }
    }
}