using System;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Wms;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Purchase
{


    [Serializable]
    public class PurchaseItemEntity : BaseEntity<PurchaseItemEntity>
    {
        /// <summary>
        /// 采购单
        /// </summary>
        public PurchaseEntity Purchase { get; set; }
        /// <summary>
        /// 库存商品编号
        /// </summary>
        public ProductEntity Product { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 是否开票
        /// </summary>
        public bool IsOpen { get; set; }
 
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 库存商品
        /// </summary>
        public InventoryEntity Inventory { get; set; }
        /// <summary>
        /// 明细
        /// </summary>
        public PurchaseItemEntity DataEntity { get; set; }
    
 

        /// <summary>
        /// 设置金额
        /// </summary>
        protected virtual void SetAmount(PurchaseItemEntity dataEntity)
        {
            if (!HasSaveProperty(it => it.Price) && !HasSaveProperty(it => it.Count)) return;
            var price = !HasSaveProperty(it => it.Price) && dataEntity != null ? dataEntity.Price : Price;
            var count = !HasSaveProperty(it => it.Count) && dataEntity != null ? dataEntity.Count : Count;
            Amount = price * count;
            if (Properties == null) return;
            SetProperty(it => it.Amount);
        }

        /// <summary>
        /// 设置添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
            SetAmount(null);
            SetPurchase(Amount);
            if (IsTransitCount())
            {
                SetInventory(0 - Count);
            }

        }

        /// <summary>
        /// 设置修改业务
        /// </summary>
        protected override void SetModifyBusiness()
        {
            InvokeItemLoader("DataEntity");
            if (DataEntity==null)
                return;
            if (DataEntity != null)
                Product = DataEntity.Product;
            SetAmount(DataEntity);
            if (HasSaveProperty(it => it.Count) || HasSaveProperty(it => it.Price))
            {
                SetPurchase(Amount - DataEntity.Amount); 
            }
            if (HasSaveProperty(it => it.Count) && Count != DataEntity.Count)
            {
                InvokeItemLoader("Purchase");
                if (IsTransitCount())
                    SetInventory(Count - DataEntity.Count);
            }
        }


        /// <summary>
        /// 设置删除订单
        /// </summary>
        protected override void SetRemoveBusiness()
        {
            InvokeItemLoader("DataEntity");
            if(DataEntity==null)
                return;
            if (DataEntity != null)
                Product = DataEntity.Product;
            SetPurchase(0 - DataEntity.Amount);
            var count = IsTransitCount() ? DataEntity.Count : 0;
            SetInventory(count);
        }

        /// <summary>
        /// 是否改变库存
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTransitCount()
        {
            return Purchase != null && Purchase.Status == PurchaseStatusType.WaitSign && Purchase.Type != PurchaseType.Return;
        }

  
        /// <summary>
        /// 设置订单
        /// </summary>
        /// <param name="amount"></param>
        protected virtual void SetPurchase(decimal amount)
        {
            if (Purchase != null && Purchase.SaveType == SaveType.Remove) return;
            InvokeItemLoader("Purchase");
            if (Purchase ==null || Purchase.SaveType == SaveType.Remove) return;
            Purchase.TotalAmount += amount;
            SetPurchaseOpenAmount(amount);
            if (Purchase.SaveType == SaveType.None)
            {
                Purchase.SaveType = SaveType.Modify;
                Purchase.SetProperty(it => it.TotalAmount);
            }
            else if (Purchase.Properties != null)
                Purchase.SetProperty(it => it.TotalAmount);
        }

        /// <summary>
        /// 设置订单开票金额
        /// </summary>
        /// <param name="amount"></param>
        protected virtual void SetPurchaseOpenAmount(decimal amount)
        {
            if (SaveType != SaveType.Add || SaveType != SaveType.Remove)
                return;
            if (IsOpen)
            {
                Purchase.OpenAmount += amount;
                if (Purchase.SaveType == SaveType.None)
                {
                    Purchase.SetProperty(it => it.OpenAmount);
                    Purchase.SaveType = SaveType.Modify;
                }
                else if (Purchase.Properties != null)
                {
                    Purchase.SetProperty(it => it.OpenAmount);
                }
            }
        }


        /// <summary>
        /// 设置商品
        /// </summary>
        /// <param name="count"></param>
        public virtual void SetInventory(int count)
        {
            InvokeItemLoader("Inventory");
            if (Inventory == null || Inventory.Id == 0)
            {
                if(Purchase==null || Purchase.Storehouse==null || Product==null || Product.Id==0)
                    return;
                Inventory = new InventoryEntity
                {
                    SaveType = SaveType.Add,
                    Product = Product,
                    Storehouse = Purchase.Storehouse,
                    LockCount = 0,
                    TransitCount = count,
                    Recycle = 0,
                    WarningCount = 0
                };
                return;
            }
            Inventory.TransitCount += count;
            if (Product.SaveType == SaveType.None)
            {
                Inventory.SetProperty(it => it.TransitCount);
                Inventory.SaveType = SaveType.Modify;
            }
            else if (Inventory.Properties != null)
            {
                Inventory.SetProperty(it => it.TransitCount);
            }
         
        }
    }
    
}
