using System;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using Gro.Core.ContentTypes.Blocks.BlockWidths;
using Gro.Core.ContentTypes.Pages.StartPages;
using Gro.ViewModels;

namespace Gro.Controllers.Pages
{
    /* Note: as the content area rendering on Alloy is customized we create ContentArea instances
     * which we render in the preview view in order to provide editors with a preview which is as
     * realistic as possible. In other contexts we could simply have passed the block to the
     * view and rendered it using Html.RenderContentData */
    [TemplateDescriptor(
        Inherited = true,
        TemplateTypeCategory = TemplateTypeCategories.MvcController, //Required as controllers for blocks are registered as MvcPartialController by default
        Tags = new[] { RenderingTags.Preview, RenderingTags.Edit },
        AvailableWithoutTag = false)]
    public class PreviewController : ActionControllerBase, IRenderTemplate<BlockData>
    {
        private readonly IContentLoader _contentLoader;

        public PreviewController(IContentLoader contentLoader, TemplateResolver templateResolver, DisplayOptions displayOptions)
        {
            _contentLoader = contentLoader;
        }

        public ActionResult Index(IContent currentContent)
        {
            //As the layout requires a page for title etc we "borrow" the start page
            var startPage = _contentLoader.Get<StartPage>(SiteDefinition.Current.StartPage);
            var blockType = currentContent.GetType().BaseType?.Name;

            var uiDescriptors = currentContent.GetType().BaseType?.GetInterfaces();
            if (uiDescriptors == null)
            {
                throw new ArgumentNullException(nameof(currentContent));
            }

            var viewModel = new PreviewModel(startPage, currentContent);
            foreach (var uiDescriptor in uiDescriptors)
            {
                if (uiDescriptor == typeof(ITwoColumnWidth))
                {
                    viewModel.Areas.Add(NewPreviewArea($"{blockType} ({nameof(ITwoColumnWidth)})", nameof(ITwoColumnWidth), currentContent, "u-1/1 u-2/3-tablet"));
                }
                if (uiDescriptor == typeof(IOneColumnWidth))
                {
                    viewModel.Areas.Add(NewPreviewArea($"{blockType} ({nameof(IOneColumnWidth)})", nameof(IOneColumnWidth), currentContent, "u-1/1 u-1/3-tablet"));
                }
            }
            if (viewModel.Areas.Count == 0)
            {
                viewModel.Areas.Add(NewPreviewArea("Generic Preview", "", currentContent));
            }

            return View(viewModel);
        }

        /// <summary>
        /// Returns a new preview area for the block based on the display option.
        /// </summary>
        /// <param name="displayOptionName">The display option name.</param>
        /// <param name="displayOptionTag">The display option tag (i.e. the specific CSS class for the container element).</param>
        /// <param name="currentContent">An instance of the current block and its content.</param>
        /// <param name="cssClass">Css class</param>
        /// <returns>An instance of a new 'PreviewModel.PreviewArea' object.</returns>
        private static PreviewModel.PreviewArea NewPreviewArea(string displayOptionName, string displayOptionTag, IContent currentContent, string cssClass = "")
        {
            var contentArea = new ContentArea();
            contentArea.Items.Add(new ContentAreaItem
            {
                ContentLink = currentContent.ContentLink
            });
            var previewArea = new PreviewModel.PreviewArea
            {
                AreaName = displayOptionName,
                AreaTag = displayOptionTag,
                ContentArea = contentArea,
                CssClass = cssClass
            };

            return previewArea;
        }

    }
}
