using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Application.Services;
using Dependent;
using Beeant.Application.Services.Sys;
using Beeant.Domain.Entities.Sys;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Basic.Services.WebForm.Pages
{
    public  class EventPageBase : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var eventApplicationServices = GetEvents(Request["eventname"]);
            Ioc.Resolve<IEventEngineApplicationService>().Execute(Request.Url.ToString(),Request["eventname"], eventApplicationServices);
       
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
            var infos= Ioc.Resolve<IApplicationService>().GetEntities<EventEntity>(query);
            if (infos == null)
                return null;
            var eventApplicationServicese = new List<IEventApplicationService>();
            foreach (var info in infos)
            {
                eventApplicationServicese.Add(Ioc.Resolve<IEventApplicationService>(info.ClassName));
            }
            return eventApplicationServicese;
        }
    }
   
}
