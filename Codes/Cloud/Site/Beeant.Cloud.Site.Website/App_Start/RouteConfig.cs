using System.Web.Mvc;
using System.Web.Routing;

namespace Beeant.Cloud.Site.Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Default", "{controller}/{action}/{SiteId}", new { controller = "Home", action = "Index", SiteId = UrlParameter.Optional }
            );
           
        }
    }
}