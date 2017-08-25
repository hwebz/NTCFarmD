using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MessageHubDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class MessageSettings
    {

        [DataMember]
        public int CustomerOrgId { get; set; }

        [DataMember]
        public int MessageSettingsId { get; set; }

        [DataMember]
        public int ModeOfDeliveryId { get; set; }

        [DataMember]
        public int MsgAreaId { get; set; }

        [DataMember]
        public int MsgTypeId { get; set; }

        [DataMember(Name = "UserID")]
        public int UserId { get; set; }

        [DataMember]
        public bool Value { get; set; }

    }
}
