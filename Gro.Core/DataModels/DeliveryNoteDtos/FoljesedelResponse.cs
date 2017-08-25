using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.DeliveryNoteDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/FoljesedelServiceInt.Models")]
    public class FoljesedelResponse
    {
        [DataMember]
        public bool ActivatedPDF { get; set; }

        [DataMember]
        public string ArticleNumber { get; set; }

        [DataMember]
        public string CustomerNumber { get; set; }

        [DataMember]
        public double DeliveredQuantity { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string FabricDescription { get; set; }

        [DataMember]
        public int FabricID { get; set; }

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public DateTime LoadingDate { get; set; }

        [DataMember]
        public int Lopnummer { get; set; }

        [DataMember]
        public string OrderNumber { get; set; }

        [DataMember]
        public double OrderedQuantity { get; set; }

        [DataMember]
        public int RowNumber { get; set; }

        [DataMember]
        public int System { get; set; }

        public string DisplayOrderNumber => System == 3 ? OrderNumber.Substring(0, OrderNumber.Length - 3) : OrderNumber;
    }
}