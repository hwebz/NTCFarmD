using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages.AppPages.PurchasingAgreement
{
    [ContentType(GUID = "C6405AE0-4762-423F-AC0E-D5E1F1472192")]
    public class PriceHedgePage : SitePageBase, ITwoColumnContainer
    {
        [Display(Order = 20, GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        public virtual Url PurchaseTermAndConditionPage { get; set; }

        [Display(Order = 30, GroupName = SystemTabNames.Content)]
        [AllowedTypes(typeof(PriceAlertPage))]
        [CultureSpecific]
        public virtual ContentReference PriceAlertPage { get; set; }

        [Display(Order = 40, GroupName = GroupNames.ConfirmPageSettings)]
        [CultureSpecific]
        public virtual XhtmlString ConfirmationTextForDepaavtal { get; set; }

        [Display(Order = 50, GroupName = GroupNames.ConfirmPageSettings)]
        [CultureSpecific]
        public virtual XhtmlString ConfirmationTextForPoolavtal { get; set; }

        [Display(Order = 60, GroupName = GroupNames.ConfirmPageSettings)]
        [CultureSpecific]
        public virtual XhtmlString ConfirmationTextForSportavtal { get; set; }

        [Display(Order = 70, GroupName = GroupNames.ConfirmPageSettings)]
        [CultureSpecific]
        public virtual XhtmlString ConfirmationTextForPrissakringDepaavtal { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            VisibleInMenu = false;
        }
    }
}
