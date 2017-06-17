using System;
using Beeant.Domain.Services.Utility;


namespace Beeant.Application.Services.Utility
{
    public class CacheApplicationService : ICacheApplicationService
    {
        public ICacheRepository CacheRepository { get; set; }

        /// <summary>
        /// 得到缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            return CacheRepository.Get<T>(key);
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
            return CacheRepository.Set(key, value, time);
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
            return CacheRepository.Set(key, value, timeSpan);
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool Remove(string key)
        {
            return CacheRepository.Remove(key);
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
            return CacheRepository.Set(key, value);
        }
    }
}
