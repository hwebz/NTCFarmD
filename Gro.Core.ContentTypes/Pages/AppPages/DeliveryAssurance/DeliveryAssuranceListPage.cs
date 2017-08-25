using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Pages.BokaPages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages.AppPages.DeliveryAssurance
{
    [ContentType(GUID = "2D635D12-BFE1-4C19-A086-61E6544FBD59")]
    [AvailableContentTypes(Availability.None)]
    public class DeliveryAssuranceListPage : SitePageBase, ITwoColumnContainer
    {
        //List settings
        [Display(Order = 20, GroupName = SystemTabNames.Content)]
        public virtual XhtmlString Introduction { get; set; }
        [Display(Order = 30, GroupName = SystemTabNames.Content)]
        [UIHint(UIHint.LongString)]
        public virtual string RemoveDeliveryAssuranceConfirmText { get; set; }

        [Display(Order = 40, GroupName = SystemTabNames.Content)]
        [AllowedTypes(typeof(BokaPage))]
        public virtual ContentReference BookDeliveryPageUrl { get; set; }

        [Display(Order = 10, GroupName = GroupNames.FormSettings)]
        public virtual string ChangeNotAvailableHeader { get; set; }

        [Display(Order = 20,  GroupName = GroupNames.FormSettings)]
        public virtual XhtmlString ChangeNotAvailable { get; set; }

        //Form settings
        [Display(Order = 30, GroupName = GroupNames.FormSettings)]
        [DefaultValue("Översikt leveransförsäkran")]
        public virtual string OverviewHeader { get; set; }

        [Display(Order = 40, GroupName = GroupNames.FormSettings)]
        [DefaultValue("Ändra leveransförsäkran")]
        public virtual string ApproveHeader { get; set; }

        [Display(Order = 50, GroupName = GroupNames.FormSettings)]
        [DefaultValue("Skapa leveransförsäkran")]
        public virtual string CreateHeader { get; set; }

        [Display(Order = 60, GroupName = GroupNames.FormSettings)]
        [DefaultValue("Godkänn leveransförsäkran")]
        public virtual string ChangeHeader { get; set; }

        [Display(Order = 70, GroupName = GroupNames.FormSettings)]
        public virtual Url DeliveryTermsPageUrl { get; set; }

        [Display(Order = 80, GroupName = GroupNames.FormSettings)]
        public virtual Url MyPageUrl { get; set; }

        //Confirm Page settings
        [Display(Order = 10, GroupName = GroupNames.ConfirmPageSettings)]
        public virtual string ThankYouText { get; set; }

        [Display(Order = 20, GroupName = GroupNames.ConfirmPageSettings)]
        public virtual string ThankYouTextForUpdate { get; set; }

        [Display(Order = 30, GroupName = GroupNames.ConfirmPageSettings)]
        public virtual string ErrorText { get; set; }

        [Display(Order = 40, GroupName = GroupNames.ConfirmPageSettings)]
        public virtual string ButtonConfirmText { get; set; }

        //Print settings
        [Display(Order = 10, GroupName = GroupNames.PrintSettings)]
        public virtual string Header1 { get; set; }
        [Display(Order = 20, GroupName = GroupNames.PrintSettings)]
        public virtual string Header2 { get; set; }
        [Display(Order = 30, GroupName = GroupNames.PrintSettings)]
        public virtual string Header3 { get; set; }
        [Display(Order = 40, GroupName = GroupNames.PrintSettings)]
        public virtual string Header4 { get; set; }
        [Display(Order = 50, GroupName = GroupNames.PrintSettings)]
        public virtual XhtmlString MainBody { get; set; }
        [Display(Order = 60, GroupName = GroupNames.PrintSettings)]
        [UIHint(UIHint.LongString)]
        public virtual string Footer1 { get; set; }
        [Display(Order = 70, GroupName = GroupNames.PrintSettings)]
        [UIHint(UIHint.LongString)]
        public virtual string Footer2 { get; set; }
        [Display(Order = 80, GroupName = GroupNames.PrintSettings)]
        [UIHint(UIHint.LongString)]
        public virtual string Footer3 { get; set; }
        [Display(Order = 90, GroupName = GroupNames.PrintSettings)]
        [UIHint(UIHint.LongString)]
        public virtual string Footer4 { get; set; }
        [Display(Order = 100, GroupName = GroupNames.PrintSettings)]
        [UIHint(UIHint.LongString)]
        public virtual string Footer5 { get; set; }
    }
}
