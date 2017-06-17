using System;

namespace Beeant.Domain.Entities.Purchase
{
    [Serializable]
    public enum PurchaseItemType
    {
        /// <summary>
        /// 产品
        /// </summary>
        Product = 1,
        /// <summary>
        /// 运费
        /// </summary>
        Express = 2,
        /// <summary>
        /// 抵扣
        /// </summary>
        Deduction=3,
        /// <summary>
        /// 其他
        /// </summary>
        Else = 4
    }

}
