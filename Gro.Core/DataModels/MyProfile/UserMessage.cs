using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MyProfile
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class UserMessage
    {
        [DataMember]
        public int MessageId { get; set; }

        [DataMember]
        public bool MessageRead { get; set; }

        [DataMember]
        public int MessageType { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int UserMessageId { get; set; }
    }
}
