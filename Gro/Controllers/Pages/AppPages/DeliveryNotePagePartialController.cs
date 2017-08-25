using System.Linq;
using System.Web.Mvc;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.AppPages;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.AppPages.DeliveryNote;
using Gro.Infrastructure.Data;
using Gro.Core.DataModels.DeliveryNoteDtos;

namespace Gro.Controllers.Pages.AppPages
{
    [TemplateDescriptor(TemplateTypeCategory = TemplateTypeCategories.MvcPartialController)]
    public class DeliveryNotePagePartialController : SiteControllerBase<DeliveryNotePage>
    {
        /* Faking data, will be implemented later*/
        private readonly string _ticket;
        private readonly IDeliveryNoteRepository _deliveryNoteRepository;

        public DeliveryNotePagePartialController(
            IDeliveryNoteRepository deliveryNoteRepository,
            IUserManagementService userManager,
            TicketProvider ticketProvider) : base(userManager)
        {
            _deliveryNoteRepository = deliveryNoteRepository;
            _ticket = ticketProvider.GetTicket();
        }

        public ActionResult Index(DeliveryNotePage currentPage)
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);
            if (string.IsNullOrWhiteSpace(supplier?.CustomerNo))
            {
                return PartialView("/Views/AppPages/DeliveryNotePage/Partial/Index.cshtml", new DeliveryNotePageViewModel(currentPage)
                {
                    ListDeliveries = new FoljesedelResponse[0]
                });
            }

            var listDeliveries = _deliveryNoteRepository.GetFoljesedlar(supplier.CustomerNo, _ticket);

            var model = new DeliveryNotePageViewModel(currentPage)
            {
                ListDeliveries = listDeliveries?.Skip(0).Take(20) ?? new FoljesedelResponse[0],
            };
            return PartialView("/Views/AppPages/DeliveryNotePage/Partial/Index.cshtml", model);
        }
    }
}
