using System;

namespace Beeant.Domain.Entities.Order
{

    /// <summary>
    /// 流水账类型
    /// </summary>
    [Serializable]
    public enum OrderSettleType
    {
        /// <summary>
        /// 即时
        /// </summary>
        Immediately = 1,
        /// <summary>
        /// 月结
        /// </summary>
        Month = 2,
        /// <summary>
        /// T+1
        /// </summary>
        T1 = 4
    }
    
}
