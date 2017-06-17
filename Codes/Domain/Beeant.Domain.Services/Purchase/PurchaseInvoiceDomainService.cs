using System.Collections.Generic;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Purchase;
using Winner.Persistence;

namespace Beeant.Domain.Services.Purchase
{
    public class PurchaseInvoiceDomainService : RealizeDomainService<PurchaseInvoiceEntity>
    {


        #region 重写事务
   
        private IDictionary<string, ItemLoader<PurchaseInvoiceEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<PurchaseInvoiceEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<PurchaseInvoiceEntity>>
                    {
                        {"Purchase", LoadPurchase},
                        {"DataEntity", LoadDataEntity}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

        /// <summary>
        /// 重写
        /// </summary>
        /// <param name="unitofworks"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool SetBusiness(IList<IUnitofwork> unitofworks, PurchaseInvoiceEntity info)
        {
            var rev = base.SetBusiness(unitofworks, info);
            if (info.Purchase != null && info.Purchase.SaveType != SaveType.None)
                unitofworks.AddList(Repository.Save(info.Purchase));
            return rev;
        }


        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual void LoadDataEntity(PurchaseInvoiceEntity info)
        {
            if(info.SaveType==SaveType.Add)return;
            info.DataEntity = Repository.Get<PurchaseInvoiceEntity>(info.Id);

        }
        /// <summary>
        /// 加载账户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadPurchase(PurchaseInvoiceEntity info)
        {
            if (info.SaveType==SaveType.Add && info.Purchase != null && info.Purchase.Id != 0)
            {
                info.Purchase = info.Purchase.SaveType == SaveType.Add ? info.Purchase : Repository.Get<PurchaseEntity>(info.Purchase.Id);
            }
            else
            {
                LoadDataEntity(info);
                if (info.DataEntity != null && info.DataEntity.Purchase != null)
                {
                    info.Purchase = Repository.Get<PurchaseEntity>(info.DataEntity.Purchase.Id);
                }
            }

        }
     
        #endregion

        #region 重写验证

        #region 添加

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PurchaseInvoiceEntity info)
        {
            PurchaseEntity purchase;
            var rev = ValidatePurchase(info, out purchase);
            if (!rev) return false;
            rev = ValidatePurchase(info, purchase) && ValidatePurchaseReceiptAmount(info, null, purchase);
            return rev;
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <param name="purchase"></param>
        /// <returns></returns>
        protected virtual bool ValidatePurchase(PurchaseInvoiceEntity info, out PurchaseEntity purchase)
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
        protected virtual bool ValidatePurchase(PurchaseInvoiceEntity info, PurchaseEntity purchase)
        {
            if (purchase == null)
            {
                info.AddErrorByName(typeof(PurchaseEntity).FullName, "NoExist");
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
        protected virtual bool ValidatePurchaseReceiptAmount(PurchaseInvoiceEntity info, PurchaseInvoiceEntity dataEntity, PurchaseEntity purchase)
        {
            if (purchase == null) return true;
            if (!info.HasSaveProperty(it => it.Amount))
                return true;
            if (purchase.OpenAmount < purchase.InvoiceAmount + info.Amount)
            {
                info.AddErrorByName(typeof(PurchaseEntity).FullName, "AmountLessReceiptAmount");
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
        protected override bool ValidateModify(PurchaseInvoiceEntity info)
        {
            var dataEntity = Repository.Get<PurchaseInvoiceEntity>(info.Id);
            if (dataEntity == null) return false;
            var order = Repository.Get<PurchaseEntity>(dataEntity.Purchase.Id);
            var rev = ValidatePurchaseReceiptAmount(info, dataEntity, order);
            return rev;
        }

        #endregion

    
        #endregion

 

    }
}
