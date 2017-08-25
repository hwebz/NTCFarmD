using System.Web.Mvc;
using Gro.Core.ContentTypes.Pages.BookService;
using Gro.ViewModels.Pages.BookService;
using Gro.Core.Interfaces;
using Gro.Business.Services.Users;
using System.Threading.Tasks;
using System;
using System.Linq;
using Gro.Core.DataModels.Security;

namespace Gro.Controllers.Pages.BookService
{
    [CustomerRole]
    public class BookServiceController : SiteControllerBase<BookServicePilotenPage>
    {
        private readonly IBookingContactRepository _contactRepository;
        private readonly IOrganizationUserRepository _orgUser;
        private readonly ISecurityRepository _securityRepository;

        public BookServiceController(IUserManagementService userManager,
            IBookingContactRepository contactRepo,
            IOrganizationUserRepository orgUser,
            ISecurityRepository securityRepo)
        {
            _contactRepository = contactRepo;
            _orgUser = orgUser;
            _securityRepository = securityRepo;
        }

        public async Task<ActionResult> Index(BookServicePilotenPage currentPage, string city, string model,
            string serial, string register, string reference)
        {
            var customer = UserManager.GetActiveCustomer(HttpContext);
            var owner = await _orgUser.GetOwnerAsync(customer);

            var userInfo = await _securityRepository.QueryUserAsync(SiteUser.UserName);
            var starredGarage = await _contactRepository.GetStarredGarageAsync(customer);

            if (string.IsNullOrWhiteSpace(city))
            {
                //city is empty, get the favorite
                city = starredGarage;
            }

            var cityInfo = (await _contactRepository.GetGaragesAsync(customer, city))?.FirstOrDefault();

            ViewData["starred"] = cityInfo?.City == starredGarage;
            ViewData["reference"] = reference;

            return View("~/Views/BookService/BookService.cshtml", new BookServicePilotenViewModel(currentPage)
            {
                Customer = customer,
                UserName = SiteUser.Name,
                OwnerEmail = owner.UserId == SiteUser.UserName ? null : owner?.Email,
                UserEmail = SiteUser.Email ?? string.Empty,
                MachineModel = model ?? string.Empty,
                MachineRegister = register ?? string.Empty,
                MachineSerialNumber = serial ?? string.Empty,
                PhoneNumber = userInfo?.UserName ?? string.Empty,
                City = city
            });
        }

        [HttpPost]
        public async Task<ActionResult> Index(BookServicePilotenPage currentPage, BookServiceForm form)
        {
            var userInfo = await _securityRepository.QueryUserAsync(SiteUser.UserName);
            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);

            var starredCity = await _contactRepository.GetStarredGarageAsync(activeCustomer);
            var cityName = form.City;
            if (string.IsNullOrWhiteSpace(cityName))
            {
                cityName = starredCity;
            }
            ViewData["starred"] = cityName == starredCity;

            if (!ModelState.IsValid)
            {
                return View("~/Views/BookService/BookService.cshtml", PostViewModel(currentPage, form, activeCustomer));
            }
            var city = (await _contactRepository.GetGaragesAsync(activeCustomer, form.City))?.FirstOrDefault();

            var salesMenInCity = await _contactRepository.GetSalemenByCityAsync(cityName);
            var saleMan = salesMenInCity?.FirstOrDefault(s => s.Description == "Säljare") ?? salesMenInCity?.FirstOrDefault();

            var requestResult = await _contactRepository.RequestBooking(form.MachineModel,
                form.MachineSerialNumber, form.MachineRegister,
                form.Message, activeCustomer, city, SiteUser.Name, SiteUser.Email, userInfo.PhoneMobile,
                form.OwnerEmail, saleMan?.Email, SiteUser.Email != form.OwnerEmail);

            //parse result
            var tokens = requestResult?.Split(new[] { "<br />" }, StringSplitOptions.RemoveEmptyEntries) ?? new string[2];
            var bookingSaved = string.Equals(tokens[0], "Garage booking saved.", StringComparison.OrdinalIgnoreCase);
            var bookingSent = string.Equals(tokens[1], "Booking email sent.", StringComparison.OrdinalIgnoreCase);

            if (bookingSaved)
            {
                ViewData["backLinkReference"] = form.BackLinkReference;
                ViewData["reference"] = form.Reference;
                return View("~/Views/BookService/MessageSent.cshtml", PostViewModel(currentPage, form, activeCustomer));
            }

            var strError = !bookingSent
                ? "Kunde inte spara garage bokning. Misslyckades skicka boknings e-post."
                : "Kunde inte spara garage bokning. Bokning e-postmeddelandet.";

            ViewData["error"] = strError;
            return View("~/Views/BookService/BookService.cshtml", PostViewModel(currentPage, form, activeCustomer));
        }

        private BookServicePilotenViewModel PostViewModel(BookServicePilotenPage page, BookServiceForm bookserviceForm, CustomerBasicInfo customer)
            => new BookServicePilotenViewModel(page)
            {
                Customer = customer,
                MachineModel = bookserviceForm.MachineModel ?? string.Empty,
                MachineRegister = bookserviceForm.MachineRegister ?? string.Empty,
                MachineSerialNumber = bookserviceForm.MachineSerialNumber ?? string.Empty,
                PhoneNumber = bookserviceForm.PhoneNumber ?? string.Empty,
                OwnerEmail = bookserviceForm.OwnerEmail ?? string.Empty,
                UserEmail = SiteUser.Email ?? string.Empty,
                UserName = SiteUser.Name ?? string.Empty,
                City = bookserviceForm.City ?? string.Empty
            };
    }
}
