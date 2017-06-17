using System;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Utility;

namespace Beeant.Repository.Services.Utility
{
    public class IdentityRepository : IIdentityRepository
    {
        /// <summary>
        /// 得到票据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual string GetTicket(IdentityEntity info)
        {
           return Guid.NewGuid().ToString().Replace("-","");
        }
        public virtual TokenEntity Set<T>(T info) where T : IdentityEntity
        {

            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
            {
                var token = new TokenEntity
                {
                    Ticket = GetTicket(info),
                    TimeOut = System.Web.HttpContext.Current.Session.Timeout
                };
                System.Web.HttpContext.Current.Session[token.Ticket] = info;
                return token;
            }
            return null;
        }

        public virtual T Get<T>(string ticket) where T : IdentityEntity
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session!=null)
            {
                return (T)System.Web.HttpContext.Current.Session[ticket];
            }
            return default(T);
        }

        public virtual bool Remove(string ticket)
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
            {
                System.Web.HttpContext.Current.Session[ticket] = null;
                return true;
            }
            return false;
        }
    }
}
