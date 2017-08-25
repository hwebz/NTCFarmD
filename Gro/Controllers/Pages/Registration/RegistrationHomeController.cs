using EPiServer.Web.Mvc;
using Gro.Business.Caching;
using Gro.Core.ContentTypes.Pages.Registration;
using Gro.ViewModels;
using System.Web.Mvc;

namespace Gro.Controllers.Pages.Registration
{
    [NoCache]
    public class RegistrationHomeController : PageController<RegistrationHomePage>
    {
        public ActionResult Index(RegistrationHomePage currentPage)
            => View("~/Views/Registration/RegistrationHome.cshtml", new PageViewModel<RegistrationHomePage>(currentPage));
    }
}
