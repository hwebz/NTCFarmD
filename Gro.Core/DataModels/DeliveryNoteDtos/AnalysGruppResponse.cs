using System.Runtime.Serialization;

namespace Gro.Core.DataModels.DeliveryNoteDtos
{
    [DataContract(Name = "AnalysGruppResponse",
        Namespace = "http://schemas.datacontract.org/2004/07/FoljesedelServiceInt.Models")]
    public class AnalysGruppResponse
    {
        [DataMember]
        public short Grupp { get; set; }

        [DataMember]
        public short? KolDelning { get; set; }

        [DataMember]
        public string NamnVisa { get; set; }
    }
}
