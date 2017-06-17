using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Member;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Member
{
    public class BrowseDomainService : RealizeDomainService<BrowseEntity>
    {
        /// <summary>
        /// 商品
        /// </summary>
        public IDomainService ProductDomainService { get; set; }
        #region 重写事务
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
        private IDictionary<string, ItemLoader<BrowseEntity>> _itemLoaders;
        /// <summary>
        /// 加载数据
        /// </summary>
        protected override IDictionary<string, ItemLoader<BrowseEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<BrowseEntity>>
                    {
                        {"Product", LoadProduct}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }
        #region 填充业务
 

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadProduct(BrowseEntity info)
        {
            var query = new QueryInfo();
            query.Query<ProductEntity>()
                 .Where(it => it.Id == info.Product.Id)
                 .Select(
                     it =>
                     new object[]
                         {
                                 it.Id, it.Goods.Id,it.Goods.Products.Select(s=>s.Id)
                         });
            var infos = Repository.GetEntities<ProductEntity>(query);
            if (infos != null)
                info.Product = infos.FirstOrDefault();
            if (info.Product != null && info.Product.Goods != null && info.Product.Goods.Products!=null)
            {
                var ids = info.Product.Goods.Products.Select(it => it.Id).ToArray();
                var queryBrowse=new QueryInfo();
                queryBrowse.Query<BrowseEntity>()
                    .Where(it => it.Account.Id == info.Account.Id && ids.Contains(it.Product.Id)).Select(it=>it.Id);
                var browses = Repository.GetEntities<BrowseEntity>(queryBrowse);
                if (browses != null && browses.Count > 0)
                {
                    info.Id = browses.First().Id;
                    info.SaveType=SaveType.Modify;
                    info.SetProperty(it => it.Product.Id);
                }
            }

        }
        #endregion

        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(BrowseEntity info)
        {
            return ValidateAccount(info) && ValidateProduct(info);
        }


        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(BrowseEntity info)
        {
            return true;
        }
        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(BrowseEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id != 0)
            {
                info.Account = Repository.Get<AccountEntity>(info.Account.Id);
                if (info.Account == null)
                {
                    info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
                    return false;
                }
                if (!info.Account.IsUsed)
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
        protected virtual bool ValidateProduct(BrowseEntity info)
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
