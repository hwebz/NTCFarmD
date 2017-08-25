using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.DeliveryNoteDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/FoljesedelServiceInt.Models")]
    public class BulkfoderLeveransResponse
    {
        [DataMember]
        public string Adress1 { get; set; }

        [DataMember]
        public string Adress2 { get; set; }

        [DataMember]
        public string Adress3 { get; set; }

        [DataMember]
        public string Artikel { get; set; }

        [DataMember]
        public string Batchnr { get; set; }

        [DataMember]
        public string Beh { get; set; }

        [DataMember]
        public string Benamning { get; set; }

        [DataMember]
        public string Biltext { get; set; }

        [DataMember]
        public int? Brutto { get; set; }

        [DataMember]
        public string DatumKlar { get; set; }

        [DataMember]
        public string DatumTillv { get; set; }

        [DataMember]
        public DateTime? DatumUts { get; set; }

        [DataMember]
        public string Fabriksnamn { get; set; }

        [DataMember]
        public int? Fabrikssedelsnr { get; set; }

        [DataMember]
        public string Inforad { get; set; }

        [DataMember]
        public string Kula { get; set; }

        [DataMember]
        public string Kundbehallare { get; set; }

        [DataMember]
        public string Kundnr { get; set; }

        [DataMember]
        public string Kundordernummer { get; set; }

        [DataMember]
        public int? Kvantitet { get; set; }

        [DataMember]
        public string Latitud { get; set; }

        [DataMember]
        public string Levadress { get; set; }

        [DataMember]
        public string Levdag { get; set; }

        [DataMember]
        public string Longitud { get; set; }

        [DataMember]
        public double Lopnr { get; set; }

        [DataMember]
        public string Mobiltelefon { get; set; }

        [DataMember]
        public string Namn { get; set; }

        [DataMember]
        public int? Netto { get; set; }

        [DataMember]
        public string Nr { get; set; }

        [DataMember]
        public int? OnskadKvantitet { get; set; }

        [DataMember]
        public short? Rad { get; set; }

        [DataMember]
        public string ReceptNr { get; set; }

        [DataMember]
        public int? ReceptVersion { get; set; }

        [DataMember]
        public string Regnr { get; set; }

        [DataMember]
        public int? Tara { get; set; }

        [DataMember]
        public string Telefon { get; set; }

        [DataMember]
        public string Tillvord { get; set; }

        [DataMember]
        public int? UtlastadKvantitet { get; set; }

        [DataMember]
        public DateTime? Utlastningsdatum { get; set; }

        [DataMember]
        public DateTime? UtlastningsdatumAngiven { get; set; }

        [DataMember]
        public int? Ver { get; set; }
    }
}