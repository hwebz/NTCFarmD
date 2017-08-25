using System;
using System.Web.Mvc;
using Gro.Business.Rendering;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Gro.ViewModels;
using InitializationModule = EPiServer.Web.InitializationModule;

namespace Gro.Business.Initialization
{
    /// <summary>
    /// Module for customizing templates and rendering.
    /// </summary>
    [InitializableModule]
    [ModuleDependency(typeof(InitializationModule))]
    public class Rendering : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            //Add custom view engine allowing partials to be placed in additional locations
            //Note that we add it first in the list to optimize view resolving when using DisplayFor/PropertyFor
            ViewEngines.Engines.Insert(0, new SiteViewEngine());

            context.InitComplete += Context_InitComplete;
            context.Locate.TemplateResolver().TemplateResolved += TemplateCoordinator.OnTemplateResolved;
        }

        private static void Context_InitComplete(object sender, EventArgs e)
        {
            GlobalFilters.Filters.Add(ServiceLocator.Current.GetInstance<PageContextActionFilter>());
        }

        public void Preload(string[] parameters)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
            context.InitComplete -= Context_InitComplete;
            ServiceLocator.Current.GetInstance<TemplateResolver>().TemplateResolved -= TemplateCoordinator.OnTemplateResolved;
        }

    }
}
