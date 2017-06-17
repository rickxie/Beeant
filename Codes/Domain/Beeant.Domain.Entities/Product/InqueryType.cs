using System;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public enum InqueryType
    {
        /// <summary>
        /// 商品咨询
        /// </summary>
        Goods=1,
        /// <summary>
        /// 库存及配送
        /// </summary>
        InventoryAndDelivery=2,
        /// <summary>
        /// 支付方式
        /// </summary>
        Payment=3,
        /// <summary>
        /// 发票及保修
        /// </summary>
        InvoiceAndRepair = 4,
        /// <summary>
        /// 其它
        /// </summary>
        Other=5,
    }
}
