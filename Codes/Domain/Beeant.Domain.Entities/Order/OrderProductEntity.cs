using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Promotion;
using Component.Extension;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Order
{
    public class OrderProductEntity : BaseEntity<OrderProductEntity>
    {
        /// <summary>
        /// 订单
        /// </summary>
        public OrderEntity Order { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 成本
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 成本金额
        /// </summary>
        public decimal CostAmount { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否改变库存
        /// </summary>
        public bool IsCount { get; set; }
        /// <summary>
        /// 是否开发票
        /// </summary>
        public bool IsInvoice { get; set; }
        /// <summary>
        /// 是否支持退货
        /// </summary>
        public bool IsReturn { get; set; }
        /// <summary>
        /// 是否已经点评
        /// </summary>
        public bool IsAppraisement { get; set; }
        /// <summary>
        /// 商品
        /// </summary>
        public ProductEntity Product { get; set; }
        /// <summary>
        /// 活动
        /// </summary>
        public PromotionEntity Promotion { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        public string FullFileName
        {
            get { return this.GetFullFileName(FileName); }
        }
        /// <summary>
        /// 图片流
        /// </summary>
        public byte[] FileByte { get; set; }
  
        /// <summary>
        /// 是否改变库存
        /// </summary>
        public string IsCountName
        {
            get { return this.GetStatusName(IsCount); }
        }
        /// <summary>
        /// 是否支持退货
        /// </summary>
        public string IsReturnName
        {
            get
            {
                return this.GetStatusName(IsReturn);
            }
        }
        /// <summary>
        /// 是否开发票
        /// </summary>
        public string IsInvoiceName
        {
            get
            {
                return this.GetStatusName(IsInvoice);
            }
        }
        /// <summary>
        /// 是否点评
        /// </summary>
        public string IsAppraisementName
        {
            get
            {
                return this.GetStatusName(IsAppraisement);
            }
        }
        /// <summary>
        /// 数据
        /// </summary>
        public OrderProductEntity DataEntity { get; set; }

        /// <summary>
        /// 设置添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
            SetAmount(null);
            SetFileName();
            InvokeItemLoader("Product");
            SetOrderTotalAmount(Amount);
            SetIsCount();
            if (IsCount)
            {
                SetProduct(0 - Count);
            }
            CreateOrderItems(Product.Price,Product.Cost,Count);
        }
        /// <summary>
        /// 设置成本
        /// </summary>
        protected virtual void CreateOrderItems(decimal price,decimal cost, int count)
        {
            Order.OrderItems = Order.OrderItems ?? new List<OrderItemEntity>();
            var orderItem = new OrderItemEntity
            {
                Count = count,
                Price = price,
                Cost= cost,
                InvoiceAmount= price*count,
                Name = Product.Name,
                Order = Order,
                Remark="",
                SaveType = SaveType.Add
            };
            Order.OrderItems.Add(orderItem);
        }
   
        /// <summary>
        /// 设置文件名
        /// </summary>
        protected virtual void SetFileName()
        {
            if (!string.IsNullOrEmpty(FileName))
                return;
            if (Properties == null) return;
            SetProperty(it => it.FileName);
        }

        /// <summary>
        /// 设置金额
        /// </summary>
        protected virtual void SetAmount(OrderProductEntity dataEntity)
        {
            if (!HasSaveProperty(it => it.Price) && !HasSaveProperty(it => it.Count)) return;
            var price = !HasSaveProperty(it => it.Price) && dataEntity != null ? dataEntity.Price : Price;
            var count = !HasSaveProperty(it => it.Count) && dataEntity != null ? dataEntity.Count : Count;
            var cost = !HasSaveProperty(it => it.Cost) && dataEntity != null ? dataEntity.Cost : Cost;
            Amount = price * count;
            CostAmount = cost*count;
            if (Properties == null) return;
            SetProperty(it => it.Amount);
            SetProperty(it => it.CostAmount);
        }

        /// <summary>
        /// 设置IsAccount
        /// </summary>
        public virtual void SetIsCount()
        {
            if (Product == null || Product.Goods == null || Order == null)
                return;
            IsCount = Product.Goods.UnusedStatusArray == null || !Product.Goods.UnusedStatusArray.Contains(Order.Status.Convert<int>());
            if (Properties == null) return;
            SetProperty(it => it.IsCount);
        }

        /// <summary>
        /// 设置商品
        /// </summary>
        /// <param name="count"></param>
        protected virtual void SetProduct(int count)
        {

            if (Product == null || Product.Id == 0) return;
            if (Order == null || Order.Type != OrderType.Standard) return;
            Product.Count += count;
            Product.ChangeCount = count;
            if (Product.SaveType == SaveType.None)
            {
                Product.SetProperty(it => it.Count).SetProperty(it => it.ChangeCount);
                Product.SaveType = SaveType.Modify;
            }
            else if (Product.Properties != null)
            {
                Product.SetProperty(it => it.Count).SetProperty(it => it.ChangeCount);
            }
        }

        /// <summary>
        /// 设置修改业务
        /// </summary>
        protected override void SetModifyBusiness()
        {
            InvokeItemLoader("DataEntity");
            SetAmount(DataEntity);
            SetFileName();
            Name = HasSaveProperty(it => it.Name) ? Name : DataEntity.Name;
            if (HasSaveProperty(it => it.Amount) && Amount != DataEntity.Amount)
            {
                SetOrderTotalAmount(Amount - DataEntity.Amount);
                CreateOrderItems(Amount - DataEntity.Amount,CostAmount-DataEntity.CostAmount,1);
            }
            if (HasSaveProperty(it => it.IsCount) && IsCount != DataEntity.IsCount)
            {
                InvokeItemLoader("Order");
                InvokeItemLoader("Product");
                SetProduct(IsCount ? 0 - Count : DataEntity.Count);
            }
            else if (HasSaveProperty(it => it.Count) && Count != DataEntity.Count && DataEntity.IsCount)
            {
                InvokeItemLoader("Product");
                SetProduct(DataEntity.Count - Count);
            }

        }

        /// <summary>
        /// 设置删除订单
        /// </summary>
        protected override void SetRemoveBusiness()
        {
            InvokeItemLoader("DataEntity");
            SetOrderTotalAmount(0 - DataEntity.Amount);
            var count = DataEntity.IsCount ? DataEntity.Count : 0;
            SetProduct(count);
            CreateOrderItems(0- DataEntity.Price, 0 - DataEntity.CostAmount, DataEntity.Count);
        }

        /// <summary>
        /// 设置订单
        /// </summary>
        /// <param name="amount"></param>
        protected virtual void SetOrderTotalAmount(decimal amount)
        {
            if (Order != null && Order.SaveType == SaveType.Remove) return;
            InvokeItemLoader("Order");
            if (Order == null || Order.SaveType == SaveType.Remove) return;
            Order.TotalAmount += amount;
            if (Order.SaveType == SaveType.None)
            {
                Order.SetProperty(it => it.TotalAmount);
                Order.SaveType = SaveType.Modify;
            }
            else if (Order.Properties != null)
            {
                Order.SetProperty(it => it.TotalAmount);
            }
            if (Product.DepositRate > 0)
            {
                Order.Deposit += amount*Product.DepositRate;
                if (Order.SaveType == SaveType.None)
                {
                    Order.SetProperty(it => it.Deposit);
                    Order.SaveType = SaveType.Modify;
                }
                else if (Order.Properties != null)
                {
                    Order.SetProperty(it => it.Deposit);
                }
            }
           

        }
   
    }
}
