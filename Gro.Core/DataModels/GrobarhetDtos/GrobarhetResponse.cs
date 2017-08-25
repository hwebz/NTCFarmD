using System.Runtime.Serialization;

namespace Gro.Core.DataModels.GrobarhetDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/GrobarhetServiceInt.Models")]
    public class GrobarhetResponse
    {
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Grobarhet { get; set; }

        [DataMember]
        public string Reference { get; set; }
    }
}
