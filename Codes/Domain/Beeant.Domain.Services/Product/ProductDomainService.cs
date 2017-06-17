using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Wms;
using Beeant.Domain.Entities.Supplier;
using Winner.Persistence;
using Winner.Persistence.Linq;
using ProductEntity = Beeant.Domain.Entities.Product.ProductEntity;

namespace Beeant.Domain.Services.Product
{
    public class ProductDomainService : RealizeDomainService<ProductEntity>
    {
        /// <summary>
        /// 映射关系
        /// </summary>
        public IDomainService SheetDomainService { get; set; }
        /// <summary>
        /// 库存商品
        /// </summary>
        public IDomainService InventoryDomainService { get; set; }
        /// <summary>
        /// 属性实例
        /// </summary>
        public IDomainService GoodsPropertyDomainService { get; set; }

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
                        {"GoodsProperties", new UnitofworkHandle<GoodsPropertyEntity>{DomainService= GoodsPropertyDomainService}},
                        {"Sheets", new UnitofworkHandle<GoodsPropertyEntity>{DomainService= SheetDomainService}},
                        {"Inventories",new UnitofworkHandle<GoodsImageEntity>{DomainService= InventoryDomainService} }
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }

        private IDictionary<string, ItemLoader<ProductEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<ProductEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<ProductEntity>>
                    {
                        {"DataEntity", LoadDataEntity},
                        {"Goods", LoadGoods}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

        #region 业务实现
        /// <summary>
        /// 设置业务
        /// </summary>
        /// <param name="unitofworks"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool SetBusiness(IList<IUnitofwork> unitofworks, ProductEntity info)
        {
            var rev = base.SetBusiness(unitofworks, info);
            if (info.Goods != null && info.Goods.SaveType != SaveType.None)
                unitofworks.AddList(Repository.Save(info.Goods));
            return rev;
        }
        /// <summary>
        /// 加载原始数据
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadDataEntity(ProductEntity info)
        {
            if (info.SaveType == SaveType.Add) return;
            info.DataEntity = Repository.Get<ProductEntity>(info.Id);
        }
        /// <summary>
        /// 填充商品
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadGoods(ProductEntity info)
        {
            if (info.Goods != null && info.Goods.Id != 0)
            {
                info.Goods = Repository.Get<GoodsEntity>(info.Goods.Id);
            }
        }

        /// <summary>
        /// 填充库存商品
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadInventories(ProductEntity info)
        {
            var query = new QueryInfo();
            query.Query<InventoryEntity>().Where(it => it.Product.Id == info.Id);
            info.Inventories = Repository.GetEntities<InventoryEntity>(query);
        }
        #endregion

        #region 重写验证


        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(ProductEntity info)
        {
            return ValidateGoods(info);
        }


        /// <summary>
        /// 验证类目
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateGoods(ProductEntity info)
        {
            if (!info.HasSaveProperty(it => it.Goods.Id))
                return true;
            if (info.Goods != null && info.Goods.SaveType == SaveType.Add)
                return true;
            if (info.Goods != null && info.Goods.Id != 0)
            {
              
                var product = Repository.Get<GoodsEntity>(info.Goods.Id);
                if (product == null)
                {
                    info.AddErrorByName(typeof(GoodsEntity).FullName, "NoExist");
                    return false;
                }
            }
            return true;
        }
 
        #endregion


        #endregion
    }
}
