using System.Runtime.Serialization;

namespace Gro.Core.DataModels.SearchTransport
{
    [DataContract(Namespace = "http://lantmannenlantbruk.se/1.0.0.0/")]
    public class OrderRowResponse
    {
        [DataMember]
        public System.DateTime? Arrived { get; set; }
        [DataMember]
        public string Customer { get; set; }
        [DataMember]
        public string CustomerNo { get; set; }
        [DataMember]
        public System.DateTime? Delivered { get; set; }
        [DataMember]
        public int DeliveredQty { get; set; }
        [DataMember]
        public string DispatcherNumber { get; set; }
        [DataMember]
        public bool IsSchenkerDelivery { get; set; }
        [DataMember]
        public string Item { get; set; }
        [DataMember]
        public string ItemNumber { get; set; }
        [DataMember]
        public int NoOfAnnouncements { get; set; }
        [DataMember]
        public string OrderNumber { get; set; }
        [DataMember]
        public int OrderRow { get; set; }
        [DataMember]
        public int OrderedQty { get; set; }
        [DataMember]
        public System.DateTime? PlannedArrival { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Warehouse { get; set; }
        [DataMember]
        public string WarehouseNumber { get; set; }
        [DataMember]
        public string WaybillNumber { get; set; }

        
    }
}
