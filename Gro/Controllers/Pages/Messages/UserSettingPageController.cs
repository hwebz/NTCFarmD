using System.Web.Mvc;
using Gro.Core.ContentTypes.Pages.Messages;
using Gro.ViewModels;

namespace Gro.Controllers.Pages.Messages
{
    public class UserSettingPageController : SiteControllerBase<UserSettingPage>
    {
        public ActionResult Index(UserSettingPage currentPage)
        {
            var model = new PageViewModel<UserSettingPage>(currentPage);
            return View("Index", model);
        }
    }
}
