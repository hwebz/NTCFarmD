using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MessageHubDtos
{
    [DataContract(Name = "ExternalMessage",
        Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class ExternalMessage
    {
        [DataMember]
        public int AddressId { get; set; }

        [DataMember]
        public string CustomerNo { get; set; }

        [DataMember]
        public string HeadLine { get; set; }

        [DataMember]
        public string MailMessage { get; set; }

        [DataMember]
        public int MessageArea { get; set; }

        [DataMember]
        public int MessageType { get; set; }

        [DataMember]
        public string MsgText { get; set; }

        [DataMember]
        public DateTime ValidFrom { get; set; }

        [DataMember]
        public DateTime ValidTo { get; set; }
    }
}
