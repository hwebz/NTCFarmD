using System.Web.Mvc;
using System.Web.Routing;
using EPiServer.Security;
using Gro.CustomeBinder;
using Gro.Security;

namespace Gro
{
    public class EPiServerApplication : EPiServer.Global
    {
        protected void Application_Start()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (se, cert, chain, sslerror) => true;
            AreaRegistration.RegisterAllAreas();
            RegisterCustomProviders();

            // add custom binder to enable use "." as the delimiter 
            ModelBinders.Binders.Add(typeof(double), new DoubleModelBinder());
            //Tip: Want to call the EPiServer API on startup? Add an initialization module instead (Add -> New Item.. -> EPiServer -> Initialization Module)
        }

        private static void RegisterCustomProviders()
        {
            ProviderCapabilities.AddProvider(typeof(FarmdayRoleProvider), new ProviderCapabilitySettings(0));
            ProviderCapabilities.AddProvider(typeof(FarmdayMembershipProvider), new ProviderCapabilitySettings(0));
        }

        protected override void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();
            base.RegisterRoutes(routes);

            routes.MapRoute(name: "Login",
                url: "login/{action}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(name: "LogViewerPlugin",
                url: "custom-plugins/{controller}/{action}",
                defaults: new { controller = "LogViewerPlugin", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(name: "ChangeCustomer",
                url: "UserManagement/ChangeCustomer",
                defaults: new { controller = "UserManagement", action = "ChangeCustomer", id = UrlParameter.Optional });
        }
    }
}
