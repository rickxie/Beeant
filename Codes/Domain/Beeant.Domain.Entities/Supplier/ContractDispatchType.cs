using System;

namespace Beeant.Domain.Entities.Supplier
{
    /// <summary>
    /// 配送方式
    /// </summary>
    [Serializable]
    public enum ContractDispatchType
    {
        /// <summary>
        /// 入仓
        /// </summary>
        Warehous=1,

        /// <summary>
        /// 直送
        /// </summary>
        DirectDelivery=2
    }
}
