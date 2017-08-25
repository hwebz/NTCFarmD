using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Web.Mvc;
using Gro.Business.Services.Users;
using Gro.Constants;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.Interfaces;
using Gro.ViewModels.Blocks;
using System.Web.Mvc;
using Gro.Core.DataModels.AgreementDtos;

namespace Gro.Controllers.Blocks
{
    [TemplateDescriptor(
        Default = true,
        Inherited = true,
        TemplateTypeCategory = TemplateTypeCategories.MvcPartialController,
        AvailableWithoutTag = true,
        Tags = new[] {ColumnLayout.OneColumnTag, ColumnLayout.FooterColumnTag})]
    public class OpenAgreementBlockController : BlockController<OpenAgreementBlock>
    {
        private readonly IAgreementRepository _agreementRepo;
        private readonly IUserManagementService _userManager;

        public OpenAgreementBlockController(IAgreementRepository agreementRepo, IUserManagementService userManager)
        {
            _agreementRepo = agreementRepo;
            _userManager = userManager;
        }

        public override ActionResult Index(OpenAgreementBlock currentBlock)
        {
            var activeCustomer = _userManager.GetActiveCustomer(HttpContext);

            var grainAgreements = _agreementRepo.GetOpenGrainAgreements(activeCustomer?.CustomerNo, currentBlock.MaxGrainTradeItems);
            var seedAgreements = _agreementRepo.GetOpenSeedAgreements(activeCustomer?.CustomerNo, currentBlock.MaxSeedTradeItems);

            return PartialView("~/Views/Shared/Blocks/OpenAgreementBlock.cshtml", new OpenAgreementsBlockViewModel(currentBlock)
            {
                OpenGrainAgreements = grainAgreements ?? new Agreement[0],
                OpenSeedAgreements = seedAgreements ?? new SeedAssurance[0]
            });
        }
    }
}
