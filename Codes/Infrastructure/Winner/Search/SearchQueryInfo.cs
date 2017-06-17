using System;
using System.Collections.Generic;

namespace Winner.Search
{
    [Serializable]
    public class SearchQueryInfo
    {
        /// <summary>
        /// 检索词库
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 条件
        /// </summary>
        public IDictionary<string, object> Conditions { get; set; }
        /// <summary>
        /// 检索前多少条
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 缓存时间
        /// </summary>
        public long TimeSpan { get; set; }
        /// <summary>
        /// 缓存时间
        /// </summary>
        public DateTime? CecheTime { get; set; }
        /// <summary>
        /// 缓存Key
        /// </summary>
        public string CacheKey { get; set; }
    }
}
