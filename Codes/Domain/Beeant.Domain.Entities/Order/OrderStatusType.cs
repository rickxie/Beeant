using System;

namespace Beeant.Domain.Entities.Order
{
    [Serializable]
    public enum OrderStatusType
    {
        /// <summary>
        /// 交易取消
        /// </summary>
        Cancel = 1,
        /// <summary>
        /// 交易完成
        /// </summary>
        Finish = 2,
        /// <summary>
        /// 等待处理
        /// </summary>
        WaitHandle = 3,
        /// <summary>
        /// 等待支付
        /// </summary>
        WaitPay = 4,
        /// <summary>
        /// 等待发货
        /// </summary>
        WaitDelivery=5,
        /// <summary>
        /// 等待签收
        /// </summary>
        WaitSign = 6
    }

}
