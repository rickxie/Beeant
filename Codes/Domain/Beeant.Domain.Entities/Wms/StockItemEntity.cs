using System;
using Beeant.Domain.Entities.Management;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Wms
{
    /// <summary>
    /// 进出库
    /// </summary>
    [Serializable]
    public class StockItemEntity : BaseEntity<StockItemEntity>
    {
        /// <summary>
        /// 出库单据
        /// </summary>
        public StockEntity Stock { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public StorehouseEntity Storehouse { get; set; } 
        /// <summary>
        /// 相关产品
        /// </summary>
        public ProductEntity Product { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 锁定数量
        /// </summary>
        public int LockCount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public UserEntity User { get; set; }
   
        /// <summary>
        /// 库存商品
        /// </summary>
        public InventoryEntity Inventory { get; set; }
        /// <summary>
        /// 原始数据
        /// </summary>
        public StockItemEntity DataEntity { get; set; }


        /// <summary>
        /// 设置添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
            if (Stock == null) return;
            var count = IsCount() ? Count : 0;
            var lockCount = IsLockCount() ? Count : 0;
            if (count != 0 || lockCount != 0)
                SetInventory(count, lockCount);
            if (count != 0)
            {
                SetProduct(count);
            }
        }


        /// <summary>
        /// 设置修改业务
        /// </summary>
        protected override void SetModifyBusiness()
        {
            InvokeItemLoader("DataEntity");
            var count = 0;
            var lockCount = 0;
            InvokeItemLoader("Stock");
            InvokeItemLoader("Product");
            if (HasSaveProperty(it => it.Count) && DataEntity.Count != Count)
            {
                if (IsCount())
                {
                    count = Count - DataEntity.Count;
                }
                if (IsLockCount())
                {
                    lockCount = Count - DataEntity.Count;
                }
            }
            if (count != 0 || lockCount != 0)
                SetInventory(count, lockCount);
            if (count != 0)
            {
                SetProduct(count);
            }
        }


        /// <summary>
        /// 设置删除订单
        /// </summary>
        protected override void SetRemoveBusiness()
        {
            InvokeItemLoader("DataEntity");
            InvokeItemLoader("Stock");
            InvokeItemLoader("Product");
            var count = 0;
            var lockCount = 0;
            if (IsCount())
            {
                count = 0 - DataEntity.Count;
            }
            if (IsLockCount())
            {
                lockCount = 0 - DataEntity.Count;
            }
            if (count != 0 || lockCount != 0)
                SetInventory(count, lockCount);
            if (count != 0)
            {
                SetProduct(count);
            }

        }
        /// <summary>
        /// 是否改变库存
        /// </summary>
        public virtual bool IsCount()
        {
            return Stock != null && Stock.Status == StockStatusType.Finish;
        }
        /// <summary>
        ///是否锁定库存
        /// </summary>
        public virtual bool IsLockCount()
        {
            return Stock != null && Count < 0 && Stock.Status == StockStatusType.WaitAudit;
        }
        /// <summary>
        /// 设置产品
        /// </summary>
        public virtual void SetProduct(int count)
        {
            if(Product==null || Stock==null || Inventory==null || Inventory.Type==InvertoryType.Immediacy)
                return;
            if(Stock.Type==StockType.SalesIn || Stock.Type==StockType.SalesOut || Stock.Type==StockType.TransferIn || Stock.Type==StockType.TransferOut)
                return;
            Product.Count += count;
            Product.SetProperty(it => it.Count);
            Product.SaveType=SaveType.Modify;
        }
        /// <summary>
        /// 设置商品数量
        /// </summary>
        /// <param name="count"></param>
        /// <param name="lockCount"></param>
        public virtual void SetInventory(int count, int lockCount)
        {
            InvokeItemLoader("Inventory");
            if (Inventory == null)
            {
                Inventory = new InventoryEntity
                {
                    SaveType = SaveType.Add,
                    Product = Product,
                    Storehouse = Storehouse,
                    LockCount = 0,
                    TransitCount = 0,
                    Recycle = 0,
                    WarningCount = 0
                };
            }
            if (Inventory.SaveType == SaveType.None)
                Inventory.SaveType = SaveType.Modify;
            Inventory.Count += count;
            if (Inventory.SaveType == SaveType.Modify)
                Inventory.SetProperty(it => it.Count);
            Inventory.LockCount += lockCount;
            if (Inventory.SaveType == SaveType.Modify)
            {
                Inventory.SetProperty(it => it.LockCount);
            }
        }
   



    
   
    }
}
