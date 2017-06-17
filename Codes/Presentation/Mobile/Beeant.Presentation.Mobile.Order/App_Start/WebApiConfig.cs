using System.Web.Http;

namespace Beeant.Presentation.Mobile.Order
{
    public static class WebApiConfig
    {
        public static void Order(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional }
            );
        }
    }
}
