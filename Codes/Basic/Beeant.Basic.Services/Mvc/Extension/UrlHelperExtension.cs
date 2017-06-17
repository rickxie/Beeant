using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Beeant.Basic.Services.Mvc.Extension
{
    public static class UrlHelperExtension
    {
       
        /// <summary>
        /// 静态页面地址
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public static string GetUrl(this HtmlHelper htmlHelper,string siteName)
        {
            return Configuration.ConfigurationManager.GetSetting<string>(siteName);
        }
        /// <summary>
        /// 静态页面地址
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public static string GetUrl(this Controller controller, string siteName)
        {
            return Configuration.ConfigurationManager.GetSetting<string>(siteName);
        }

        /// <summary>
        /// 得到登录信息
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HtmlString CreatePartnerUrl(this HtmlHelper htmlHelper,string url,string name="pid")
        {
              var value = htmlHelper.ViewContext.RequestContext.RouteData.Values[name] ??
                        htmlHelper.ViewContext.RequestContext.HttpContext.Request.QueryString[name];
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return new HtmlString(url);
            var flag = "?";
            if (url.Contains("?"))
                flag = "&";
            return new HtmlString(string.Format("{0}{1}{2}={3}", url, flag, name, value));
            
        }

        /// <summary>
        /// 得到地址
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetUrl(this HtmlHelper htmlHelper, string name,string value)
        {
            var pageLinkValueDictionary = new RouteValueDictionary(new RouteValueDictionary());
            foreach (var routeDataValue in htmlHelper.ViewContext.RequestContext.RouteData.Values)
            {
                if (!pageLinkValueDictionary.ContainsKey(routeDataValue.Key))
                {
                    pageLinkValueDictionary.Add(routeDataValue.Key, routeDataValue.Value);
                }
            }
            foreach (var qs in htmlHelper.ViewContext.RequestContext.HttpContext.Request.QueryString.AllKeys)
            {
                if (!pageLinkValueDictionary.ContainsKey(qs))
                {
                    pageLinkValueDictionary.Add(qs,
                                                htmlHelper.ViewContext.RequestContext.HttpContext.Request.QueryString[qs
                                                    ]);
                }
            }
            if (pageLinkValueDictionary.ContainsKey(name))
                pageLinkValueDictionary.Remove(name);
            if (!pageLinkValueDictionary.ContainsKey(name))
                pageLinkValueDictionary.Add(name, value);
            var virtualPathForArea = RouteTable.Routes.GetVirtualPathForArea(htmlHelper.ViewContext.RequestContext,
                                                                             pageLinkValueDictionary);
            if (virtualPathForArea == null) return null;
            return virtualPathForArea.VirtualPath;
        }

      
    }
}
