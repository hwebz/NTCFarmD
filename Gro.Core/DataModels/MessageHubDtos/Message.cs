using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MessageHubDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class Message
    {

        [DataMember]
        public string AreaDescription { get; set; }

        [DataMember]
        public string CustomerAddress { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string CustomerZipAndCity { get; set; }

        [DataMember]
        public bool Handled { get; set; }

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
        public string TypeDescription { get; set; }

        [DataMember]
        public DateTime ValidFrom { get; set; }

        [DataMember]
        public DateTime ValidTo { get; set; }

	    [DataMember]
        public DateTime? SendDate { get; set; }
    }
}
