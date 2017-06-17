using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Utility;
using Component.Extension;
using Winner;
using Winner.Log;


namespace Beeant.Repository.Services.Utility
{
    public class IpRepository : IIpRepository
    {

         
        /// <summary>
        /// 得到IP信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public virtual IpEntity Get(string ip)
        {
            var info=new IpEntity {Ip=ip,Country="",Province="",City=""};
            try
            {
                if (string.IsNullOrEmpty(ip))
                    return info;
                var url = string.Format("{0}{1}", Configuration.ConfigurationManager.GetSetting<string>("SinaIp"), ip);
                var json = Creator.Get<Winner.Base.IComponent>().GetPageContent(url, "utf-8").DeserializeJson<IDictionary<string, string>>();
                if (json != null)
                {
                    info.Country = json.ContainsKey("country") ? "" : json["country"];
                    info.Province = json.ContainsKey("province") ? "" : json["province"];
                    info.City = json.ContainsKey("city") ? "" : json["city"];
                }
            }
            catch (Exception ex)
            {
                Creator.Get<ILog>().AddException(ex);
            }
            return info;
        }
    }
}
