using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beeant.Domain.Entities.Sys;
using Beeant.Domain.Services;
using Configuration;
using Winner.Dislan;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Repository.Services.Sys
{
    public class CookieDbLanguage : XmlLanguage
    {
        private const string LanguageKey = "language";
        private readonly string _domain = ConfigurationManager.GetSetting<string>("Domain");
        public IRepository Repository { get; set; }
        protected  IDictionary<string,IDictionary<string,LanguageInfo>> CustomerNames { get; set; }

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object Locker = new object();

        public CookieDbLanguage()
        {
            if (CustomerNames == null)
            {
                LoadCustomerNames();
            }
        }
        /// <summary>
        /// 得到自定义
        /// </summary>
        protected  void LoadCustomerNames()
        {
            lock (Locker)
            {
                if(CustomerNames!=null)
                    return;
                var infos = GetLanguages();
                if(infos==null)
                    return;
                CustomerNames=new Dictionary<string, IDictionary<string, LanguageInfo>>();
                foreach (var info in infos)
                {
                    var key = string.Format("{0}{1}", info.Key, info.Type);
                    if (!CustomerNames.ContainsKey(key))
                        CustomerNames.Add(info.Key,new Dictionary<string, LanguageInfo>());
                    if(!CustomerNames[info.Key].ContainsKey(info.Name))
                        CustomerNames[info.Key].Add(info.Name, new LanguageInfo {Name = info.Name, Message = info.Value });
                }
            }
        }
        /// <summary>
        /// 得到语音包
        /// </summary>
        /// <returns></returns>
        protected virtual IList<LanguageEntity> GetLanguages()
        {
            var query = new QueryInfo();
            query.Query<LanguageEntity>().Where(it=>it.Tag== "Language");
            var infos = Repository.GetEntities<LanguageEntity>(query);
            return infos;
        }

        /// <summary>
        /// 得到语言版本
        /// </summary>
        /// <returns></returns>
        protected virtual string GetLanguageType()
        {
            if (HttpContext.Current == null)
                return null;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[LanguageKey];
            if (cookie != null)
            {
                cookie.Domain = _domain;
                cookie.Secure = false;
                cookie.Path = "/";
                var type = HttpContext.Current.Server.UrlDecode(cookie.Value);
                return type;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string GetName(string key, string name)
        {
            var type = GetLanguageType();
            key = string.Format("{0}{1}", key, type);
            if (CustomerNames!=null && CustomerNames.ContainsKey(key))
            {
               if (CustomerNames[key].ContainsKey(name) && CustomerNames[key][name]!=null)
                    return CustomerNames[key][name].Message;
            }
            return base.GetName(key, name);
        }
        /// <summary>
        /// 得到接到名称
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override IList<LanguageInfo> GetNames(string key)
        {
            var type = GetLanguageType();
            key = string.Format("{0}{1}", key, type);
            if (CustomerNames != null && CustomerNames.ContainsKey(key))
            {
                return CustomerNames[key].Values.ToList();
            }
            return base.GetNames(key);
        }
    }
}
