using System.Web.Mvc;
using Component.Extension;

namespace Beeant.Cloud.Site
{
    public class SitePrinterAuthorizeFilterAttribute : SiteAuthorizeFilterAttribute
    {
        public override bool CheckFilter(ActionExecutingContext filterContext)
        {
            var siteId = filterContext.RouteData.Values["SiteId"].Convert<string>() ??
                         filterContext.HttpContext.Request["SiteId"].Convert<string>();
            var mark = filterContext.RouteData.Values["mark"].Convert<string>() ??
                       filterContext.HttpContext.Request["mark"].Convert<string>();
            var timespan = filterContext.RouteData.Values["timespan"].Convert<string>() ??
                           filterContext.HttpContext.Request["timespan"].Convert<string>();
            if (string.IsNullOrEmpty(siteId) || string.IsNullOrEmpty(mark) || string.IsNullOrEmpty(timespan))
            {
                var rev = base.CheckFilter(filterContext);
                if (rev)
                {
                    rev = filterContext.Controller.ViewBag.Site != null && filterContext.Controller.ViewBag.Site.IsPrint;
                }
                return rev;
            }
            else
            {
                var sign = Winner.Creator.Get<Winner.Base.ISecurity>().EncryptSign(timespan);
                var rev = sign.ToUpper().Equals(mark.ToUpper());
                if (rev)
                {
                    filterContext.Controller.ViewBag.SiteId = siteId.Convert<long>();
                }
                return rev;
            }
        }

         
    }
}
