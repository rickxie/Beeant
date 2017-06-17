using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Purchase;
using Winner.Persistence;

namespace Beeant.Domain.Services.Purchase
{
    public class PurchasePayDomainService : RealizeDomainService<PurchasePayEntity>
    {
     

        #region 重写验证

        #region 添加

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PurchasePayEntity info)
        {
            PurchaseEntity purchase;
            var rev = ValidatePurchase(info, out purchase);
            if (!rev) return false;
            rev = ValidatePurchase(info, purchase) && ValidatePurchasePayAmount(info, null, purchase) && ValidatePayable(info);
            return rev;
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <param name="purchase"></param>
        /// <returns></returns>
        protected virtual bool ValidatePurchase(PurchasePayEntity info, out PurchaseEntity purchase)
        {
            purchase = info.Purchase;
            if (purchase != null && purchase.SaveType != SaveType.Add)
                purchase = Repository.Get<PurchaseEntity>(info.Purchase.Id);
            if (purchase == null)
            {
                info.AddErrorByName(typeof (PurchaseEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <param name="purchase"></param>
        /// <returns></returns>
        protected virtual bool ValidatePurchase(PurchasePayEntity info, PurchaseEntity purchase)
        {
            if (purchase == null)
            {
                info.AddErrorByName(typeof(PurchaseEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证收款
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidatePayable(PurchasePayEntity info)
        {
            if (!info.HasSaveProperty(it => it.Payout.Id))
                return true;
            var receivable = info.Payout;
            if(receivable!=null && receivable.SaveType != SaveType.Add)
                receivable = Repository.Get<PayoutEntity>(info.Payout.Id);
            if (receivable == null)
            {
                info.AddErrorByName(typeof(PayoutEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证实收金额
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <param name="purchase"></param>
        /// <returns></returns>
        protected virtual bool ValidatePurchasePayAmount(PurchasePayEntity info, PurchasePayEntity dataEntity, PurchaseEntity purchase)
        {
            if (purchase == null) return true;
            if (!info.HasSaveProperty(it => it.Amount))
                return true;
            if (dataEntity != null && dataEntity.Amount == info.Amount)
                return true;
            var amount = info.Amount;
            if (dataEntity != null)
                amount = amount - dataEntity.Amount;
            if (purchase.TotalAmount < purchase.PayAmount + amount)
            {
                info.AddErrorByName(typeof(PurchaseEntity).FullName, "AmountLessPaidAmount");
                return false;
            }
            return true;
        }
        #endregion

        #region 验证修改
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(PurchasePayEntity info)
        {
            var dataEntity = Repository.Get<PurchasePayEntity>(info.Id);
            if (dataEntity == null) return false;
            var order = Repository.Get<PurchaseEntity>(dataEntity.Purchase.Id);
            var rev = ValidatePurchasePayAmount(info, dataEntity, order);
            return rev;
        }
    
        #endregion



        #endregion


    }
}
