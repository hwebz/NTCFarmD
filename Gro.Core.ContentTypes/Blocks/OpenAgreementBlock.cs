using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Blocks.BlockWidths;
using Gro.Core.ContentTypes.Pages.AppPages;

namespace Gro.Core.ContentTypes.Blocks
{
    [ContentType(DisplayName = "OpenAgreementBlock", GUID = "89e33172-aa4b-46ad-92a0-38e76d11f8c2", Description = "")]
    public class OpenAgreementBlock : BlockData, IOneColumnContainer, IOneColumnWidth
    {
        [Display(Order = 10, Name = "Maximum Grain Trade Items", GroupName = SystemTabNames.Settings)]
        [CultureSpecific]
        [Range(1, 5)]
        public virtual int MaxGrainTradeItems { get; set; }

        [Display(Order = 20, Name = "Maximum Seed Trade Items", GroupName = SystemTabNames.Settings)]
        [CultureSpecific]
        [Range(1, 5)]
        public virtual int MaxSeedTradeItems { get; set; }

        [Display(Order = 30, Name = "Block Heading", GroupName = SystemTabNames.Settings)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        [Display(Order = 40, Name = "No agreement text", GroupName = SystemTabNames.Settings)]
        [CultureSpecific]
        public virtual string NoAgreementText { get; set; }

        [Display(Order = 50, Name = "Agreement page link", GroupName = SystemTabNames.Settings)]
        [CultureSpecific]
        [Required]
        [AllowedTypes(typeof(AgreementPage))]
        public virtual ContentReference AgreementPageLink { get; set; }

        [Display(Order = 60, Name = "Link text", GroupName = SystemTabNames.Settings)]
        [CultureSpecific]
        public virtual string LinkText { get; set; }

        [Display(Order = 70, Name = "Grain trade headline", GroupName = SystemTabNames.Settings)]
        [CultureSpecific]
        public virtual string GrainTradeHeadline{ get; set; }

        [Display(Order = 80, Name = "Seed trade headline", GroupName = SystemTabNames.Settings)]
        [CultureSpecific]
        public virtual string SeedTradeHeadline { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            MaxGrainTradeItems = 3;
            MaxSeedTradeItems = 3;
        }
    }
}
