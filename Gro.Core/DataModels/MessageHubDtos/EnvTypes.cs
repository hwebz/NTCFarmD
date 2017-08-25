using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MessageHubDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class EnvTypes
    {
        [DataMember]
        public string EnvTypeDescription { get; set; }

        [DataMember]
        public int EnvTypesId { get; set; }
    }
}
