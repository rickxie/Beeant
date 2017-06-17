using System.Web.Http;

namespace Beeant.Presentation.Mobile.Member
{
    public static class WebApiConfig
    {
        public static void Member(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional }
            );
        }
    }
}
