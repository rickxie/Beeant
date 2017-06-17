using System.Web.Mvc;
using Configuration;
using Beeant.Basic.Services.Mvc.Result;

namespace Beeant.Basic.Services.Mvc.Extension
{
    static public class ActionResultControllerExtension
    {

   

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="controller"></param>
        public static ActionResult GetDefaultActionResult(this Controller controller)
        {
            return new RedirectResult(string.IsNullOrEmpty(controller.Request.QueryString["url"])
                                          ? ConfigurationManager.GetSetting<string>("DefaultPage")
                                          : controller.Server.UrlDecode(controller.Request.QueryString["url"]));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JsonpResult Jsonp(this Controller controller, object data)
        {
            JsonpResult result = new JsonpResult()
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return result;
        }
        /// <summary>
        /// 是否PC
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static bool IsPcBrowser(this Controller controller)
        {
            bool b = true;
            string agent = controller.Request.UserAgent;
            string[] agents = { "Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod" };
            for (int i = 0; i < agents.Length; i++)
            {
                if (agent.IndexOf(agents[i]) > -1)
                {
                    b = false;
                }
            }
            return b;
        }
    }
}
