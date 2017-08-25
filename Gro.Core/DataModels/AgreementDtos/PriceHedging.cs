using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.AgreementDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/AgreementServiceInt.Models")]
    public class PriceHedging
    {
        [DataMember]
        public string InputPriceHedging { get; set; } //Leveransadress

        [DataMember]
        public DateTime  AgreementDate { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public int Price { get; set; }

    }
}
