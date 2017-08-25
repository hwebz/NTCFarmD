using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.DeliveryAssuranceDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DeliveryAssuranceServiceInt.Models")]
    public class DeliveryAssurance : DeliveryAssuranceList
    {
        [DataMember]
        public long ATNR { get; set; }

        [DataMember]
        public string Buyer { get; set; }

        [DataMember]
        public string DeliveryAddress { get; set; }

        [DataMember]
        public string Depaavtal { get; set; }

        [DataMember]
        public string ExistingWarehouse { get; set; }

        [DataMember]
        public string GHGvarde { get; set; }

        [DataMember]
        public string Glyfosat { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public string KundorderNr { get; set; }

        [DataMember]
        public string KundsOrderNr { get; set; }

        [DataMember]
        public DateTime Leveransdatum { get; set; }

        [DataMember]
        public string Leveransvillkor { get; set; }

        [DataMember]
        public string Levsatt { get; set; }

        [DataMember]
        public string[] NumbersToUpdate { get; set; }

        [DataMember]
        public string OrderTyp { get; set; }

        [DataMember]
        public string Ovrigt { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public string RED { get; set; }

        [DataMember]
        public DateTime RequestDate { get; set; }

        [DataMember]
        public string SLAM { get; set; }

        [DataMember]
        public string Skordear { get; set; }

        [DataMember]
        public string Sort { get; set; }

        [DataMember]
        public string SortName { get; set; }

        [DataMember]
        public string Straforkortat { get; set; }

        [DataMember]
        public string SupplierNumber { get; set; }

        [DataMember]
        public string Torkat { get; set; }

        [DataMember]
        public string Warehouse { get; set; }
    }
}