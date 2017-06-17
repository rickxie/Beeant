using System;
using Beeant.Domain.Services.Utility;
using Winner.Cache;

namespace Beeant.Repository.Services.Utility
{
    public class CacheRepository : ICacheRepository
    {
       
        public virtual ICache CacheServer
        {
            get { return Winner.Creator.Get<ICache>(); }
        }
        /// <summary>
        /// 得到缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            try
            {
                return CacheServer.Get<T>(key);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return default(T);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public virtual bool Set<T>(string key, T value, DateTime time)
        {
            try
            {
                if (value == null)
                    return false;
                return CacheServer.Set(key, value, time);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return false;
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Set<T>(string key, T value)
        {
            try
            {
                if (value == null)
                    return false;
                return CacheServer.Set(key, value);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return false;
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public virtual bool Set<T>(string key, T value, long timeSpan)
        {
            try
            {
                if (value == null)
                    return false;
                return CacheServer.Set(key, value, timeSpan);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return false;
           
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool Remove(string key)
        {
            try
            {
                return CacheServer.Remove(key);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return false;
   
        }
    }
}
