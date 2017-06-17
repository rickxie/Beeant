using System;
using System.Web;
using System.Web.Caching;

namespace Winner.Cache
{
    public class LocalCache :  ICache
    {
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
        /// 得到对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual object Get(string key, Type type)
        {
            var value = HttpRuntime.Cache[key];
            if (value == null)
                return null;
            return Convert.ChangeType(value,type);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public virtual bool Set<T>(string key, T value, DateTime time)
        {
            if (value == null)
            {
                return false;
            }
            HttpRuntime.Cache.Insert(key, value, null, time, TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
            return true;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public virtual bool Set<T>(string key, T value, long timeSpan)
        {
            if (value==null)
            {
                return false;
            }
            HttpRuntime.Cache.Insert(key, value, null, DateTime.MaxValue, TimeSpan.FromSeconds(timeSpan), CacheItemPriority.NotRemovable, null);
            return true;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Set<T>(string key, T value)
        {
            if (value == null)
            {
                return false;
            }
            HttpRuntime.Cache[key] = value;
            return true;
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public virtual bool Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
            return true;
        }

        #endregion
    }
}
