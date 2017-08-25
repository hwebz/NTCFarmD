using System.Runtime.Serialization;

namespace Gro.Core.DataModels.WeighInDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/WeighInServiceInt")]
    public class WeighInExtended
    {
        [DataMember]
        public int DeliveryNumber { get; set; }

        [DataMember]
        public string Agreement { get; set; }

        [DataMember]
        public double Quantity { get; set; }

        [DataMember]
        public string ReceptionPoint { get; set; }

        [DataMember]
        public bool FarmCollection { get; set; }

        [DataMember]
        public string CarNr { get; set; }
    }
}
