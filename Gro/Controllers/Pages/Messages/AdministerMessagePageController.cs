using System.Web.Mvc;
using Gro.Core.ContentTypes.Pages.Messages;
using Gro.ViewModels;

namespace Gro.Controllers.Pages.Messages
{
    public class AdministerMessagePageController : SiteControllerBase<AdministerMessagePage>
    {
        public ActionResult Index(AdministerMessagePage currentPage)
        {
            var model = new PageViewModel<AdministerMessagePage>(currentPage);
            return View("Index", model);
        }
    }
}
