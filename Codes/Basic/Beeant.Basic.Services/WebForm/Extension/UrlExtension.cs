using System.Web.UI;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class UrlExtension
    {



        /// <summary>
        /// 静态页面地址
        /// </summary>
        /// <param name="page"></param>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public static string GetUrl(this Page page, string siteName)
        {
            return Configuration.ConfigurationManager.GetSetting<string>(siteName);
        }

    }
}
