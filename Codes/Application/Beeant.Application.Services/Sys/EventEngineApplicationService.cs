using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Component.Extension;
using Beeant.Domain.Entities.Sys;
using Beeant.Domain.Services;
using Configuration;
using Winner.Creation;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Sys
{
    public class EventEngineApplicationService : IEventEngineApplicationService
    {
        /// <summary>
        /// 存储实例
        /// </summary>
        public virtual IRepository Repository { get; set; }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public virtual void Trigger(string name, IDictionary<string, string> args)
        {
            if(string.IsNullOrWhiteSpace(name))
                return;
            Action<string, IDictionary<string, string>> func = BeginTrigger;
            func.BeginInvoke(name, args,null, null);
           
        }

        /// <summary>
        /// 开始出发
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        protected virtual void BeginTrigger(string name, IDictionary<string, string> args)
        {
            var infos = GetTriggerEntities(name);
            if (infos == null)
                return;
            Trigger(infos, args);
        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <returns></returns>
        protected virtual IList<EventEntity> GetTriggerEntities(string name)
        {
            var query=new QueryInfo();
            query.Query<EventEntity>().Where(it => it.Name == name);
            var infos = Repository.GetEntities<EventEntity>(query);
            return infos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="args"></param>
        public virtual void Trigger(IList<EventEntity> infos, IDictionary<string, string> args)
        {

            try
            {
                var timespan = DateTime.Now.Ticks.ToString();
                var mark = Winner.Creator.Get<Winner.Base.ISecurity>().EncryptSign(timespan);
                foreach (var info in infos)
                {
                    if (string.IsNullOrEmpty(info.Url))
                        continue;
                    var ul = info.Url.ToLower();
                    if (!ul.StartsWith("http"))
                    {
                        var index = info.Url.IndexOf("/");
                        var key = info.Url.Substring(0, index);
                        ul = string.Format("{0}{1}", ConfigurationManager.GetSetting<string>(key), ul.Substring(index));
                    }
                    var url =
                        new StringBuilder(string.Format("{0}{1}eventname={2}&timespan={3}&mark={4}", ul,
                            info.Url.Contains("?") ? "&" : "?",
                            info.Name, timespan, mark));

                    if (args != null && args.Count > 0)
                    {
                        foreach (var arg in args)
                        {
                            url.AppendFormat("&{0}={1}", arg.Key, arg.Value);
                        }
                    }
                    WebRequestHelper.SendPostRequest(url.ToString(), null);
                }
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);

            }

        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="eventApplicationServicese"></param>
        /// <returns></returns>
        public virtual void Execute(string url, string name, IList<IEventApplicationService> eventApplicationServicese)
        {
            if(eventApplicationServicese==null || eventApplicationServicese.Count==0)
                return;
            Action<string,string, IList<IEventApplicationService>> func = BeginExecute;
            func.BeginInvoke(url,name, eventApplicationServicese, null, null);
        }

        ///  <summary>
        /// 异步执行
        ///  </summary>
        ///  <param name="url"></param>
        ///  <param name="name"></param>
        /// <param name="eventApplicationServices"></param>
        protected virtual void BeginExecute(string url, string name, IList<IEventApplicationService> eventApplicationServices)
        {
            var timespan = HttpUtility.ParseQueryString(url).Get("timespan");
            var mark = HttpUtility.ParseQueryString(url).Get("mark");
            if (string.IsNullOrEmpty(mark) || string.IsNullOrEmpty(timespan))
                return;
            var mk = Winner.Creator.Get<Winner.Base.ISecurity>().EncryptSign(timespan);
            if (mark.ToLower() != mk.ToLower())
                return;
            foreach (var eventApplicationService in eventApplicationServices)
            {
                eventApplicationService.Execute(url, name);
            }
          
        }
    
    }
}
