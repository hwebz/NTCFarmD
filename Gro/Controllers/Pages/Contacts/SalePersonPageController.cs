using Gro.Core.ContentTypes.Business;
using Gro.Core.ContentTypes.Pages.Contacts;
using Gro.Core.DataModels.Contacts;
using Gro.Core.Interfaces;
using Gro.ViewModels.Contacts;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Gro.Controllers.Pages.Contacts
{
    public class SalePersonPageController : SiteControllerBase<SalesPersonPage>
    {
        private readonly IBookingContactRepository _contactRepo;
        public SalePersonPageController(IBookingContactRepository contactRepo)
        {
            _contactRepo = contactRepo;
        }

        [HttpGet]
        public async Task<ActionResult> Index(SalesPersonPage currentPage, string query, string id)
        {
            if (!string.IsNullOrWhiteSpace(id)) return (await SalesmenDetailAsync(currentPage, id));

            var categoryString = EnumSelectionFactory<ServiceTeam>.GetDisplayText(currentPage.TeamCategory);
            ViewData["category"] = categoryString;
            var salesPersons = string.IsNullOrWhiteSpace(query) ?
                (await _contactRepo.GetAllSalesMenAsync(categoryString)) : (await _contactRepo.SearchSalesMenAsync(categoryString, query));

            salesPersons = salesPersons ?? new SalesPerson[0];
            ViewData["query"] = string.IsNullOrWhiteSpace(query) ? string.Empty : query;

            return View("~/Views/ContactPage/SalePersonSearchPage.cshtml", new PersonSearchPageViewModel(currentPage, salesPersons));
        }

        private async Task<ActionResult> SalesmenDetailAsync(SalesPersonPage currentPage, string id)
        {
            var categoryString = EnumSelectionFactory<ServiceTeam>.GetDisplayText(currentPage.TeamCategory);
            ViewData["category"] = categoryString;

            var allSalesMen = (await _contactRepo.GetAllSalesMenAsync(categoryString)) ?? new SalesPerson[0];
            var saleMan = allSalesMen.FirstOrDefault(s => s.Id == id);
            if (saleMan == null) { throw new Exception(); }

            return View("~/Views/ContactPage/SalePersonDetailPage.cshtml", new SalePersonDetailPageViewModel(currentPage, saleMan));
        }
    }
}
