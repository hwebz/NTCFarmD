using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MessageHubDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class MsgType
    {

        [DataMember]
        public bool MailChecked { get; set; }

        [DataMember()]
        public int MailId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool SmsChecked { get; set; }

        [DataMember]
        public int SmsId { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "showMail")]
        public bool ShowMail { get; set; }

        [DataMember(Name = "showSMS")]
        public bool ShowSms { get; set; }
    }
}
