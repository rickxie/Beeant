using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Winner.Storage.Cache
{
    public class LocalCache :  ICache
    {
        private IList<CacheInfo> _caches=new List<CacheInfo>();
        /// <summary>
        /// 缓存对象
        /// </summary>
        public IList<CacheInfo> Caches
        {
            get { return _caches; }
            set { _caches = value; }
        }
        #region 接口的实现
        /// <summary>
        /// 得到缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            var value = HttpRuntime.Cache[key];
            if (value == null)
                return default(T);
            return (T)value;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Set<T>(string key, T value)
        {
            var cache = Caches.FirstOrDefault(it => key.Contains(it.Path));
            if (cache == null) return false;
            HttpRuntime.Cache.Insert(key, value, null, DateTime.Now.AddMinutes(cache.Times), TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
            return true;
        }
      
        #endregion
    }
}
