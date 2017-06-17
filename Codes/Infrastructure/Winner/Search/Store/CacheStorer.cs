using System;
using Winner.Cache;

namespace Winner.Search.Store
{
    public class CacheStorer : FileStorer
    {
        #region 变量
        private string _cacheTag = "Winner_SearchSourchCache_";
        /// <summary>
        /// 缓存前缀
        /// </summary>
        public virtual string CacheTag
        {
            get { return _cacheTag; }
            set { _cacheTag = value; }
        }
        #endregion

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
        public CacheStorer()
        {
        }
        /// <summary>
        /// 路径，缓存实例
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cache"></param>
        public CacheStorer(string path, ICache cache)
            :base(path)
        {
            Cache = cache;
        }
        #endregion

        #region 接口重写

        /// <summary>
        /// 从缓存中读取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public override T Read<T>(string name)
        {
            var key = GetCacheKey(name);
            var value = Cache.Get<T>(key);
            if (value == null)
            {
                value = base.Read<T>(name);
                if (value != null)
                {
                    Cache.Set(key, value, DateTime.MaxValue);
                }
            }
            return value;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="name"></param>
        public override void Flush(string name)
        {
            var value = base.Read<object>(name);
            if (value == null) return;
            var key = GetCacheKey(name);
            Cache.Set(key, value, DateTime.MaxValue);
        }
        /// <summary>
        /// 得到缓存名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual string GetCacheKey(string name)
        {
            var value = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(name, "MD5");
            return string.Format("{0}{1}", CacheTag, value);
        }

        #endregion



    }
}
