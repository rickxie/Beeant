using System;
using System.Web;
using Component.Extension;
using Configuration;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Utility;
using Winner.Base;

namespace Beeant.Application.Services.Utility
{
    public class IdentityApplicationService : IIdentityApplicationService
    {
        public IIdentityRepository IdentityRepository { get; set; }
        /// <summary>
        /// 加密至
        /// </summary>
        private static string DesKey
        {
            get { return ConfigurationManager.GetSetting<string>("Keys").DeserializeJson<dynamic>().Decrypt3Des.ToString(); }

        }
        private const string TicketKey = "ticket";
        private const string TimeOutKey = "utime";
        private readonly string _domain = ConfigurationManager.GetSetting<string>("Domain");
        public virtual TokenEntity Set<T>(T info) where T : IdentityEntity
        {
            try
            {
                Remove();
                var token= IdentityRepository.Set(info);
                if (token != null)
                {
                    SetToken(token);
                    SetTokenTimeOut(token);
                }
                return token;
            }
            catch (Exception)
            {

                return null;
            }
       
        }

        public virtual T Get<T>() where T : IdentityEntity
        {
            try
            {
                var ticket = GetTokenTicket();
                if (string.IsNullOrEmpty(ticket))
                    return default(T);
                ResetToken();
                ResetTokenTimeOut();
                return IdentityRepository.Get<T>(ticket);
            }
            catch (Exception)
            {

                return default(T);
            }
        }

        public virtual bool Remove()
        {
            try
            {
                var rev = IdentityRepository.Remove(GetTokenTicket());
                RemoveToken();
                RemoveTokenTimeOut();
                return rev;
            }
            catch (Exception)
            {
                return false;
            }

        }
        /// <summary>
        /// 得到登陆信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public virtual T Get<T>(string ticket) where T : IdentityEntity
        {
            ticket = Winner.Creator.Get<ISecurity>().Decrypt3Des(ticket, DesKey);
            if (string.IsNullOrEmpty(ticket))
                return default(T);
            return IdentityRepository.Get<T>(ticket);
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public virtual bool Remove(string ticket)
        {
            try
            {
                var rev = IdentityRepository.Remove(ticket);
                return rev;
            }
            catch (Exception)
            {
                return false;
            }
        }
 

        /// <summary>
        /// 设置token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual bool SetToken(TokenEntity token)
        {
            if (token==null || HttpContext.Current==null)
                return false;
            var cookie = new HttpCookie(TicketKey) { Domain = _domain, Expires = DateTime.Now.AddMinutes(token.TimeOut), Path = "/", Secure = false };
            var key = Winner.Creator.Get<ISecurity>().Encrypt3Des(token.Ticket, DesKey);
            cookie.Value = HttpContext.Current.Server.UrlEncode(key);
            HttpContext.Current.Response.AppendCookie(cookie);
            return true;
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public virtual TokenEntity GetToken()
        {
            var ticket = GetTokenTicket();
            if (string.IsNullOrEmpty(ticket))
                return null;
            if (IdentityRepository.Get<IdentityEntity>(ticket) == null)
            {
                RemoveToken();
                RemoveTokenTimeOut();
                return null;
            }
            if (HttpContext.Current == null)
                return null;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[TicketKey];
            if (cookie != null)
            {
                var token = new TokenEntity { TimeOut = GetTokenTimeOut(), Ticket = cookie.Value };
                return token;
            }
            return null;
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public virtual void ResetToken()
        {
            if (HttpContext.Current == null)
                return ;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[TicketKey];
            if (cookie != null)
            {
                HttpContext.Current.Response.AppendCookie(cookie);
                cookie.Expires = DateTime.Now.AddMinutes(GetTokenTimeOut());
                cookie.Secure = false;
                cookie.Path = "/";
                cookie.Domain = _domain;
                HttpContext.Current.Response.SetCookie(cookie);
            }

        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public virtual string GetTokenTicket()
        {
            if (HttpContext.Current == null)
                return null;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[TicketKey];
            if (cookie != null)
            {
                cookie.Domain = _domain;
                cookie.Secure = false;
                cookie.Path = "/";
                var tickit = HttpContext.Current.Server.UrlDecode(cookie.Value);
                return Winner.Creator.Get<ISecurity>().Decrypt3Des(tickit, DesKey);
            }
            return null;
        }
        /// <summary>
        /// 移除token
        /// </summary>
        /// <returns></returns>
        public virtual bool RemoveToken()
        {
            if (HttpContext.Current == null)
                return false;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[TicketKey];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1d);
                cookie.Secure = false;
                cookie.Path = "/";
                cookie.Domain = _domain;
                HttpContext.Current.Response.SetCookie(cookie);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 设置token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual bool SetTokenTimeOut(TokenEntity token)
        {
            if (token == null)
                return false;
            if (HttpContext.Current == null)
                return false;
            var cookie = new HttpCookie(TimeOutKey)
                {
                    Domain=_domain,
                    Expires = DateTime.Now.AddMinutes(token.TimeOut),
                    Path = "/",
                    Secure = false,
                    Value = HttpContext.Current.Server.UrlEncode(token.TimeOut.ToString())
                };
            HttpContext.Current.Response.AppendCookie(cookie);
            return true;
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        protected virtual int GetTokenTimeOut()
        {
            if (HttpContext.Current == null)
                return 0;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[TimeOutKey];
            if (cookie != null)
            {
                cookie.Domain = _domain;
                cookie.Secure = false;
                cookie.Path = "/";
                var timeout = HttpContext.Current.Server.UrlDecode(cookie.Value);
                return timeout.Convert<int>();
            }
            return 0;
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        protected virtual void ResetTokenTimeOut()
        {
            if (HttpContext.Current == null)
                return ;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[TimeOutKey];
            if (cookie != null)
            {
                cookie.Secure = false;
                cookie.Path = "/";
                cookie.Domain = _domain;
                HttpContext.Current.Response.SetCookie(cookie);
            }
        }
        /// <summary>
        /// 移除token
        /// </summary>
        /// <returns></returns>
        protected virtual bool RemoveTokenTimeOut()
        {
            if (HttpContext.Current == null)
                return false;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[TimeOutKey];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1d);
                cookie.Secure = false;
                cookie.Path = "/";
                cookie.Domain = _domain;
                HttpContext.Current.Response.SetCookie(cookie);
                return true;
            }
            return false;
        }

       
    }
}
