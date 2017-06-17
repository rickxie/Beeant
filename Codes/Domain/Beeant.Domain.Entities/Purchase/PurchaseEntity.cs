using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Wms;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Purchase
{
    [Serializable]
    public class PurchaseEntity : BaseEntity<PurchaseEntity>
    {
        /// <summary>
        /// 订单
        /// </summary>
        public OrderEntity Order { get; set; }
        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime PurchaseDate { get; set; }
        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime DeliveryDate { get; set; }
        /// <summary>
        /// 应付
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 已开发票金额
        /// </summary>
        public decimal OpenAmount { get; set; }
        /// <summary>
        /// 已开发票金额
        /// </summary>
        public decimal InvoiceAmount { get; set; }
     
        /// <summary>
        /// 实付
        /// </summary>
        public decimal PayAmount { get; set; }
 
        /// <summary>
        /// 是否为冲
        /// </summary>
        public PurchaseType Type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 原始采购单
        /// </summary>
        public PurchaseEntity OriginalPurchase { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public StorehouseEntity Storehouse { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 付款核销记录
        /// </summary>
        public IList<PurchasePayEntity> PurchasePays { get; set; }
        /// <summary>
        /// 订单发票
        /// </summary>
        public IList<PurchaseInvoiceEntity> PurchaseInvoices { get; set; }
        /// <summary>
        /// 采购明细
        /// </summary>
        public IList<PurchaseItemEntity> PurchaseItems { get; set; }
        /// <summary>
        /// 快递信息
        /// </summary>
        public IList<PurchaseExpressEntity> PurchaseExpresses { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public IList<PurchaseAttachmentEntity> PurchaseAttachments { get; set; }
        /// <summary>
        /// 库存清单
        /// </summary>
        public IList<StockEntity> Stocks { get; set; } 
        /// <summary>
        /// 采购单类型
        /// </summary>
        public string TypeName
        {
            get { return Type.GetName(); }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public PurchaseStatusType Status { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName
        {
            get { return Status.GetName(); }
        }
        #region 设置业务代码
        /// <summary>
        /// 原始数据
        /// </summary>
        public PurchaseEntity DataEntity { get; set; }
 

        /// <summary>
        /// 设置添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
            SetPurchaseItems(Status == PurchaseStatusType.WaitSign,  SaveType.Add);
            if (PurchasePays != null)
            {
                foreach (var purchasePay in PurchasePays)
                {
                    purchasePay.SaveType = SaveType.Add;
                }
            }
        }

        /// <summary>
        /// 设置修改业务
        /// </summary>
        protected override void SetModifyBusiness()
        {
            InvokeItemLoader("DataEntity");
            if (HasSaveProperty(it => it.Status) && DataEntity.Status != Status && DataEntity.Type!=PurchaseType.Sales)
            {
                bool? isTransitCount=null;
                if (DataEntity.Status != PurchaseStatusType.WaitSign && Status == PurchaseStatusType.WaitSign)
                {
                    isTransitCount = true;
                }
                else if (DataEntity.Status == PurchaseStatusType.WaitSign && Status != PurchaseStatusType.WaitSign)
                {
                    isTransitCount = false;
                }
                SetPurchaseItems(isTransitCount,SaveType.Modify);
            }
        }

        /// <summary>
        /// 设置删除订单
        /// </summary>
        protected override void SetRemoveBusiness()
        {
            var isTransitCount = DataEntity.Status != PurchaseStatusType.WaitSign && Status == PurchaseStatusType.WaitSign;
            SetPurchaseItems(isTransitCount, SaveType.Remove);
            InvokeItemLoader("PurchasePays");
            if (PurchasePays != null)
            {
                foreach (var purchasePay in PurchasePays)
                {
                    purchasePay.SaveType = SaveType.Remove;
                }
                if (PurchaseInvoices != null)
                {
                    foreach (var purchaseInvoice in PurchaseInvoices)
                    {
                        purchaseInvoice.Purchase = this;
                        purchaseInvoice.SaveType = SaveType.Remove;
                    }
                }
            }
        }

        /// <summary>
        /// 设置商品数量
        /// </summary>
        protected virtual void SetPurchaseItems(bool? isTransitCount, SaveType saveType)
        {
            InvokeItemLoader("PurchaseItems");
            if (PurchaseItems == null)
                return;
            foreach (var purchaseItem in PurchaseItems)
            {
                purchaseItem.Purchase = this;
                if (saveType == SaveType.Remove)
                {
                    purchaseItem.SaveType = SaveType.Remove;
                }
                if (purchaseItem.SaveType == SaveType.Add)
                    continue;
                if (isTransitCount.HasValue && purchaseItem.Product!=null && purchaseItem.Product.Id>0)
                {
                    purchaseItem.SetInventory(isTransitCount.Value ? purchaseItem.Count : 0 - purchaseItem.Count);
                }
             
            }
        }




        #endregion


        #region 转换出入库

        /// <summary>
        /// 生产出入库
        /// </summary>
        /// <returns></returns>
        public virtual StockEntity CreateStocks()
        {
            if (PurchaseItems == null || PurchaseItems.Count(it => it.Product != null && it.Product.Id != 0) == 0)
                return null;
            var info = new StockEntity { Purchase = this, StockItems = new List<StockItemEntity>(), SaveType = SaveType.Add };
            foreach (var purchaseItem in PurchaseItems.Where(it => it.Product != null && it.Product.Id != 0))
            {
                var stockItem = new StockItemEntity
                {
                    Count = purchaseItem.Count,
                    Stock = info,
                    Product = purchaseItem.Product,
                    Storehouse = Storehouse,
                    Name = purchaseItem.Name,
                    SaveType = SaveType.Add
                };
                info.StockItems.Add(stockItem);
            }

            return info;
        }


        #endregion
    }
}
