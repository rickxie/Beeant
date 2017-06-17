using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Cart;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Cart
{
    public class AttentionDomainService : RealizeDomainService<AttentionEntity>
    {

        public IDomainService ProductDomainService { get; set; }





        private IDictionary<string, IUnitofworkHandle> _itemHandles;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, IUnitofworkHandle> ItemHandles
        {
            get
            {
                return _itemHandles ?? (_itemHandles = new Dictionary<string, IUnitofworkHandle>
                    {
                        {"Product", new UnitofworkHandle<GoodsEntity>{DomainService= ProductDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }

        private IDictionary<string, ItemLoader<AttentionEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<AttentionEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<AttentionEntity>>
                    {
                        {"Product", LoadProduct}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }
        #region 重写事务

      
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadProduct(AttentionEntity info)
        {
            if (info.SaveType == SaveType.Remove && info.Product == null)
            {
                var query = new QueryInfo();
                query.Query<AttentionEntity>()
                     .Where(it => it.Id == info.Id)
                     .Select(
                         it =>
                         new object[]
                             {
                                 it, it.Product.Id,it.Product.Goods.Id
                             });
                var infos = Repository.GetEntities<AttentionEntity>(query);
                if (infos != null)
                {
                    var dataEntity = infos.FirstOrDefault();
                    if (dataEntity != null)
                        info.Product = dataEntity.Product;
                }
            }
            else
            {
                var query = new QueryInfo();
                query.Query<ProductEntity>()
                     .Where(it => it.Id == info.Product.Id)
                     .Select(
                         it =>
                         new object[]
                             {
                                 it, it.Goods.Id
                             });
                var infos = Repository.GetEntities<ProductEntity>(query);
                if(infos!=null)
                    info.Product = infos.FirstOrDefault();
            }
                
        }
        #endregion

        #region 重写验证

        /// <summary>
        /// 批量验证
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<AttentionEntity> infos)
        {
            var temps =
                infos.Where(it => it.SaveType == SaveType.Add)
                     .ToList();
            if (temps.Count > 1)
            {
                foreach (var temp in temps)
                {
                    temp.AddErrorByName(typeof(BaseEntity).FullName, "NoAllowBatchSave");
                }
                return false;
            }
            return true;
        }
        #region 验证添加

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(AttentionEntity info)
        {
            return ValidateAccount(info)
                   && ValidateProduct(info)
                   && ValidateExist(info);
        }
        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(AttentionEntity info)
        {
            var query = new QueryInfo();
            query.Query<AttentionEntity>()
                 .Where(it => it.Product.Id == info.Product.Id && it.Account.Id == info.Account.Id);
            var infos = Repository.GetEntities<AttentionEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("Exist");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateAccount(AttentionEntity info)
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
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateProduct(AttentionEntity info)
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

        #endregion




        #endregion


    }
}
