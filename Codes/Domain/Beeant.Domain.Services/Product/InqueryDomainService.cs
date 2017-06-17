using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;

namespace Beeant.Domain.Services.Product
{
    public class InqueryDomainService : RealizeDomainService<InqueryEntity>
    {

        #region 重写事务
   
        private IDictionary<string, ItemLoader<InqueryEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<InqueryEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<InqueryEntity>>
                    {
                         
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

       

        #endregion

        #region 重写验证

        #region 添加验证
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(InqueryEntity info)
        {
            return ValidateGoods(info)  && ValidateAccount(info); 
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(InqueryEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id == 0)
                return true;
            if (info.Account != null && info.Account.Id != 0)
            {
                var account = Repository.Get<AccountEntity>(info.Account.Id);
                if (account == null)
                {
                    info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
                    return false;
                }
                if (!account.IsUsed)
                {
                    info.AddErrorByName(typeof(AccountEntity).FullName, "UnUsed");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
            return false;
        }
        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateGoods(InqueryEntity info)
        {
            if (!info.HasSaveProperty(it => it.Goods.Id))
                return true;
            if (info.Goods != null && info.Goods.SaveType == SaveType.Add)
                return true;
            if (info.Goods != null && info.Goods.Id != 0)
            {
                if (Repository.Get<GoodsEntity>(info.Goods.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(GoodsEntity).FullName, "NoExist");
            return false;
        }

      
        #endregion


        #endregion


    }
}
