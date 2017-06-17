using System;
using System.Web;
using System.Web.Caching;
using Beeant.Domain.Services.Utility;

namespace Beeant.Repository.Services.Utility
{
    public class IdentityService : IIdentityContract
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="timeOut"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool Set(string ticket,int timeOut, string info)
        {
            HttpRuntime.Cache.Insert(ticket, info, null, DateTime.MaxValue, TimeSpan.FromSeconds(timeOut * 60), CacheItemPriority.High, null);
            return true;
          
        }

        /// <summary>
        /// 得到登入
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public virtual string Get(string ticket)
        {
            var value = HttpRuntime.Cache[ticket];
            if (value == null)
                return null;
            return (string)value;
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public virtual bool Remove(string ticket)
        {
            HttpRuntime.Cache.Remove(ticket);
            return true;
        }
   
    }
}
