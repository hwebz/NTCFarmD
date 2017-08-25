using System.Linq;
using System.Web.Mvc;

namespace Gro.Business.Rendering
{
    public class SiteViewEngine : RazorViewEngine
    {
        private static readonly string[] AdditionalPartialViewFormats =
        {
            TemplateCoordinator.BlockFolder + "{0}.cshtml",
            TemplateCoordinator.PartialsFolder + "{0}.cshtml",
            TemplateCoordinator.StaticFolder + "{0}.cshtml",
            TemplateCoordinator.AppPagesFolder + "{1}/{0}.cshtml",
            TemplateCoordinator.MessagePagesFolder + "{1}/{0}.cshtml",
            TemplateCoordinator.ProfilesPagesFolder + "{1}/{0}.cshtml",
            TemplateCoordinator.BlockFolder + "{1}/{0}.cshtml",
        };

        private static readonly string[] AdditionViewLocationFormats =
        {
            TemplateCoordinator.AppPagesFolder + "{1}/{0}.cshtml",
            TemplateCoordinator.MessagePagesFolder + "{1}/{0}.cshtml",
            TemplateCoordinator.ProfilesPagesFolder + "{1}/{0}.cshtml"
        };

        public SiteViewEngine()
        {
            PartialViewLocationFormats =
                PartialViewLocationFormats.Union(AdditionalPartialViewFormats).ToArray();
            ViewLocationFormats = ViewLocationFormats.Union(AdditionViewLocationFormats).ToArray();
        }
    }
}
