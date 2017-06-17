using System;

namespace Beeant.Domain.Entities.Purchase
{
    [Serializable]
    public enum PurchaseType
    {
        /// <summary>
        /// 采购入库
        /// </summary>
        Store = 1,
        /// <summary>
        /// 销售采购
        /// </summary>
        Sales = 2,
        /// <summary>
        /// 退换货
        /// </summary>
        Return = 3
    }

}
