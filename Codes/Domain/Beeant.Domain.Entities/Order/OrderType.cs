using System;

namespace Beeant.Domain.Entities.Order
{
    [Serializable]
    public enum OrderType
    {
        /// <summary>
        /// 标准
        /// </summary>
        Standard = 1,
        /// <summary>
        /// 定制
        /// </summary>
        Custom = 2,
        /// <summary>
        /// 退换货
        /// </summary>
        Return = 3,
        /// <summary>
        /// 积分订单
        /// </summary>
        Integral = 4
    }

}
