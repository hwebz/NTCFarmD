using Gro.Core.ContentTypes.Blocks.BlockWidths;

namespace Gro.Constants
{
    public static class ColumnLayout
    {
        public const string TwoColumnTag = nameof(ITwoColumnWidth);
        public const string OneColumnTag = nameof(IOneColumnWidth);
        public const string FooterColumnTag = nameof(FooterColumnTag);
    }

    public static class Cookies
    {
        public const string SiteUser = "_groUser";
        public const string ActiveCustomer = "_activeCustomer";
        public const string InternalActiveCustomer = "_internalActiveCustomer";
        public const string EPiServerLogin = ".EPiServerLogin";
    }

    public static class Email
    {
        public const string LantmannenFromAddress = "noreply@lantmannen.com";
    }

    public enum CompanyTypeEnum
    {
        EnskildFirma = 1,
        JuridiksPerson = 2
    }

    public static class DeliveryItemHeader
    {
        /// <summary>
        /// For the first Tbl
        /// </summary>
        public static string Sandning = "Sändning";
        public static string Transportor = "Transportör";
        public static string Bil = "Bil";
        public static string TelefonBil = "Telefon bil";

        public static string SummaKvantitet = "Summa Kvantitet";
        public static string AntalOrderrader = "Antal Orderrader";
        public static string FranFabrikLager = "Från fabrik/lager";
        public static string TelTransportor = "Tel Transportör";

        /// <summary>
        /// For the second table
        /// </summary>
        public static string Ordernr = "Ordernr";
        public static string Rad = "Rad";
        public static string Artikel = "Artikel";
        public static string BestKvantitet = "Best Kvantitet";
        public static string LevKvantitet = "Lev Kvantitet";
        public static string Silo = "Silo";
        public static string Planeradankomst = "Planerad ankomst";
        public static string PlaneradKvantitet = "Planerad Kvantitet";
        public static string Levererat = "Levererat";
        public static string FranFabrik = "Från fabrik";
        public static string LevererasTill = "Levereras till";
        public static string PlaneradHamtning = "Planerad hämtning";
        public static string FranLager = "Från lager";
        public static string Enhet = "Enhet";
        public static string Hamtat = "Hämtat";
        public static string LevKvant = "Lev. Kvant.";
        public static string BestKvant = "Best. Kvant.";

        public static string Ordernumbmer = "Ordernummer";
        public static string Sandnr = "Sändnr";
        public static string Summa = "Summa";
        public static string PlanAnkomst = "Plan. ankomst";
        public static string TillLager = "Till Lager";

        // 
        public static string Sandningsnr = "Sändningsnr";
        public static string LevForsakran = "Lev.försäkran";
    }
    public static class VirtualPathConfig
    {
        public static string ImageFolder = "images";
        public static string DocumentsFolder = "documents";
        public static string XmlFolder = "";
    }

    public static class DeliveryAssuranceAction
    {
        public const string Create = nameof(Create);
        public const string Change = nameof(Change);
        public const string Approve = nameof(Approve);
    }

    public static class DeliveryDateCssClass
    {
        public const string DateHigherThanToday = "dateHigherThanToday";
        public const string DateHarvest206 = "dateHarvest206";
        public const string DateHarvestRange = "dateHarvestRange";
        public const string DateRange = "daterange";
    }

    public static class SearchTransportCatogories
    {
        public const string CustomerNumber = "Kundnummer";
        public const string Ordernummer = "Ordernummer";
        public const string ShipmentId = "Sändningsnr";
        public const string CustomerOrderNumber = "Inköpsordernr";
        public const string Carrier = "Lastbil";
        public const string WayBill = "Fraktsedelnr";
    }

    public static class Priskey
    {
        public const string Spotprisavtal = "Spotprisavtal";
        public const string Terminsavtal = "Terminsavtal";
        public const string Poolavtal = "Poolavtal";
        public const string Depaavtal = "Depåavtal";
    }

    public enum SearchOptions
    {
        CustomerNumber,
        CompanyName,
        OrganizationNumber,
        UserName,
        UserId,
        PersonalNumber
    }

}
