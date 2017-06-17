using System.Web.Http;

namespace Beeant.Presentation.Mobile.Buy
{
    public static class WebApiConfig
    {
        public static void Buy(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional }
            );
        }
    }
}
