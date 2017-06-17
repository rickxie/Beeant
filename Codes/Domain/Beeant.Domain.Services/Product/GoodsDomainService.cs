using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Merchant;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Services.Product
{
    public class GoodsDomainService : RealizeDomainService<GoodsEntity>
    {

      
        /// <summary>
        /// 属性实例
        /// </summary>
        public IGoodsPropertyDomainService GoodsPropertyValidateDomainService { get; set; }

        /// <summary>
        /// 属性实例
        /// </summary>
        public IDomainService GoodsPropertyDomainService { get; set; }

        /// <summary>
        /// 图片实例
        /// </summary>
        public IDomainService GoodsImageDomainService { get; set; }
        /// <summary>
        /// 详情实例
        /// </summary>
        public IDomainService GoodsDetailDomainService { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public IDomainService ProductDomainService { get; set; }

        /// <summary>
        /// 目录商品
        /// </summary>
        public IDomainService CatalogueGoodsDomainService { get; set; }

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
                        {"Products", new UnitofworkHandle<ProductEntity>{DomainService= ProductDomainService}},
                        {"GoodsProperties", new UnitofworkHandle<GoodsPropertyEntity>{DomainService= GoodsPropertyDomainService}},
                        {"GoodsImages",new UnitofworkHandle<GoodsImageEntity>{DomainService= GoodsImageDomainService} },
                        {"GoodsDetails",new UnitofworkHandle<GoodsDetailEntity>{DomainService= GoodsDetailDomainService} },
                        {"CatalogueGoodses", new UnitofworkHandle<CatalogueGoodsEntity>{DomainService= CatalogueGoodsDomainService}},
                      
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        private IDictionary<string, ItemLoader<GoodsEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<GoodsEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<GoodsEntity>>
                    {
                        {"DataEntity", LoadDataEntity},
                        {"Products", LoadProducts},
                        {"Category", LoadCategory}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

        #region 填充业务
        /// <summary>
        /// 加载原始数据
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadDataEntity(GoodsEntity info)
        {
            if (info.SaveType == SaveType.Add) return;
            info.DataEntity = Repository.Get<GoodsEntity>(info.Id);
        }
        /// <summary>
        /// 填充订单明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadCategory(GoodsEntity info)
        {
            if (info.SaveType == SaveType.Add && info.Category != null && info.Category.Id != 0)
            {
                info.Category = Repository.Get<CategoryEntity>(info.Category.Id);
            }
            else if (info.SaveType == SaveType.Modify)
            {
                LoadDataEntity(info);
                if (info.DataEntity != null)
                    info.Category = info.DataEntity.Category;
            }
        }

        /// <summary>
        /// 填充商品
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadProducts(GoodsEntity info)
        {
            if (info.SaveType == SaveType.Add) return;
            if (info.HasSaveProperty(it => it.Products) || info.Properties == null && info.Products != null)
                return;
            var query = new QueryInfo();
            query.Query<ProductEntity>().Where(it => it.Goods.Id == info.Id);
            info.Products = Repository.GetEntities<ProductEntity>(query);
        }


        #endregion

        #region 重写事务
        protected override bool SetBusiness(IList<IUnitofwork> unitofworks, GoodsEntity info)
        {
            if (info.SaveType == SaveType.Remove)
            {
                var catalogueGoods = new CatalogueGoodsEntity { SaveType = SaveType.Remove };
                catalogueGoods.Where(it => it.Goods.Id == info.Id);
                unitofworks.AddList(Repository.Save(catalogueGoods));
            }
            var result = base.SetBusiness(unitofworks, info);
            return result;
        }

        #endregion

        #region 重写验证


        #region 添加验证

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(GoodsEntity info)
        {
            return ValidateAccount(info) && ValidateFreight(info, null) && ValidateCategory(info, null) &&
                   ValidateDataId(info, null) && ValidateSkuCount(info, null);
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(GoodsEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account == null || info.Account.Id == 0)
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
        /// 验证类目
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateCategory(GoodsEntity info, GoodsEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Category.Id))
                return true;
            if (info.Category != null && info.Category.SaveType == SaveType.Add)
                return true;
            if (info.Category != null && info.Category.Id != 0)
            {
                if (dataEntity != null && dataEntity.Category != null && dataEntity.Category.Id == info.Category.Id)
                    return true;
                var category = Repository.Get<CategoryEntity>(info.Category.Id);
                if (category == null)
                {
                    info.AddErrorByName(typeof(CategoryEntity).FullName, "NoExist");
                    return false;
                }
                if (!category.IsPublish)
                {
                    info.AddErrorByName(typeof(CategoryEntity).FullName, "UnPublish");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(CategoryEntity).FullName, "NoExist");
            return false;
        }

        /// <summary>
        /// 验证集成商
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateFreight(GoodsEntity info, GoodsEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Freight.Id) || info.Freight == null || info.Freight.Id == 0)
                return true;
            if (dataEntity != null && dataEntity.Freight != null && info.Freight.Id.Equals(dataEntity.Freight.Id))
                return true;
            var freight = Repository.Get<FreightEntity>(info.Freight.Id);
            if (freight == null)
            {
                info.AddErrorByName(typeof(FreightEntity).FullName, "NoExist");
                return false;
            }

            var accountid = dataEntity == null || dataEntity.Account == null ? 0 : dataEntity.Account.Id;

            var freightAccountId = freight.Account == null ? 0 : freight.Account.Id;
            if (accountid != freightAccountId)
            {
                info.AddError("FreightAccountNotEqual");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证同步编号
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateDataId(GoodsEntity info, GoodsEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.DataId) || string.IsNullOrEmpty(info.DataId))
                return true;
            if (dataEntity != null && dataEntity.DataId == info.DataId)
                return true;
            var query = new QueryInfo();
            query.Query<GoodsEntity>().Where(it => it.DataId == info.DataId).Select(it => it.Id);
            var infos = Repository.GetEntities<GoodsEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("DataIdExist");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证SKU数量
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateSkuCount(GoodsEntity info, GoodsEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.GoodsImages) && !info.HasSaveProperty(it => it.Products))
                return true;
            var catagoryId = !info.HasSaveProperty(it => it.Category.Id) && dataEntity != null && dataEntity.Category != null
                                 ? dataEntity.Category.Id
                                 : info.Category == null ? 0 : info.Category.Id;
            if (catagoryId == 0)
                return true;
            var category = Repository.Get<CategoryEntity>(catagoryId);
            if (category == null)
                return true;
            var query = new QueryInfo();
            query.Query<PropertyEntity>().Where(it => it.Category.Id == catagoryId).Select(it => new object[] {it.Value,it.CustomCount});
            var infos = Repository.GetEntities<PropertyEntity>(query);
            if (infos == null)
                return true;
            var count =infos.Sum(it=>it.CustomCount)+ infos.Where(it => it.ValueArray != null).Sum(it => it.ValueArray.Length);
            if (info.HasSaveProperty(it => it.GoodsImages) && info.GoodsImages != null)
            {
                var goodsImageCount =
                    info.GoodsImages.Count(it => it.SaveType == SaveType.Add || it.SaveType == SaveType.Modify);
                if (goodsImageCount > category.ImageCount + category.ImageCount * count)
                {
                    info.AddError("ImageCountOver");
                    return false;
                }
            }
            if (info.HasSaveProperty(it => it.GoodsImages) && info.GoodsImages != null)
            {
                if (info.Products!=null && info.Products.Count - 1 > category.ImageCount * count)
                {
                    info.AddError("ProductCountOver");
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 修改验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(GoodsEntity info)
        {
            var dataEntity = Repository.Get<GoodsEntity>(info.Id);
            return ValidateFreight(info, dataEntity) && ValidateCategory(info, dataEntity)
                && ValidateDataId(info, dataEntity) && ValidateSkuCount(info, dataEntity);
        }

        #endregion

        #endregion



    }
}
