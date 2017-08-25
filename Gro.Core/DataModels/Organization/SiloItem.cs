using System.Runtime.Serialization;
namespace Gro.Core.DataModels.Organization
{
    [DataContract(Name = "SiloItem", Namespace = "http://schemas.datacontract.org/2004/07/CustomerSupportServiceInt.Models")]
    public   class SiloItem 
    {

        [DataMember]
        public string Accessibility { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Number { get; set; }
    }
}
