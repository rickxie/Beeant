using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Application.Services;
using Dependent;
using Beeant.Application.Services.Sys;
using Beeant.Application.Services.Utility;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Agent;
using Beeant.Domain.Entities.Api;
using Beeant.Domain.Entities.Cms;
using Beeant.Domain.Entities.Hr;
using Beeant.Domain.Entities.Site;
using Beeant.Domain.Entities.Sys;
using Winner;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Basic.Services.Mvc.Bases
{
    public class SharedBaseController : Controller
    {
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="eventName"></param>
        public virtual void Event(string eventName)
        {
            var eventApplicationServices = GetEvents(eventName);
            Ioc.Resolve<IEventEngineApplicationService>().Execute(Request.Url.ToString(), Request["eventname"], eventApplicationServices);

        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual IList<IEventApplicationService> GetEvents(string name)
        {
            var query = new QueryInfo();
            query.Query<EventEntity>().Where(it => it.Name == name);
            var infos = Ioc.Resolve<IApplicationService>().GetEntities<EventEntity>(query);
            if (infos == null)
                return null;
            var eventApplicationServicese = new List<IEventApplicationService>();
            foreach (var info in infos)
            {
                eventApplicationServicese.Add(Ioc.Resolve<IEventApplicationService>(info.ClassName));
            }
            return eventApplicationServicese;
        }
        /// <summary>
        /// 返回错误信息
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Error()
        {
            var error = Creator.Get<IValidation>().GetErrorInfo(typeof(VerificationEntity).FullName, "99999");
            var result = new Dictionary<string, string> { { "Code", "99999" }, { "Message", error == null ? "99999" : error.Message } };
            return this.Jsonp(result);
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Quit(string url)
        {
            var loginUrl = "PresentationWebsiteLoginUrl";
            if (HttpContext.Request.Url != null &&
                (HttpContext.Request.Browser.IsMobileDevice || HttpContext.Request.Url.AbsoluteUri.Contains("m.")))
            {
                loginUrl = "PresentationMobileLoginUrl";
            }
            url = string.Format("{0}/home/quit?url={1}", this.GetUrl(loginUrl), url);
            return Redirect(url);
        }
    }
}
