using System.Web.Mvc;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Web.Mvc;
using Gro.Business.Rendering;
using Gro.Business.Services.News;
using Gro.Constants;
using Gro.Core.ContentTypes.Blocks;
using Gro.ViewModels.Blocks;

namespace Gro.Controllers.Blocks
{
    [TemplateDescriptor(
        Default = true,
        Inherited = true,
        TemplateTypeCategory = TemplateTypeCategories.MvcPartialController,
        AvailableWithoutTag = true,
        Tags = new[] { ColumnLayout.OneColumnTag, ColumnLayout.FooterColumnTag })]
    public class ListBlockController : BlockController<ListBlock>
    {
        private readonly IGroContentDataService _groContentDataService;

        public ListBlockController(IGroContentDataService groContentDataService)
        {
            _groContentDataService = groContentDataService;
        }
        public override ActionResult Index(ListBlock currentBlock)
        {
            var layout = (string)ControllerContext.ParentActionViewContext.ViewData["Tag"];

            var viewName = layout == ColumnLayout.FooterColumnTag ?
                "ListBlockOnFooter.cshtml" : "ListBlock.cshtml";

            var viewModel = new ListBlockViewModel(currentBlock)
            {
                LinkItems = _groContentDataService.PopulateLinkItems(currentBlock.Items, HttpContext)
            };

            return PartialView($"{TemplateCoordinator.BlockFolder}/ListBlock/{viewName}", viewModel);
        }
    }
}
