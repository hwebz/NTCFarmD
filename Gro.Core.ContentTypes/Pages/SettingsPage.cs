using System;
using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.ContentTypes.Pages.AppPages.DeliveryAssurance;
using Gro.Core.ContentTypes.Pages.ContainerPages;
using Gro.Core.ContentTypes.Pages.Messages;
using Gro.Core.ContentTypes.Pages.MyProfile;
using Gro.Core.ContentTypes.Pages.Organization;
using Gro.Core.ContentTypes.Pages.Registration;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.ContentTypes.Pages.BookService;
using Gro.Core.ContentTypes.Pages.InternalPages;
using Gro.Core.ContentTypes.Pages.StartPages;

namespace Gro.Core.ContentTypes.Pages
{
    [ContentType(GUID = "964d7984-1b3d-4a6d-8ff8-9bec401523cb")]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-settings.png")]
    public class SettingsPage : PageData, IContainerPage
    {
        #region Analytics

        [Display(GroupName = GroupNames.Analytics, Order = 10)]
        public virtual string GoogleTagMgrId { get; set; }

        #endregion

        #region General

        [Display(GroupName = GroupNames.SiteSettings, Order = 10)]
        [CultureSpecific]
        public virtual string SiteName { get; set; }

        [Display(GroupName = GroupNames.SiteSettings, Order = 20)]
        [CultureSpecific]
        public virtual string SelectServiceText { get; set; }

        [Display(GroupName = GroupNames.SiteSettings, Order = 30)]
        [CultureSpecific]
        public virtual int PurchasePriceLow { get; set; }

        #region Delivery Assurance

        [Display(GroupName = GroupNames.SiteSettings, Order = 30)]
        [CultureSpecific]
        public virtual bool IsInternal { get; set; }

        [Display(GroupName = GroupNames.SiteSettings, Order = 40)]
        [CultureSpecific]
        //"601,602,603,610";
        public virtual string LorryTypesInExternalSite { get; set; }

        [Display(GroupName = GroupNames.SiteSettings, Order = 50)]
        [CultureSpecific]
        [AllowedTypes(typeof(DeliveryAssuranceListPage))]
        public virtual ContentReference DeliveryAssurancePage { get; set; }

        #endregion

        #endregion

        #region Registration settings

        [Display(GroupName = GroupNames.Registration, Order = 10)]
        [CultureSpecific]
        [AllowedTypes(typeof(RegisterAccountPage))]
        public virtual ContentReference RegisterAccountPage { get; set; }

        [Display(GroupName = GroupNames.Registration, Order = 10)]
        [CultureSpecific]
        [AllowedTypes(typeof(RegistrationHomePage))]
        public virtual ContentReference RegistrationHomePage { get; set; }

        #endregion

        #region Footer
        [Display(GroupName = GroupNames.Footer, Order = 10)]
        [CultureSpecific]
        [UIHint(UIHint.LongString)]
        public virtual string FreeTextOnFooter { get; set; }

        [Display(GroupName = GroupNames.Footer, Order = 20)]
        [AllowedTypes(typeof(ListBlock), RestrictedTypes = new Type[] { })]
        [CultureSpecific]
        public virtual ContentArea Column1 { get; set; }

        [Display(GroupName = GroupNames.Footer, Order = 30)]
        [AllowedTypes(typeof(ListBlock), RestrictedTypes = new Type[] { })]
        [CultureSpecific]
        public virtual ContentArea Column2 { get; set; }

        [Display(GroupName = GroupNames.Footer, Order = 40)]
        [AllowedTypes(typeof(ListBlock), RestrictedTypes = new Type[] { })]
        [CultureSpecific]
        public virtual ContentArea Column3 { get; set; }

        #endregion

        #region ExternalFooter
        [Display(GroupName = GroupNames.ExternalFooter, Order = 10)]
        [CultureSpecific]
        [UIHint(UIHint.LongString)]
        public virtual string ExternalFooterFreeText { get; set; }

        [Display(GroupName = GroupNames.ExternalFooter, Order = 20)]
        [AllowedTypes(typeof(ListBlock), RestrictedTypes = new Type[] { })]
        [CultureSpecific]
        public virtual ContentArea ExternalFooterColumn1 { get; set; }

        [Display(GroupName = GroupNames.ExternalFooter, Order = 30)]
        [AllowedTypes(typeof(ListBlock), RestrictedTypes = new Type[] { })]
        [CultureSpecific]
        public virtual ContentArea ExternalFooterColumn2 { get; set; }

        [Display(GroupName = GroupNames.ExternalFooter, Order = 40)]
        [AllowedTypes(typeof(ListBlock), RestrictedTypes = new Type[] { })]
        [CultureSpecific]
        public virtual ContentArea ExternalFooterColumn3 { get; set; }

        #endregion

        #region Messages setting
        [Display(GroupName = GroupNames.MessagesSettings, Order = 30)]
        [CultureSpecific]
        [Required]
        [AllowedTypes(typeof(UserMessagePage))]
        public virtual ContentReference UserViewMessage { get; set; }

        [Display(GroupName = GroupNames.MessagesSettings, Order = 30)]
        [CultureSpecific]
        [Required]
        [AllowedTypes(typeof(UserSettingPage))]
        public virtual ContentReference UserSettingMessage { get; set; }

        [Display(GroupName = GroupNames.MessagesSettings, Order = 40)]
        [CultureSpecific]
        [AllowedTypes(typeof(AdministerMessagePage))]
        [Required]
        public virtual ContentReference AdminSettingMessage { get; set; }

        [Display(GroupName = GroupNames.MessagesSettings, Order = 50)]
        [CultureSpecific]
        [UIHint(UIHint.LongString)]
        public virtual string EmptyCategoryMessage { get; set; }

        [Display(GroupName = GroupNames.MessagesSettings, Order = 60)]
        [CultureSpecific]
        [Range(1, int.MaxValue)]
        public virtual int PageSizeInViewMessage { get; set; }

        [Display(GroupName = GroupNames.MessagesSettings, Order = 70)]
        [CultureSpecific]
        [Range(1, int.MaxValue)]
        public virtual int PageSizeInAdminMessage { get; set; }

        [Display(GroupName = GroupNames.MessagesSettings, Order = 80)]
        [CultureSpecific]
        public virtual XhtmlString MessageSignature { get; set; }

        #endregion

        #region My profile setting

        [Display(GroupName = GroupNames.ProfileSettings, Order = 30)]
        [CultureSpecific]
        [AllowedTypes(typeof(MyAccountPage))]
        [Required]
        public virtual ContentReference MyAccountLink { get; set; }

        [Display(GroupName = GroupNames.ProfileSettings, Order = 40)]
        [CultureSpecific]
        [AllowedTypes(typeof(UserAgreementsPage))]
        public virtual ContentReference UserAgreementPage { get; set; }

        [Display(GroupName = GroupNames.ProfileSettings, Order = 50)]
        [CultureSpecific]
        [AllowedTypes(typeof(AddDeliveryAddressPage))]
        public virtual ContentReference AddDeliveryAddressPage { get; set; }

        [Display(GroupName = GroupNames.ProfileSettings, Order = 60)]
        [CultureSpecific]
        [AllowedTypes(typeof(HandleDeliveryAddressPage))]
        public virtual ContentReference HandleAddressPage { get; set; }

        [Display(GroupName = GroupNames.ProfileSettings, Order = 70)]
        [CultureSpecific]
        [AllowedTypes(typeof(AddUserToOrganizationPage))]
        public virtual ContentReference AddUserToOrganizationPage { get; set; }

        [Display(GroupName = GroupNames.ProfileSettings, Order = 80)]
        [CultureSpecific]
        [AllowedTypes(typeof(HandleOrganizationUserPage))]
        public virtual ContentReference HandleOrganizationUserPage { get; set; }

        [CultureSpecific]
        public virtual ContentReference MachineMediaFolder { get; set; }

        #endregion

        [Display(GroupName = GroupNames.SiteSettings, Order = 90)]
        [CultureSpecific]
        [AllowedTypes(typeof(BookServicePilotenPage))]
        public virtual ContentReference BookServicePage { get; set; }

        [Display(GroupName = GroupNames.SiteSettings, Order = 100)]
        [CultureSpecific]
        [AllowedTypes(typeof(MaskinStartPage))]
        public virtual ContentReference MachineStartPage { get; set; }

        [Display(GroupName = GroupNames.SiteSettings, Order = 120)]
        [CultureSpecific]
        [AllowedTypes(typeof(DeliveryMessagesPage))]
        public virtual ContentReference PlannedDeliveryPage { get; set; }

        [Display(GroupName = GroupNames.InternalPageSettings, Order = 130)]
        [CultureSpecific]
        [AllowedTypes(typeof(CustomerCardPage))]
        public virtual ContentReference CustomerCardPage { get; set; }

        [Display(GroupName = GroupNames.InternalPageSettings, Order = 140)]
        [AllowedTypes(typeof(InternalStartPage))]
        public virtual ContentReference InternalStartPage { get; set; }
    }
}
