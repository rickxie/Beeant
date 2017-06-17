using System;
using Beeant.Domain.Entities.Purchase;
using Winner.Persistence;
using PayEntity = Beeant.Domain.Entities.Purchase.PurchasePayEntity;

namespace Beeant.Domain.Entities.Finance
{


    [Serializable]
    public class InvoiceinItemEntity : BaseEntity<InvoiceinItemEntity>
    {
        /// <summary>
        /// 发票
        /// </summary>
        public InvoiceinEntity Invoicein { get; set; }
        /// <summary>
        /// 订单
        /// </summary>
        public PurchaseEntity Purchase { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
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
        /// 数据
        /// </summary>
        public PayEntity DataEntity { get; set; }




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
            if (Purchase != null && Purchase.SaveType ==SaveType.Remove) return;
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
                Purchase.SetProperty(it => it.PayAmount);
                Purchase.SaveType = SaveType.Modify;
            }
            else if (Purchase.Properties != null)
            {
                Purchase.SetProperty(it => it.PayAmount);
            }
        }


    }
}
