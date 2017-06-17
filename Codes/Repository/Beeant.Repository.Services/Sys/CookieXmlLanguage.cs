using System.Web;
using Configuration;
using Winner.Dislan;

namespace Beeant.Repository.Services.Sys
{
    public class CookieXmlLanguage : XmlLanguage
    {
        private const string LanguageKey = "language";
        private readonly string _domain = ConfigurationManager.GetSetting<string>("Domain");
        /// <summary>
        /// 重写语言包
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string GetName(string key, string name)
        {
            var lang = GetLanguageType();
            if (string.IsNullOrWhiteSpace(lang))
                return base.GetName(key, name);
            var langkey = string.Format("{0}.{1}", key, lang);
            var value = base.GetName(langkey, name);
            if (!string.IsNullOrWhiteSpace(value))
                return value;
            return base.GetName(key, name);
        }

        /// <summary>
        /// 得到语言版本
        /// </summary>
        /// <returns></returns>
        protected virtual string GetLanguageType()
        {
            if (HttpContext.Current == null)
                return null;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[LanguageKey];
            if (cookie != null)
            {
                cookie.Domain = _domain;
                cookie.Secure = false;
                cookie.Path = "/";
                var type = HttpContext.Current.Server.UrlDecode(cookie.Value);
                return type;
            }
            return null;
        }
    }
}
