using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.DeliveryAssuranceDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DeliveryAssuranceServiceInt.Models")]
    public class DeliveryAssuranceList
    {
        [DataMember]
        public int BookedTime { get; set; }

        [DataMember]
        public DateTime DeliveryDate { get; set; }

        [DataMember]
        public bool Gardshamtning { get; set; }

        [DataMember]
        public string IONumber { get; set; }

        [DataMember]
        public string Item { get; set; }

        [DataMember]
        public int LineNumber { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember(Name = "hasBooking")]
        public bool HasBooking { get; set; }
    }
}