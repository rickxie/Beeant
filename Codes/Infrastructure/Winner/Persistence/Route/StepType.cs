using System;

namespace Winner.Persistence.Route
{
    [Serializable]
    public enum StepType
    {
        /// <summary>
        /// 小时
        /// </summary>
        Value = 1,
        /// <summary>
        /// 小时
        /// </summary>
        Hour =2,
        /// <summary>
        /// 天
        /// </summary>
         Day=4,
         /// <summary>
         /// 月
         /// </summary>
         Month=8,
         /// <summary>
         /// 年 
         /// </summary>
         Year=16
    }
}
