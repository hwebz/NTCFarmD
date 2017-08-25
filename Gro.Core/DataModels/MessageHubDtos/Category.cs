using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MessageHubDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class Category
    {
        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public int Categoryid { get; set; }

        [DataMember]
        public MsgType[] MessageType { get; set; }
    }
}
