using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.WeighInDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/WeighInServiceInt")]
    public class WeighIn
    {
        [DataMember]
        public int DeliveryNumber { get; set; }

        [DataMember]
        public string Sort { get; set; }

        [DataMember]
        public int Weight { get; set; }

        [DataMember]
        public DateTime Date { get; set; }
    }
}
