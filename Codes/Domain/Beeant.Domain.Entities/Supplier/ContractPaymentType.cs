using System;

namespace Beeant.Domain.Entities.Supplier
{
    /// <summary>
    /// 支付方式
    /// </summary>
    [Serializable]
    public enum ContractPaymentType
    {
        /// <summary>
        /// 现金
        /// </summary>
        Cash=1,

        /// <summary>
        /// 支票
        /// </summary>
        Check=2,

        /// <summary>
        /// 转账
        /// </summary>
        Transfer=3,

        /// <summary>
        /// 汇票
        /// </summary>
        Draft=4
    }
}
