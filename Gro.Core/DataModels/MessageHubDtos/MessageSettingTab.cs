using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MessageHubDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class MessageSettingTab
    {

        [DataMember]
        public Category[] Category { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public int DisplayOrder { get; set; }

    }
}
