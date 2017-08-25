using System.Runtime.Serialization;

namespace Gro.Core.DataModels.SearchTransport
{
    [DataContract(Namespace = "http://lantmannenlantbruk.se/1.0.0.0/")]
    public class ShipmentResponse
    {
        [DataMember]
        public string Carrier { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public System.DateTime? PlanedDepartureDate { get; set; }

        [DataMember]
        public int ShipmentId { get; set; }

        [DataMember]
        public int ShipmentIdMX { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}
