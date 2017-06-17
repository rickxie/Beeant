using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beeant.Domain.Entities.Sys;
using Beeant.Domain.Services;
using Configuration;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Repository.Services.Sys
{
    public class CookieDbValidation : XmlValidation
    {
        private const string LanguageKey = "language";
        private readonly string _domain = ConfigurationManager.GetSetting<string>("Domain");
        public IRepository Repository { get; set; }
        protected  IDictionary<string,IDictionary<string,string>> CustomerNames { get; set; }

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object Locker = new object();

        public CookieDbValidation()
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
                CustomerNames=new Dictionary<string, IDictionary<string, string>>();
                foreach (var info in infos)
                {
                    var key = string.Format("{0}{1}", info.Key, info.Type);
                    if (!CustomerNames.ContainsKey(key))
                        CustomerNames.Add(info.Key,new Dictionary<string, string>());
                    if(!CustomerNames[info.Key].ContainsKey(info.Name))
                        CustomerNames[info.Key].Add(info.Name, info.Value);
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
            query.Query<LanguageEntity>().Where(it=>it.Tag== "Validation");
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
        /// 重新错误提示
        /// </summary>
        /// <param name="validInfo"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        protected override string GetMessage(ValidationInfo validInfo, RuleInfo rule)
        {
            var type = GetLanguageType();
            var key = string.Format("{0}{1}", validInfo.ModelName, type);
            if (CustomerNames != null && CustomerNames.ContainsKey(key))
            {
                var name = string.Format("{0}{1}", validInfo.PropertName, rule.Name);
                if (CustomerNames[key].ContainsKey(name) && CustomerNames[key][name] != null)
                    return CustomerNames[key][name];
                if (CustomerNames[key].ContainsKey(validInfo.PropertName) && CustomerNames[key][validInfo.PropertName] != null)
                    return CustomerNames[key][validInfo.PropertName];
            }
            return base.GetMessage(validInfo, rule);
        }
    }
}
