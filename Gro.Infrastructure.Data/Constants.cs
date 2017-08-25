namespace Gro.Infrastructure.Data
{
    //Attribute Mapping table Farmday -> ISIM
    public static class IamUserAttributes
    {
        public const string UserName = "uid"; //format email
        public const string Name = "cn"; //format FirstName + ’ ’ + SurName
        public const string FirstName = "givenname";
        public const string Surname = "sn";
        public const string Address = "postaladdress";
        public const string ZipCode = "postalcode";
        public const string City = "l";
        public const string PersonNumber = "personnumber";
        public const string Email = "mail";
        public const string CellPhone = "mobile";
        public const string Telephone = "telephonenumber";
        public const string MinaSidorCunr = "minasidorcunr";
        public const string Org = "lmorg";
        public const string OrgIdAndRightsId = "lmorgrights"; //format OrgID;Right1,Right2,Right3
        public const string OrgIdAndRoleId = "lmorgroles"; //OrgID;Role1,Role1,Role3
    }

    public enum MessageStatus
    {
        Default = 0,
        Starred = 1,
        Archived = 2,
        InTrash = 3,
        Deleted = 99
    }

    public static class MachineCategoryEnum
    {
        public const string Tool = "31646";
        public const string Tractor = "31644";
        public const string Tresch = "31645";
    }

    public static class MachineImageType
    {
        public static string ProductImage = "Produktbild";
        public static string CategoryImage = "Kategori";
    }

    public static class DeliveryAssuranceRadioButtons
    {
        public const string Glyfosat = "Glyfosat";
        public const string Torkat = "TORKAT";
        public const string Red = "RED";
        public const string Straforkortat = "STRÅFÖRKORTNING";
        public const string Slam = "SLAMMAT";
    }

    public static class DeliveryAssuranceTermConditions
    {
        public const string OwnTransport = "CPT";
        public const string SupplierChoice = "CPX";
    }

    public static class DeliveryTypes
    {
        public const string Depa = "Depåavtal";
        public const string Spon = "Spontanleverans";
    }
}
