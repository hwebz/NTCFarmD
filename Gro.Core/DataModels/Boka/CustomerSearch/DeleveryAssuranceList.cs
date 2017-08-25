using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Boka.CustomerSearch
{
    [DataContract(Namespace = "http://lantmannen.com/centralen/")]
    public class DeleveryAssuranceList
    {
        [DataMember(IsRequired = true)]
        public int Quantity { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Warehouse { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string Itemname { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 3)]
        public string IOnumber { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 4)]
        public string Status { get; set; }
        [DataMember(IsRequired = true, Order = 5)]
        public int LineNumber { get; set; }
    }
}
