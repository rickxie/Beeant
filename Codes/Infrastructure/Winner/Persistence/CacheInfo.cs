using System;

namespace Winner.Persistence
{
    public class CacheInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 缓存值
        /// </summary>
        public virtual string Key { get; set; }
        /// <summary>
        /// 设置查询缓存
        /// </summary>
        public virtual DateTime Time { get; set; }
        /// <summary>
        /// 设置缓存时间
        /// </summary>
        public virtual long TimeSpan { get; set; }
     
    }
}
