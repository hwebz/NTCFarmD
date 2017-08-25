using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Security
{
    [DataContract(Name = "SecurityExceptionInfo", Namespace = "http://schemas.datacontract.org/2004/07/SecurityServiceInt")]
    public class SecurityExceptionInfo
    {
        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public string Message { get; set; }

    }
}
