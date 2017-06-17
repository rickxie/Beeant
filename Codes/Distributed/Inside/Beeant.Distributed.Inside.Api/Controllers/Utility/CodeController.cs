using System;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace Beeant.Distributed.Inside.Api.Controllers.Utility
{

    public class DepositController : Controller
    {
        //
        // GET: /Test/
        /// <summary>
        /// 得到票据
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetTicket()
        {
            var ticket=Guid.NewGuid().ToString();
            HttpRuntime.Cache.Insert(ticket, ticket, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero, CacheItemPriority.High, null);
            return Content(ticket);
        }
        // GET: /Test/
        /// <summary>
        /// 得到验证码
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public virtual ActionResult GetCode(string ticket)
        {
            var value = HttpRuntime.Cache[ticket];
            HttpRuntime.Cache.Remove(ticket);
            return Content((value != null).ToString().ToLower());
        }
    }
}
