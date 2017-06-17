using System;
using System.Text;
using System.Web;
using Component.Extension;
using Configuration;
using Winner.Base;

namespace Beeant.Basic.Services.Common.Extension
{
    static public class CodeHelper
    {
   

        #region 客户端验证码
        /// <summary>
        /// 加密至
        /// </summary>
        private static string CodeKey
        {
            get { return ConfigurationManager.GetSetting<string>("Keys").DeserializeJson<dynamic>().Decrypt3Des.ToString(); }

        }


        /// <summary>
        /// 得到验证码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void CreateCode(string name)
        {
            var code = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < 4; i++)
            {
                code.Append(random.Next(0, 9));
            }
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(name, Winner.Creator.Get<ISecurity>().Encrypt3Des(code.ToString(), CodeKey)));
            HttpContext.Current.Response.ContentType = "image/jpeg";
            HttpContext.Current.Response.BinaryWrite(Winner.Creator.Get<IComponent>().CreateCodeImage(code.ToString()));
        }


        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ValidateCode(string code, string name)
        {
            var cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null) return false;
            var value = Winner.Creator.Get<ISecurity>().Decrypt3Des(cookie.Value, CodeKey);
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.AppendCookie(cookie);
            return value == code;
        }

        /// <summary>
        /// 初始化验证码
        /// </summary>
        /// <param name="name"></param>
        public static void InitilzeCodeErrorCount(string name)
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(name, Winner.Creator.Get<ISecurity>().Encrypt3Des("0", CodeKey)));
        }

        /// <summary>
        /// 初始化验证码
        /// </summary>
        /// <param name="name"></param>
        public static void AddCodeErrorCount( string name)
        {
            var value = GetErrorCount(name);
            if (value == null)
                return;
            value = value + 1;
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(name, Winner.Creator.Get<ISecurity>().Encrypt3Des(value.ToString(), CodeKey)));
        }

        /// <summary>
        /// 初始化验证码
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveCodeErrorCount( string name)
        {
            var cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
                return;
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 得到错误数量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int? GetErrorCount(string name)
        {
            var cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
                return null;
            var value= Winner.Creator.Get<ISecurity>().Decrypt3Des(cookie.Value, CodeKey);
            int i;
            if (int.TryParse(value, out i))
                return i;
            return null;
        }
        #endregion

    }
}
