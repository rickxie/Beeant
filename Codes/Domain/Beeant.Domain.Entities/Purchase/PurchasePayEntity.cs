using System;
using Beeant.Domain.Entities.Finance;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Purchase
{
    [Serializable]
    public class PurchasePayEntity : BaseEntity<PurchasePayEntity>
    {
        /// <summary>
        /// 付款记录
        /// </summary>
        public PayoutEntity Payout { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 采购单
        /// </summary>
        public PurchaseEntity Purchase { get; set; }
     
 
     
        /// <summary>
        /// 原始数据
        /// </summary>
        public PurchasePayEntity DataEntity { get; set; }
 
        /// <summary>
        /// 设置添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
            InvokeItemLoader("Purchase");
            SetPurchase(Amount);
        }

    

    


        /// <summary>
        /// 设置删除订单
        /// </summary>
        protected override void SetRemoveBusiness()
        {
            InvokeItemLoader("DataEntity");
            if (Purchase != null && Purchase.SaveType == SaveType.Remove) return;
            InvokeItemLoader("Purchase");
            SetPurchase(0 - DataEntity.Amount);
        }
        /// <summary>
        /// 设置订单
        /// </summary>
        /// <param name="amount"></param>
        protected virtual void SetPurchase(decimal amount)
        {
            if (Purchase == null || Purchase.SaveType == SaveType.Remove) return;
            Purchase.PayAmount += amount;
            if (Purchase.SaveType == SaveType.None)
            {
                Purchase.SaveType = SaveType.Modify;
                Purchase.SetProperty(it => it.PayAmount);
            }
            else if (Purchase.Properties != null)
                Purchase.SetProperty(it => it.PayAmount);
        }
       
    

    }
}
