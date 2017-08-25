using System.Web.Mvc;
using EPiServer.Web.Mvc;
using Gro.Core.ContentTypes.Blocks;
using Gro.Constants;
using Gro.Business.Rendering;
using Gro.Business.Services.News;
using Gro.ViewModels.Blocks;

namespace Gro.Controllers.Blocks
{
    public class InformationListBlockController : BlockController<InformationListBlock>
    {
        private readonly INewsService _newsService;

        public InformationListBlockController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public override ActionResult Index(InformationListBlock currentBlock)
        {
            var recentInformationPages = _newsService.GetLatestInformationPages(currentBlock.ArchivePage, HttpContext, currentBlock.NumberOfItems);

            var layout = (string)ControllerContext.ParentActionViewContext.ViewData["Tag"];
            var viewName = layout == ColumnLayout.TwoColumnTag ? "TwoThirds" : "OneThird";

            return PartialView($"{TemplateCoordinator.BlockFolder}/InformationListBlock/{viewName}.cshtml", new InformationListBlockViewModel
            {
                CurrentBlock = currentBlock,
                RecentInformationPages = recentInformationPages
            });
        }
    }
}
