using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Boka
{
    [DataContract(Namespace = "http://lantmannen.com/centralen/")]
    public class BookingOrder
    {
        [DataMember(IsRequired = true)]
        public int Linenumber { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 1)]
        public string ItemNumber { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string Item { get; set; }
        [DataMember(IsRequired = true, Order = 3)]
        public bool Torkat { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 4)]
        public string Sort { get; set; }
        [DataMember(IsRequired = true, Order = 5)]
        public bool DeliveryAssuranceConfirmed { get; set; }
        [DataMember(IsRequired = true, Order = 6)]
        public int Status { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 7)]
        public string Supplier { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 8)]
        public string Warehouse { get; set; }
        [DataMember(IsRequired = true, Order = 9)]
        public DateTime DeliveryDate { get; set; }
        [DataMember(IsRequired = true, Order = 10)]
        public int Quantity { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 11)]
        public string DeliveryMode { get; set; }
    }
}
