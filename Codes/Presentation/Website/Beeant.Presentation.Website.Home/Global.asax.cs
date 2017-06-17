using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Basic.Services.Mvc.Extension;

namespace Beeant.Presentation.Website.Home
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            this.Initialize();
            //RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes); 
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                this.AddExceptionLog();
            }
            finally
            {

            }
        }
    }
}