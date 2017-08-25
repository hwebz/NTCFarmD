using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Logging;
using EPiServer.Web.Mvc.Html;
using Gro.Business.Paging;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.AppPages;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.AppPages.DeliveryNote;
using Gro.Infrastructure.Data;
using Gro.Core.DataModels.DeliveryNoteDtos;

namespace Gro.Controllers.Pages.AppPages
{
    [SessionState(SessionStateBehavior.Disabled)]
    [TemplateDescriptor(Inherited = true)]
    public class DeliveryNotePageController : SiteControllerBase<DeliveryNotePage>
    {
        private static readonly ILogger Logger = LogManager.GetLogger();

        private readonly string _ticket;

        private readonly IDeliveryNoteRepository _deliveryNoteRepository;


        public DeliveryNotePageController(
            IDeliveryNoteRepository deliveryNoteRepository,
            IUserManagementService userManager,
            TicketProvider ticketProvider) : base(userManager)
        {
            _deliveryNoteRepository = deliveryNoteRepository;
            _ticket = ticketProvider.GetTicket();
        }

        public ActionResult Index(DeliveryNotePage currentPage, int? page)
        {
            var htmlHelper = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
            var url = htmlHelper.ContentUrl(currentPage.ContentLink);
            //var supplier = _securityRepository.GetCustomerNumber(User.Identity.Name);
            var supplier = UserManager.GetActiveCustomer(HttpContext);
            //private const string supplier = "77004216";
            
            if (string.IsNullOrWhiteSpace(supplier?.CustomerNo))
            {
                return View("Index", new DeliveryNotePageViewModel(currentPage)
                {
                    ListDeliveries = new FoljesedelResponse[0],
                    Pager = new Pager(0, 0, url)
                });
            }

            var listDeliveries = _deliveryNoteRepository.GetFoljesedlar(supplier.CustomerNo, _ticket);
            var foljesedelResponses = listDeliveries as FoljesedelResponse[] ?? listDeliveries.ToArray();
            var pager = new Pager(foljesedelResponses?.Count() ?? 0, page, url);
            var model = new DeliveryNotePageViewModel(currentPage)
            {
                //ListDeliveries = listDeliveries != null ? listDeliveries.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize) : null,
                ListDeliveries = foljesedelResponses,
                Pager = pager
            };

            return View("Index", model);
        }

        [Route("api/delivery-note/get-pdf")]
        public ActionResult GetPdf(string orderNumber, int fabricId, int rowNumber)
        {
            try
            {
                var pdfData = _deliveryNoteRepository.GetPDF(orderNumber, fabricId, rowNumber, _ticket);

                return Json(new {pdfData = pdfData}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Error("Method GetPdf Unhandle Exception: {0}", ex.Message);
                return null;
            }
        }
    }
}
