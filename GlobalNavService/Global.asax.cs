using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GlobalNavService
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Version route
            routes.MapRoute(null, "version", new {controller = "Version", action = "Index"});

            // Healthcheck : ping
            routes.MapRoute(null, "healthcheck", new { controller = "HealthCheck", action = "HealthCheck" });
            routes.MapRoute(null, "healthcheck/DPPortalRead", new { controller = "HealthCheck", action = "DPPortalReadCheck" });
            routes.MapRoute(null, "healthcheck/ping", new { controller = "HealthCheck", action = "PingCheck" });

            // LinkName route
            routes.MapRoute(null, "{applicationName}/{linkName}/{navZeroId}", new { controller = "Navigation", action = "GetHeaderByName", navZeroId = UrlParameter.Optional });
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            log4net.Config.XmlConfigurator.Configure();
        }
    }
}