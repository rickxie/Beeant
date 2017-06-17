using System.Collections.Generic;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Purchase;
using Winner.Persistence;
 

namespace Beeant.Domain.Services.Purchase
{
    public class PurchaseExpressDomainService : RealizeDomainService<PurchaseExpressEntity>
    {

        #region 加载
        private IDictionary<string, ItemLoader<PurchaseExpressEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<PurchaseExpressEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<PurchaseExpressEntity>>
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
        protected override bool SetBusiness(IList<IUnitofwork> unitofworks, PurchaseExpressEntity info)
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
        protected virtual void LoadDataEntity(PurchaseExpressEntity info)
        {
            if (info.SaveType == SaveType.Add) return;
            info.DataEntity = Repository.Get<PurchaseExpressEntity>(info.Id);

        }
        /// <summary>
        /// 加载账户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadPurchase(PurchaseExpressEntity info)
        {
            if (info.SaveType == SaveType.Add && info.Purchase != null && info.Purchase.Id != 0)
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



        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PurchaseExpressEntity info)
        {
            return ValidatePurchase(info);
        }
      
        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidatePurchase(PurchaseExpressEntity info)
        {
            if (!info.HasSaveProperty(it => it.Purchase.Id))
                return true;
            if (info.Purchase != null && info.Purchase.SaveType == SaveType.Add)
                return true;
            if (info.Purchase != null && info.Purchase.Id != 0)
            {
                if (Repository.Get<PurchaseEntity>(info.Purchase.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(PurchaseEntity).FullName, "NoExist");
            return false;
        }
   
        #endregion


    }
}
