using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using Gro.Controllers.Pages;
using Gro.Core.ContentTypes.Pages.ContainerPages;

namespace Gro.Business.Rendering
{
    [ServiceConfiguration(typeof(IViewTemplateModelRegistrator))]
    public class TemplateCoordinator : IViewTemplateModelRegistrator
    {
        public const string BlockFolder = "~/Views/Shared/Blocks/";

        public const string DisplayTemplates = "~/Views/Shared/DisplayTemplates/";

        public const string PartialsFolder = "~/Views/Shared/Partials/";

        public const string StaticFolder = "~/Views/Shared/Partials/Static/";

        public const string AppPagesFolder = "~/Views/AppPages/";

        public const string MessagePagesFolder = "~/Views/Messages/";
        public const string ProfilesPagesFolder = "~/Views/MyProfile/";

        public static void OnTemplateResolved(object sender, TemplateResolverEventArgs args)
        {
            //Disable DefaultPageController for page types that shouldn't have any renderer as pages
            if (args.ItemToRender is IContainerPage && args.SelectedTemplate != null
                && args.SelectedTemplate.TemplateType == typeof(DefaultPageController))
            {
                args.SelectedTemplate = null;
            }
        }

        public void Register(TemplateModelCollection viewTemplateModelRegistrator)
        {
        }
    }
}
