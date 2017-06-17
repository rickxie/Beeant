using System.Web;

namespace Beeant.Basic.Services.Common.Extension
{
    static public class HttpContextHelper
    {

   
         
        /// <summary>
        /// 得到客户端地址
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {
            if (HttpContext.Current == null)
                return null;
            return HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null ?
                HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] :
                HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}
