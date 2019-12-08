using System.Web.Mvc;
using System.Web.Routing;

namespace FileHub
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                namespaces: new string[] { "FileHub.Controllers" },
                url: "{controller}/{action}",
                defaults: new { controller = "Files", action = "FileList" }
            );
        }
    }
}
