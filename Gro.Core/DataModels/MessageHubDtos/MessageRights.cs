using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MessageHubDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class MessageRights
    {

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public int MessageRightsId { get; set; }

        [DataMember]
        public int MessageTypeId { get; set; }

        [DataMember]
        public int UserId { get; set; }
    }
}
