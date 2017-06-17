using System.Web.Mvc;
using System.Web.Routing;

namespace Beeant.Cloud.Crm.Mobile
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Default", "{controller}/{action}/{CrmId}", new { controller = "Home", action = "Index", CrmId = UrlParameter.Optional }
            );
           
        }
    }
}