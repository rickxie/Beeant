using System.Web.Mvc;
using System.Web.Routing;

namespace Beeant.Cloud.Site.Reception
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "site",
            url: "Home/Index/{siteid}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}