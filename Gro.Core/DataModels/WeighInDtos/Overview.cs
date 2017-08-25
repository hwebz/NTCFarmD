using System.Runtime.Serialization;

namespace Gro.Core.DataModels.WeighInDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/WeighInServiceInt")]
    public class Overview
    {

        [DataMember]
        public string Agreement { get; set; }

        [DataMember]
        public string Item { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public double WeightIn { get; set; }
    }
}
