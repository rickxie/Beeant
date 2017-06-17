using System;
using System.Collections.Generic;
using Component.Extension;
using Configuration;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Utility;
using Winner.Wcf;

namespace Beeant.Repository.Services.Utility
{
    public class RemoteIdentityRepository : IdentityRepository
    {
 
        public IWcfService WcfService { get; set; }

        /// <summary>
        /// 存储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public override TokenEntity Set<T>(T info) 
        {
            try
            {
                var token = new TokenEntity
                {
                    Ticket = GetTicket(info),
                    TimeOut = ConfigurationManager.GetSetting<int>("IdentityTimes")
                };
                WcfService.Invoke<IIdentityContract>(Set, GetEndPoints, token.Ticket, token.TimeOut, info);
                return token;
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return null;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override T Get<T>(string ticket)
        {
            try
            {
                var value = WcfService.Invoke<IIdentityContract>(Get, GetEndPoints, ticket).Convert<string>();
                return value.DeserializeJson<T>();
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return default(T);
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <returns></returns>
        public override bool Remove(string ticket)
        {
            try
            {
                var rev = WcfService.Invoke<IIdentityContract>(Remove, GetEndPoints, ticket);
                if (rev == null)
                    return false;
                return (bool)rev;
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return false;
        }

        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="service"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public virtual object Set(IIdentityContract service, params object[] paramters)
        {
            return service.Set(paramters[0].Convert<string>(),paramters[1].Convert<int>(),paramters[2].SerializeJson()).Convert<bool>();
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="service"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public virtual object Get(IIdentityContract service, params object[] paramters)
        {
            return service.Get(paramters[0] as string);
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="service"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public virtual object Remove(IIdentityContract service, params object[] paramters)
        {
            var key = paramters[0] as string;
            if (string.IsNullOrEmpty(key))
                return null;
            return service.Remove(key);
        }
        /// <summary>
        /// 得到节点
        /// </summary>
        /// <param name="endPoints"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public virtual IList<EndPointInfo> GetEndPoints(IList<EndPointInfo> endPoints, params object[] paramters)
        {
            if (endPoints == null || endPoints.Count == 0 || paramters==null || paramters.Length==0 || paramters[0]==null) 
                return endPoints;
            var hash = Math.Abs(paramters[0].Convert<string>().GetHashCode()) % endPoints.Count;
            return new List<EndPointInfo> { endPoints[hash] };
        }
       
    }
}
