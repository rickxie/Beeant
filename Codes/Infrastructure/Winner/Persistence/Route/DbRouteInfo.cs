using System;
using System.Collections.Generic;

namespace Winner.Persistence.Route
{
    public class DbRouteInfo
    {
        /// <summary>
        /// 查询数量
        /// </summary>
        public int TopCount { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否返回所以分片
        /// </summary>
        public bool IsReturnAllShardings { get; set; }

        /// <summary>
        /// 是否关联对象一起切表
        /// </summary>
        public bool IsMapTableAutoSharding { get; set; } = true;
        /// <summary>
        /// 存储名称
        /// </summary>
        public IList<RuleInfo> Rules { get; set; } 
        /// <summary>
        /// 分片
        /// </summary>
        public IList<ShardingInfo> Shardings { get; set; }
        /// <summary>
        /// 得到存储分片
        /// </summary>
        public Func<object, ShardingInfo> GetSaveShardingHandle { get; set; }
        /// <summary>
        /// 得到查询分片
        /// </summary>
        public Func<QueryInfo, IList<ShardingInfo>> GetQueryShardingHandle { get; set; }
    }
}
