using System.Web.Mvc;
using Gro.Core.ContentTypes.Pages.Registration;
using Gro.ViewModels;
using EPiServer.Web.Mvc;
using Gro.Business.Caching;

namespace Gro.Controllers.Pages.Registration
{
    [NoCache]
    public class RegisterAccountController : PageController<RegisterAccountPage>
    {
        [HttpGet]
        public ActionResult Index(RegisterAccountPage currentPage)
            => View("~/Views/Registration/CustomerVerification.cshtml", new PageViewModel<RegisterAccountPage>(currentPage));

        [HttpPost]
        public ActionResult Index(RegisterAccountPage currentPage, string idNumber)
        {
            idNumber = idNumber.Trim();
            int n;
            if (!int.TryParse(idNumber, out n) || n < 0 || idNumber.Length != 10)
            {
                ViewData["error"] = "Organisationsnumret du angett är av fel typ. Korrigera detta och försök igen.";
                return View("~/Views/Registration/CustomerVerification.cshtml", new PageViewModel<RegisterAccountPage>(currentPage));
            }

            n = int.Parse(idNumber.Substring(2, 1));
            ViewData["idNumber"] = idNumber;
            return View($"~/Views/Registration/{(n >= 2 ? "NonPrivateFirm" : "PrivateFirm")}.cshtml", new PageViewModel<RegisterAccountPage>(currentPage));
        }
    }
}
