using System.Web.Mvc;
using Gro.Core.ContentTypes.Pages.Messages;
using Gro.ViewModels;

namespace Gro.Controllers.Pages.Messages
{
    //[CustomerRole]
    public class UserMessageController : SiteControllerBase<UserMessagePage>
    {
        // GET: UserMessage
        public ActionResult Index(UserMessagePage currentPage)
        {
            var model = new PageViewModel<UserMessagePage>(currentPage);
            return View("~/Views/Messages/UserMessage/Index.cshtml", model);
        }
    }
}
