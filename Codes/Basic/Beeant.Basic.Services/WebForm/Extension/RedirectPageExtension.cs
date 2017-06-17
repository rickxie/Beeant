using System.Web.UI;
using Configuration;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class RedirectPageExtension
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="page"></param>
        public static void RedirectDefaultPage(this Page page)
        {
            Redirect(page,
                     string.IsNullOrEmpty(page.Request.QueryString["url"])
                         ? ConfigurationManager.GetSetting<string>("PresentationAdminDefaultUrl")
                         : page.Server.UrlDecode(page.Request.QueryString["url"]));
        }

        /// <summary>
        /// 验证失败
        /// </summary>
        /// <param name="page"></param>
        public static void RedirectLoginPage(this Page page)
        {
            Redirect(page, string.Format("{0}?url={1}", ConfigurationManager.GetSetting<string>("PresentationAdminLoginUrl"),
               page.Server.UrlEncode( page.Request.Url.AbsoluteUri)));
        }
        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="page"></param>
        /// <param name="url"></param>
        public static void Redirect(this Page page, string url)
        {
            try
            {
                page.Response.Redirect(url);
            }
            catch
            {
                page.Server.ClearError();
            }
        }
    }
}
