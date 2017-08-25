using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MessageHubDtos
{
    [DataContract(Name = "UserMessageInfo",
        Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class UserMessageInfo
    {
        [DataMember]
        public string HeadLine { get; set; }

        [DataMember]
        public string MailMessage { get; set; }

        [DataMember]
        public int MessageArea { get; set; }

        [DataMember]
        public int MessageId { get; set; }

        [DataMember]
        public bool MessageRead { get; set; }

        [DataMember]
        public int MessageType { get; set; }

        [DataMember]
        public int ModeOfDelivery { get; set; }

        [DataMember]
        public string MsgText { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public DateTime ValidFrom { get; set; }

        [DataMember]
        public DateTime ValidTo { get; set; }
    }
}