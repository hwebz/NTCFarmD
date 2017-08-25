using System.Threading.Tasks;
using System.Web.Mvc;
using Gro.Core.ContentTypes.Pages.AppPages;
using Gro.Core.DataModels.GrobarhetDtos;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.AppPages.Grobarhet;
using Gro.Infrastructure.Data;

namespace Gro.Controllers.Pages.AppPages
{
    public class GrobarhetPageController : SiteControllerBase<GrobarhetPage>
    {
        private readonly IGrobarhetRepository _grobarhetRepository;
        private readonly string _ticket;

        public GrobarhetPageController(IGrobarhetRepository grobarhetRepository, TicketProvider ticketProvider)
        {
            _grobarhetRepository = grobarhetRepository;
            _ticket = ticketProvider.GetTicket();
        }

        public async Task<ActionResult> Index(GrobarhetPage currentPage)
        {
            var searchTerm = Request.QueryString["term"];
            var items = searchTerm == null ? new GrobarhetResponse[0] : (await _grobarhetRepository.GetGrobarhetAsync(searchTerm, _ticket));

            ViewData["searching"] = searchTerm != null;
            ViewData["searchTerm"] = searchTerm ?? string.Empty;

            return View("Index", new GrobarhetPageViewModel(currentPage)
            {
                SearchItems = items ?? new GrobarhetResponse[0]
            });
        }
    }
}
