using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Purchase;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Wms
{
    /// <summary>
    /// 进出库
    /// </summary>
    [Serializable]
    public class StockEntity : BaseEntity<StockEntity>
    {
        /// <summary>
        /// 相关采购单
        /// </summary>
        public PurchaseEntity Purchase { get; set; }
        /// <summary>
        /// 相关订单
        /// </summary>
        public OrderEntity Order { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public StockType Type { get; set; }
        /// <summary>
        /// 是否为冲
        /// </summary>
        public bool IsFlush { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 出库单明细
        /// </summary>
        public IList<StockItemEntity> StockItems { get; set; }
        /// <summary>
        /// 是否为冲
        /// </summary>
        public string IsFlushName
        {
            get { return this.GetStatusName(IsFlush); }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string TypeName
        {
            get { return Type.GetName(); }
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        public StockEntity DataEntity { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StockStatusType Status { get; set; }

        /// <summary>
        /// 设置添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
          
        }


        /// <summary>
        /// 设置修改业务
        /// </summary>
        protected override void SetModifyBusiness()
        {
            InvokeItemLoader("DataEntity");
            if (HasSaveProperty(it => it.Status) && DataEntity.Status != Status)
            {
                InvokeItemLoader("StockItems");
                if (StockItems == null)
                    return;
                foreach (var stockItem in StockItems)
                {
                    if (stockItem.SaveType == SaveType.Add)
                        continue;
                    var count = 0;
                    var lockCount = 0;
                    if (DataEntity.Status != StockStatusType.Finish && Status == StockStatusType.Finish)
                    {
                        count = stockItem.Count;
                    }
                    else if (DataEntity.Status == StockStatusType.Finish && Status != StockStatusType.Finish)
                    {
                        count = 0 - stockItem.Count;
                    }
                    if (stockItem.Count < 0)
                    {
                        if (DataEntity.Status != StockStatusType.Finish && Status == StockStatusType.WaitAudit)
                        {
                            lockCount = stockItem.Count;
                        }
                        else if (DataEntity.Status == StockStatusType.WaitAudit && Status != StockStatusType.WaitAudit)
                        {
                            lockCount = 0 - stockItem.Count;
                        }
                    }
                    stockItem.SetInventory(count, lockCount);
                    if (count > 0)
                    {
                        stockItem.SetProduct(count);
                    }
                }
            }
        }
      

        /// <summary>
        /// 设置删除订单
        /// </summary>
        protected override void SetRemoveBusiness()
        {
            InvokeItemLoader("DataEntity");
            InvokeItemLoader("StockItems");
            if (StockItems == null)
                return;
            foreach (var stockItem in StockItems)
            {
                stockItem.SaveType = SaveType.Remove;
            }
        }

     

    
    }
}
