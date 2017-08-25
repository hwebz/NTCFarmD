using System.Runtime.Serialization;

namespace Gro.Core.DataModels.DeliveryNoteDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/FoljesedelServiceInt.Models")]
    public class FabricItemResponse
    {
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int FabricId { get; set; }
    }
}
