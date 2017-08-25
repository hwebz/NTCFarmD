using System.Web.Mvc;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Web.Mvc;
using Gro.Core.ContentTypes.Blocks;
using Gro.Constants;
using Gro.Business.Rendering;
using Gro.Business.Services.News;
using Gro.ViewModels.Blocks;

namespace Gro.Controllers.Blocks
{
    [TemplateDescriptor(
        Default = true,
        Inherited = true,
        TemplateTypeCategory = TemplateTypeCategories.MvcPartialController,
        AvailableWithoutTag = true,
        Tags = new[] { ColumnLayout.TwoColumnTag, ColumnLayout.OneColumnTag })]
    public class NewsListBlockController : BlockController<NewsListBlock>
    {
        private readonly INewsService _newsService;

        public NewsListBlockController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public override ActionResult Index(NewsListBlock currentBlock)
        {
            var recentInformationPages = _newsService.GetLatestInformationPages(currentBlock.ArchivePage, HttpContext);
            var layout = (string)ControllerContext.ParentActionViewContext.ViewData["Tag"];

            var viewName = layout == ColumnLayout.TwoColumnTag ?
                "ThreeColumns.cshtml" : "OneColumn.cshtml";

            return PartialView($"{TemplateCoordinator.BlockFolder}/NewsListBlock/{viewName}", new NewsListBlockViewModel
            {
                CurrentBlock = currentBlock,
                RecentInformationPages = recentInformationPages
            });
        }
    }
}
