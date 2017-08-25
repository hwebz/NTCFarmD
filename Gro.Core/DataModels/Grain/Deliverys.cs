using System;
using System.Runtime.Serialization;
namespace Gro.Core.DataModels.Grain
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/GrainService.Models")]
    public class Deliverys : AgreementsDeliverysDto
    {
        [DataMember]
        public DateTime DeliveredDate { get; set; }

        [DataMember]
        public string ItemDescription { get; set; }

        [DataMember]
        public string OrderNr { get; set; }
    }
}
