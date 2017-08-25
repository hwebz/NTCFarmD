using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Organization
{
    [DataContract(Name = "CustomerRegistration", Namespace = "http://schemas.datacontract.org/2004/07/LM2OrganisationService.Model")]
    public partial class CustomerRegistration
    {

        [DataMember]
        public bool Account_AllowAutogiro { get; set; }

        [DataMember]
        public string Account_ClearingNr { get; set; }

        [DataMember]
        public string Account_Giro { get; set; }

        [DataMember]
        public string Account_Number { get; set; }

        [DataMember]
        public string AdditionalInfo { get; set; }

        [DataMember]
        public byte[] BoardProtocolFile { get; set; }

        [DataMember]
        public string BoardProtocolFileName { get; set; }

        [DataMember]
        public string Contact_Email { get; set; }

        [DataMember]
        public string Contact_Mobile { get; set; }

        [DataMember]
        public string Contact_Phone { get; set; }

        [DataMember]
        public bool CustomerExists { get; set; }

        [DataMember]
        public string CustomerNr { get; set; }

        [DataMember]
        public string Customer_Email { get; set; }

        [DataMember]
        public string Customer_Name { get; set; }

        [DataMember]
        public string Customer_SocialSecurity { get; set; }

        [DataMember]
        public string Customer_VATNr { get; set; }

        [DataMember]
        public string DeliveryAddr_City { get; set; }

        [DataMember]
        public string DeliveryAddr_Directions { get; set; }

        [DataMember]
        public string DeliveryAddr_Mobile { get; set; }

        [DataMember]
        public string DeliveryAddr_Phone { get; set; }

        [DataMember]
        public string DeliveryAddr_Street { get; set; }

        [DataMember]
        public string DeliveryAddr_ZipCode { get; set; }

        [DataMember]
        public byte[] EServiceAgrFile { get; set; }

        [DataMember]
        public string EServiceAgrFileName { get; set; }

        [DataMember]
        public byte[] IdCopyFile { get; set; }

        [DataMember]
        public string IdCopyFileName { get; set; }

        [DataMember]
        public string InvoiceAddr_City { get; set; }

        [DataMember]
        public string InvoiceAddr_Street { get; set; }

        [DataMember]
        public string InvoiceAddr_ZipCode { get; set; }

        [DataMember]
        public bool PrivateCompany { get; set; }

        [DataMember]
        public CustomerProfile Profile { get; set; }

        [DataMember]
        public string Purchase_FarmCredit { get; set; }

        [DataMember]
        public string Purchase_For { get; set; }

        [DataMember]
        public string Purchase_MachineCredit { get; set; }

        [DataMember]
        public byte[] RegistrationFile { get; set; }

        [DataMember]
        public string RegistrationFileName { get; set; }

        [DataMember]
        public string User_Email { get; set; }

        [DataMember]
        public string User_FirstName { get; set; }

        [DataMember]
        public string User_LastName { get; set; }

        [DataMember]
        public string User_Mobile { get; set; }

        [DataMember]
        public string User_Phone { get; set; }

        [DataMember]
        public string User_SocialSecurity { get; set; }
    }
}
