using System.Web.Mvc;
using EPiServer.Web.Mvc;
using Gro.Core.ContentTypes.Blocks;
using Gro.Business.Rendering;
using Gro.ViewModels.Blocks;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data;
using Gro.Business.Services.Users;

namespace Gro.Controllers.Blocks
{
    public class AgreementListBlockController : BlockController<AgreementListBlock>
    {
        private readonly IUserManagementService _usersManagementService;
        private readonly IGrainRepository _grainRepository;
        private readonly string _ticket;

        public AgreementListBlockController(IGrainRepository grainRepository, TicketProvider ticketProvider, IUserManagementService usersManagementService)
        {
            _grainRepository = grainRepository;
            _ticket = ticketProvider.GetTicket();
            _usersManagementService = usersManagementService;
        }

        public override ActionResult Index(AgreementListBlock currentBlock)
        {
            var grainListBlock = new AgreementListBlockViewModel();
            var supplier = _usersManagementService.GetActiveCustomer(HttpContext);
            grainListBlock.CurrentBlock = currentBlock;
            if(supplier != null)
            {
                grainListBlock.ListAgreementsDeliverys = _grainRepository.GetAgreementsDeliverysThreeLatest(supplier.CustomerNo, _ticket);
            }
            return PartialView($"{TemplateCoordinator.BlockFolder}/GrainListBlock/Index.cshtml", grainListBlock);
        }
    }
}
