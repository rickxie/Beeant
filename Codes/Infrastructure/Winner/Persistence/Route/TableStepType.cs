using System;

namespace Winner.Persistence.Route
{
    [Serializable]
    public enum TableStepType
    {
        /// <summary>
        /// 数值
        /// </summary>
        Value = 1,
        /// <summary>
        /// 小时
        /// </summary>
        Hour =2,
        /// <summary>
        /// 天
        /// </summary>
         Day=3,
         /// <summary>
         /// 月
         /// </summary>
         Month=4,
         /// <summary>
         /// 年 
         /// </summary>
         Year=5
    }
}
