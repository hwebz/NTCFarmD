using Gro.Core.ContentTypes.Pages.Contacts;
using Gro.Core.DataModels.Contacts;
using Gro.Core.Interfaces;
using Gro.ViewModels.Contacts;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Gro.Controllers.Pages.Contacts
{
    [CustomerRole]
    public class WorkshopPageController : SiteControllerBase<WorkshopPage>
    {
        private readonly IBookingContactRepository _contactRepo;

        public WorkshopPageController(IBookingContactRepository contactRepo)
        {
            _contactRepo = contactRepo;
        }

        [HttpGet]
        public async Task<ActionResult> Index(WorkshopPage currentPage, string query, string city)
        {
            if (!string.IsNullOrWhiteSpace(city)) return (await CityDetail(currentPage, city));
            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);

            ViewData["userName"] = SiteUser?.UserName ?? string.Empty;

            var garages = await (_contactRepo.GetGaragesAsync(activeCustomer, query?.Trim() ?? string.Empty)) ?? new GarageWorkshop[0];
            var starredGarage = await _contactRepo.GetStarredGarageAsync(activeCustomer);

            foreach (var garage in garages)
            {
                garage.OwnStar = garage.City == starredGarage;
            }

            return View("~/Views/ContactPage/WorkshopSearchPage.cshtml", new WorkshopPageViewModel(currentPage, garages));
        }

        private async Task<ActionResult> CityDetail(WorkshopPage currentPage, string city)
        {
            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);
            var cityInfo = await _contactRepo.GetGaragesAsync(activeCustomer, city);
            var workshop = cityInfo?.FirstOrDefault(c => c.City == city);
            if (workshop == null) return HttpNotFound();

            var salesMen = await _contactRepo.GetSalemenByCityAsync(city);
            var manager = salesMen.FirstOrDefault(s => s.Description == "Verkstadschef");
            ViewData["zipCode"] = manager?.Zipcode ?? string.Empty;
            ViewData["telephone"] = manager?.Phone ?? string.Empty;

            ViewData["userName"] = SiteUser?.UserName ?? string.Empty;

            var starredGarage = await _contactRepo.GetStarredGarageAsync(activeCustomer);
            workshop.OwnStar = starredGarage == workshop.City;

            return View("~/Views/ContactPage/WorkshopDetailPage.cshtml", new WorkshopDetailPageViewModel(currentPage)
            {
                SalesMen = salesMen,
                Workshop = workshop
            });
        }

        [Route("api/workshop/setstar")]
        [HttpPost]
        public async Task<JsonResult> SetStar(string city, string newStarValue)
        {
            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);
            var setStarResultTask = newStarValue == "True"
                ? _contactRepo.SetGarageStarAsync(activeCustomer, city)
                : _contactRepo.RemoveGarageStarAsync(activeCustomer, city);

            var setStarResult = await setStarResultTask;
            if (setStarResult) return Json(new { success = true });

            Response.StatusCode = 400;
            return Json(new { success = false });
        }
    }
}
