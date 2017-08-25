using System.Web.Mvc;
using EPiServer.Web.Mvc;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.Interfaces;
using Gro.Business.Services.Users;
using Gro.Infrastructure.Data;
using Gro.ViewModels.Blocks;
using Gro.Business.Rendering;
using Gro.Core.DataModels.Grain;
using System.Collections.Generic;

namespace Gro.Controllers.Blocks
{
    public class WeighinListBlockController : BlockController<WeighinListBlock>
    {
        private readonly IUserManagementService _usersManagementService;
        private readonly IGrainRepository _grainRepository;
        private readonly string _ticket;

        public WeighinListBlockController(IGrainRepository grainRepository, TicketProvider ticketProvider, IUserManagementService usersManagementService)
        {
            _grainRepository = grainRepository;
            _ticket = ticketProvider.GetTicket();
            _usersManagementService = usersManagementService;
        }

        public override ActionResult Index(WeighinListBlock currentBlock)
        {
            var deliveryListBlock = new WeighinListBlockViewModel();
            var supplier = _usersManagementService.GetActiveCustomer(HttpContext);
            deliveryListBlock.CurrentBlock = currentBlock;
            if (supplier != null)
            {
                deliveryListBlock.Deliveries = _grainRepository.GetDeliverysFiveLatest(supplier.CustomerNo, _ticket) ??
                    new List<Deliverys>();
            }
            else
            {
                deliveryListBlock.Deliveries = new List<Deliverys>();
            }
            return PartialView($"{TemplateCoordinator.BlockFolder}/GrainListBlock/DeliveryList.cshtml", deliveryListBlock);
        }
    }
}
