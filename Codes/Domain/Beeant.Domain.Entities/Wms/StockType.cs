using System;

namespace Beeant.Domain.Entities.Wms
{
    /// <summary>
    /// 出入库类型
    /// </summary>
    [Serializable]
    public enum StockType
    {
        /// <summary>
        /// 采购入库
        /// </summary>
        PurchaseIn = 1,
        /// <summary>
        /// 采购出库
        /// </summary>
        PurchaseOut = 2,
        /// <summary>
        /// 销售出库
        /// </summary>
        SalesOut = 3,
        /// <summary>
        /// 销售入库
        /// </summary>
        SalesIn = 4,
        /// <summary>
        /// 盘盈
        /// </summary>
        CheckIn = 5,
        /// <summary>
        /// 盘亏
        /// </summary>
        CheckOut = 6,
        /// <summary>
        /// 调拨入库
        /// </summary>
        TransferIn = 7,
        /// <summary>
        /// 调拨出库
        /// </summary>
        TransferOut = 8,
        /// <summary>
        /// 生产入库
        /// </summary>
        ProduceIn = 9,
        /// <summary>
        /// 生产出库
        /// </summary>
        ProduceOut = 10,
        /// <summary>
        /// 加工入库
        /// </summary>
        ProcessIn = 11,
        /// <summary>
        /// 加工出库
        /// </summary>
        ProcessOut = 12,
        /// <summary>
        /// 移位
        /// </summary>
        Move = 13,
        /// <summary>
        /// 其它
        /// </summary>
        Else = 14
    }
}
