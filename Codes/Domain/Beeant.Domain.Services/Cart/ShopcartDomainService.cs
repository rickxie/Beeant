using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Cart;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Cart
{
    public class ShopcartDomainService : RealizeDomainService<ShopcartEntity>
    {
 
        #region 重写事务

        private IDictionary<string, ItemLoader<ShopcartEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<ShopcartEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<ShopcartEntity>>
                    {
                        {"Product", LoadProduct}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }


        protected virtual void LoadProduct(ShopcartEntity info)
        {
            info.Product = Repository.Get<ProductEntity>(info.Product.Id);
        }


  

        #endregion

        #region 重写验证


        #region 验证添加

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(ShopcartEntity info)
        {
            var rev = ValidateAccount(info)
                      && ValidateProduct(info)
                      && ValidateMaxCount(info)
                      && ValidateExist(info);
            return rev;
        }



        /// <summary>
        /// 验证类型
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateAccount(ShopcartEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
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
        /// 验证订单明细
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateProduct(ShopcartEntity info)
        {
            if (!info.HasSaveProperty(it => it.Product.Id))
                return true;
            if (info.Product != null && info.Product.SaveType == SaveType.Add)
                return true;
            if (info.Product != null && info.Product.Id != 0)
            {
                info.Product = Repository.Get<ProductEntity>(info.Product.Id);
                if (info.Product == null)
                {
                    info.AddErrorByName(typeof(ProductEntity).FullName, "NoExist");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(ProductEntity).FullName, "NoExist");
            return false;
        }
        /// <summary>
        /// 验证订单明细
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateMaxCount(ShopcartEntity info)
        {
            var query = new QueryInfo();
            query.SetPageIndex(0).SetPageSize(1).Query<ShopcartEntity>()
                .Where(it => it.Account.Id == info.Account.Id && it.Product.Id != info.Product.Id).Select(it => it.Product.Id);
            Repository.GetEntities<ShopcartEntity>(query);
            var count = Configuration.ConfigurationManager.GetSetting<int>("ShopcartMaxCount");
            count = count == 0 ? 20 : count;
            if (query.DataCount > count - 1)
            {
                info.AddErrorByName(typeof(ShopcartEntity).FullName, "MaxCountOver");
                return false;
            }

            return true;
        }
        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(ShopcartEntity info)
        {
            var query = new QueryInfo();
            query.Query<ShopcartEntity>()
                   .Where(it => it.Account.Id == info.Account.Id && it.Product.Id == info.Product.Id);
            var dataEntities = Repository.GetEntities<ShopcartEntity>(query);
            if (dataEntities != null && dataEntities.Count > 0)
            {
                info.AddError("Exist");
                return false;
            }
            return true;
        }
        #endregion




        #endregion


    }
}
