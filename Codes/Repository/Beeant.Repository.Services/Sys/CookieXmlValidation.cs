using System.Collections.Generic;
using System.Web;
using Configuration;
using Winner.Filter;

namespace Beeant.Repository.Services.Sys
{
    public class CookieXmlValidation : XmlValidation
    {
        private const string LanguageKey = "language";
        private readonly string _domain = ConfigurationManager.GetSetting<string>("Domain");
        /// <summary>
        /// 得到验证信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override IList<ValidationInfo> GetValidations(string name)
        {
            var lang = GetLanguageType();
            if (string.IsNullOrWhiteSpace(lang))
                return base.GetValidations(name);
            var key = string.Format("{0}.{1}",name,lang);
            if (!Validations.ContainsKey(key))
                return base.GetValidations(name);
            return Validations[key];
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
