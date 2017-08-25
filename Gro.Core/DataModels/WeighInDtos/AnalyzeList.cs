using System.Runtime.Serialization;

namespace Gro.Core.DataModels.WeighInDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/WeighInServiceInt")]
    public class AnalyzeList
    {
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Keyvalue { get; set; }
    }
}
