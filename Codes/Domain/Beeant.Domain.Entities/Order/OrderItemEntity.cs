using System;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Order
{


    [Serializable]
    public class OrderItemEntity : BaseEntity<OrderItemEntity>
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
        /// 发票金额
        /// </summary>
        public decimal InvoiceAmount { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public OrderItemEntity DataEntity { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 设置添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
            SetAmount(null);
            SetOrderTotalPayAmount(Amount);
            SetOrderTotalInvoiceAmount(InvoiceAmount);
            SetOrderCostAmount(CostAmount);
        }



        /// <summary>
        /// 设置金额
        /// </summary>
        protected virtual void SetAmount(OrderItemEntity dataEntity)
        {
            if (!HasSaveProperty(it => it.Price) && !HasSaveProperty(it => it.Count) && !HasSaveProperty(it => it.Cost)) return;
            var price = !HasSaveProperty(it => it.Price) && dataEntity != null ? dataEntity.Price : Price;
            var count = !HasSaveProperty(it => it.Count) && dataEntity != null ? dataEntity.Count : Count;
            var cost = !HasSaveProperty(it => it.Cost) && dataEntity != null ? dataEntity.Cost : Cost;
            Amount = price * count;
            CostAmount =  cost * count;
            if (Properties == null) return;
            SetProperty(it => it.Amount);
            SetProperty(it => it.CostAmount);
        }

    

        /// <summary>
        /// 设置修改业务
        /// </summary>
        protected override void SetModifyBusiness()
        {
            InvokeItemLoader("DataEntity");
            if(DataEntity==null)
                return;
            SetAmount(DataEntity);
            if (HasSaveProperty(it => it.Amount) && Amount != DataEntity.Amount)
            {
                SetOrderTotalPayAmount(Amount - DataEntity.Amount);
            }
            if (HasSaveProperty(it => it.CostAmount) && CostAmount != DataEntity.CostAmount)
            {
                SetOrderCostAmount(CostAmount - DataEntity.CostAmount);
            }
            if (HasSaveProperty(it => it.InvoiceAmount) && InvoiceAmount != DataEntity.InvoiceAmount)
            {
                SetOrderTotalInvoiceAmount(InvoiceAmount - DataEntity.InvoiceAmount);
            }
        }

        /// <summary>
        /// 设置删除订单
        /// </summary>
        protected override void SetRemoveBusiness()
        {
            InvokeItemLoader("Order");
            if (Order == null || Order.SaveType == SaveType.Remove)
                return;
            InvokeItemLoader("DataEntity");
            if (DataEntity == null)
                return;
            SetOrderTotalPayAmount(0 - DataEntity.Amount);
            SetOrderTotalInvoiceAmount(0 - DataEntity.InvoiceAmount);
            SetOrderCostAmount(0-DataEntity.CostAmount);
        }

        /// <summary>
        /// 设置订单
        /// </summary>
        /// <param name="amount"></param>
        protected virtual void SetOrderTotalPayAmount(decimal amount)
        {
            InvokeItemLoader("Order");
            Order.TotalPayAmount += amount;
            if (Order.SaveType == SaveType.None)
            {
                Order.SetProperty(it => it.TotalPayAmount);
                Order.SaveType = SaveType.Modify;
            }
            else if (Order.Properties != null)
            {
                Order.SetProperty(it => it.TotalPayAmount);
            }
           
 

        }
        /// <summary>
        /// 设置订单开票金额
        /// </summary>
        /// <param name="amount"></param>
        protected virtual void SetOrderTotalInvoiceAmount(decimal amount)
        {
            InvokeItemLoader("Order");
            Order.TotalInvoiceAmount += amount;
            if (Order.SaveType == SaveType.None)
            {
                Order.SetProperty(it => it.TotalInvoiceAmount);
                Order.SaveType = SaveType.Modify;
            }
            else if (Order.Properties != null)
            {
                Order.SetProperty(it => it.TotalInvoiceAmount);
            }
        }
        /// <summary>
        /// 设置订单开票金额
        /// </summary>
        /// <param name="amount"></param>
        protected virtual void SetOrderCostAmount(decimal amount)
        {
            InvokeItemLoader("Order");
            Order.CostAmount += amount;
            if (Order.SaveType == SaveType.None)
            {
                Order.SetProperty(it => it.CostAmount);
                Order.SaveType = SaveType.Modify;
            }
            else if (Order.Properties != null)
            {
                Order.SetProperty(it => it.CostAmount);
            }
        }
    }
}
