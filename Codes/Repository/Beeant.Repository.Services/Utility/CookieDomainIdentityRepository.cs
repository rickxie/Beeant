using System;
using System.Web;
using Component.Extension;
using Configuration;
using Beeant.Domain.Entities.Utility;
using Winner.Base;

namespace Beeant.Repository.Services.Utility
{
    public class CookieDomainIdentityRepository : IdentityRepository
    {

        private readonly string _domain = ConfigurationManager.GetSetting<string>("Domain");
        /// <summary>
        /// 加密至
        /// </summary>
        private string DesKey
        {
            get { return ConfigurationManager.GetSetting<string>("Keys").DeserializeJson<dynamic>().Decrypt3Des.ToString(); }

        }
        public override TokenEntity Set<T>(T info) 
        {
            var token = new TokenEntity
                {
                    Ticket =GetTicket(info),
                    TimeOut = ConfigurationManager.GetSetting<int>("IdentityTimes")
                };
            var cookie = new HttpCookie(token.Ticket) { Domain = _domain, Secure = false, Expires = DateTime.Now.AddMinutes(token.TimeOut) };
            var cok = HttpContext.Current.Server.UrlEncode(info.SerializeJson());
            cookie.Value = Winner.Creator.Get<ISecurity>().Encrypt3Des(cok, DesKey);
            HttpContext.Current.Response.AppendCookie(cookie);
            return token;
        }

        public override T Get<T>(string ticket)  
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[ticket];
            if (cookie != null)
            {
                cookie.Domain = _domain;
                cookie.Secure = false;
                var value = Winner.Creator.Get<ISecurity>().Decrypt3Des(cookie.Value, DesKey);
                var info = HttpContext.Current.Server.UrlDecode(value);
                cookie.Expires = DateTime.Now.AddMinutes(ConfigurationManager.GetSetting<int>("IdentityTimes"));
                HttpContext.Current.Response.SetCookie(cookie);
                return info.DeserializeJson<T>();
            }
            return default(T);
        }

        public override bool Remove(string ticket)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[ticket];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1d);
                cookie.Domain = _domain;
                cookie.Secure = false;
                HttpContext.Current.Response.AppendCookie(cookie);
                return true;
            }
            return false;
        }
  

    }
}
