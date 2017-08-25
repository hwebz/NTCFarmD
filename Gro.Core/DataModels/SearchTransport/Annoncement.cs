using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.SearchTransport
{
    [DataContract(Namespace = "http://lantmannenlantbruk.se/1.0.0.0/")]
    public class Annoncement
    {
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Recipient { get; set; }
        [DataMember]
        public bool SentBySMS { get; set; }
        [DataMember]
        public DateTime? SentTime { get; set; }
        [DataMember]
        public string Type { get; set; }
    }
}
