using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gro.Core.ContentTypes.Pages.AppPages;
using Gro.Core.DataModels.PriceGraph;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.AppPages.PriceGraph;
using Gro.Infrastructure.Data;

namespace Gro.Controllers.Pages.AppPages
{
    public class PriceGraphPageController : SiteControllerBase<PriceGraphPage>
    {
        private readonly IPriceGraphRepository _priceGraphRepo;
        private readonly string _ticket;

        public PriceGraphPageController(IPriceGraphRepository priceGraphRepo, TicketProvider ticketProvider)
        {
            _priceGraphRepo = priceGraphRepo;
            _ticket = ticketProvider.GetTicket();
        }

        public async Task<ActionResult> Index(PriceGraphPage currentPage)
        {
            int periodNumber;
            var periodQuery = Request.QueryString["period"];
            if (!int.TryParse(periodQuery, out periodNumber))
            {
                periodNumber = await _priceGraphRepo.GetStartingPeriodAsync(_ticket);
            }

            var endPeriod = DateTime.Today;
            var startPeriod = endPeriod.AddMonths(-periodNumber);
            var data = await _priceGraphRepo.GetAllChartDataAsync(startPeriod, endPeriod, _ticket);

            var periodOptions = await _priceGraphRepo.GetSelectablePeriodsAsync(_ticket) ?? new Dictionary<string, int>();
            ViewData["SelectedPeriod"] = periodNumber;
            return View("Index", new PriceGraphPageViewModel(currentPage)
            {
                GraphDisplay = data ?? new PriceGraphDisplay
                {
                    Legends = new string[0],
                    Data = new List<GraphItemRow>()
                },
                PeriodOptions = periodOptions
            });
        }
    }
}
