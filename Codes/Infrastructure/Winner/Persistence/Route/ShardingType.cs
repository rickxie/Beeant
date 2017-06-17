using System;

namespace Winner.Persistence.Route
{
    [Serializable]
    public enum ShardingType
    {
        /// <summary>
        /// 固定
        /// </summary>
        Fixed = 1,
        /// <summary>
        /// 余数
        /// </summary>
        Remainder = 2,
        /// <summary>
        /// 值
        /// </summary>
        Value = 3,
        /// <summary>
        /// 小时
        /// </summary>
        Hour =4,
        /// <summary>
        /// 天
        /// </summary>
         Day=5,
         /// <summary>
         /// 月
         /// </summary>
         Month=6,
         /// <summary>
         /// 年 
         /// </summary>
         Year=7
    }
}
