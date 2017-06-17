using System;

namespace Winner.Cache
{
    public class CacheService :  ICacheContract
    {
        #region 属性

        /// <summary>
        /// 缓存实例
        /// </summary>
        public ICache Cache { get; set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public CacheService()
        { }

        /// <summary>
        /// WCF配置文路径件,缓存实例
        /// </summary>
        /// <param name="cache"></param>
        public CacheService(ICache cache)
        {
            Cache = cache;
        }
        #endregion 

        #region 接口的实现

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public virtual bool SetByTime(string key, object value, DateTime time)
        {
            return Cache.Set(key, value, time);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public virtual bool SetByTimeSpan(string key, object value, long timeSpan)
        {
            return Cache.Set(key, value, timeSpan);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Set(string key, object value)
        {
            return Cache.Set(key, value);
        }


        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual object Get(string key)
        {
            return Cache.Get<object>(key);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool Remove(string key)
        {
            return Cache.Remove(key);
        }
      



        #endregion



    }
}
