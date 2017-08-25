using System.ComponentModel.DataAnnotations;
using EPiServer.DataAnnotations;

namespace Gro.Core.ContentTypes.Utils
{
    [GroupDefinitions]
    public static class GroupNames
    {
        #region Constants

        [Display(Name = "Site Settings", Order = 100)]
        public const string SiteSettings = nameof(SiteSettings);

        [Display(Name = nameof(CustomScripts), Order = 200)]
        public const string CustomScripts = nameof(CustomScripts);

        [Display(Name = nameof(MetaFacebook), Order = 300)]
        public const string MetaFacebook = nameof(MetaFacebook);

        [Display(Name = nameof(MetaTwitter), Order = 400)]
        public const string MetaTwitter = nameof(MetaTwitter);

        [Display(Name = nameof(SEO), Order = 500)]
        public const string SEO = nameof(SEO);

        [Display(Order = 600)]
        public const string Footer = nameof(Footer);

        [Display(Name = nameof(SiteVerification), Order = 700)]
        public const string SiteVerification = nameof(SiteVerification);

        [Display(Name = "Messages Settings", Order = 800)]
        public const string MessagesSettings = nameof(MessagesSettings);

        [Display(Name = "Profile Settings", Order = 900)]
        public const string ProfileSettings = nameof(ProfileSettings);

        [Display(Name = "Push Banner", Order = 1000)]
        public const string PushBannerSetting = nameof(PushBannerSetting);

        [Display(Name = "Registration Settings", Order = 1100)]
        public const string Registration = nameof(Registration);

        [Display(Name = "Print Settings", Order = 1200)]
        public const string PrintSettings = nameof(PrintSettings);

        [Display(Name = "Form Settings", Order = 1300)]
        public const string FormSettings = nameof(FormSettings);

        [Display(Name = "Confirm Page Settings", Order = 1400)]
        public const string ConfirmPageSettings = nameof(ConfirmPageSettings);

        [Display(Order = 1500, Name = "External Footer")]
        public const string ExternalFooter = nameof(ExternalFooter);

        [Display(Name = "Internal Pages Settings", Order = 1500)]
        public const string InternalPageSettings = nameof(InternalPageSettings);

        [Display(Name = nameof(Analytics), Order = 2000)]
        public const string Analytics = nameof(Analytics);
        #endregion
    }
}
