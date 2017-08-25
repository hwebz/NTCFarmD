using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.CustomProperties;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.AppPages.PurchasingAgreement
{
    [ContentType(GUID = "4D447622-563B-4708-90A2-5E553DD51C72")]
    public class PurchasingAgreementPage : SitePageBase, ITwoColumnContainer
    {
        [CultureSpecific]
        [AutoSuggestSelection(typeof(AgreementTypeSelectionQuery))]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string AgreementType { get; set; }

        [Display(Order = 30, GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        public virtual Url PurchaseTermAndConditionPage { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 50)]
        [AllowedTypes(typeof(ListBlock))]
        public virtual ContentArea RightContent { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 60)]
        public virtual XhtmlString Introduce { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 70)]
        public virtual XhtmlString Body { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            AgreementType = Utils.AgreementType.SportAndForwardAvtal;
        }
    }
}
