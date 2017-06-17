using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Management;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Wms;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Wms
{
    public class StockItemDomainService : RealizeDomainService<StockItemEntity>
    {
        /// <summary>
        /// 库存商品
        /// </summary>
        public IDomainService InventoryDomainService { get; set; }
        /// <summary>
        /// 产品
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
                        {"Inventory", new UnitofworkHandle<InventoryEntity>{DomainService= InventoryDomainService}},
                        {"Product", new UnitofworkHandle<ProductEntity>{DomainService= ProductDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        private IDictionary<string, ItemLoader<StockItemEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<StockItemEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<StockItemEntity>>
                    {
                        {"Stock", LoadStock},
                        {"DataEntity", LoadDataEntity},
                        {"User", LoadUser},
                        {"Product", LoadProduct},
                        {"Inventory", LoadInventory}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }




        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual void LoadDataEntity(StockItemEntity info)
        {
            if(info.SaveType==SaveType.Add)return;
            info.DataEntity = Repository.Get<StockItemEntity>(info.Id);

        }
        /// <summary>
        /// 加载账户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadStock(StockItemEntity info)
        {
            if (info.SaveType==SaveType.Add && info.Stock != null && info.Stock.Id != 0)
            {
                info.Stock = info.Stock.SaveType == SaveType.Add ? info.Stock : Repository.Get<StockEntity>(info.Stock.Id);
            }
            else
            {
                LoadDataEntity(info);
                if (info.DataEntity != null && info.DataEntity.Stock != null)
                {
                    info.Stock = Repository.Get<StockEntity>(info.DataEntity.Stock.Id);
                }
            }

        }
        /// <summary>
        /// 加载用户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadUser(StockItemEntity info)
        {
            info.User = Repository.Get<UserEntity>(info.User.Id);
        }
        /// <summary>
        /// 加载用户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadProduct(StockItemEntity info)
        {
            LoadDataEntity(info);
            if (info.DataEntity != null && info.DataEntity.Product != null && info.DataEntity.Product.Id > 0)
            {
                info.Product = Repository.Get<ProductEntity>(info.DataEntity.Product.Id);
            }
            else if (info.Product != null && info.Product.Id > 0)
            {
                info.Product = Repository.Get<ProductEntity>(info.Product.Id);
            }
            
        }
        /// <summary>
        /// 加载清单
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadInventory(StockItemEntity info)
        {
            if (info.Inventory == null)
            {
                LoadDataEntity(info);
                var product = info.DataEntity != null ? info.DataEntity.Product : info.Product;
                var storehouse = info.DataEntity != null ? info.DataEntity.Storehouse : info.Storehouse;
                if (product != null && storehouse != null)
                {
                    var query = new QueryInfo();
                    query.Query<InventoryEntity>()
                         .Where(it => it.Storehouse.Id == storehouse.Id && it.Product.Id == product.Id);
                    var inventories = Repository.Gets<InventoryEntity>(query);
                    if (inventories != null)
                        info.Inventory = inventories.FirstOrDefault();
                }
            }
        }


        #endregion
 

        #region 重写验证

        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(StockItemEntity info)
        {
            return ValidateStock(info) && ValidateStorehouse(info) && ValidateProduct(info)
                && ValidateUser(info, null) && ValidateLockCount(info,null);
        }


        #endregion

        #region 修改验证
        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(StockItemEntity info)
        {
            var dataEntity = Repository.Get<StockItemEntity>(info.Id);
            return ValidateUser(info, dataEntity) && ValidateLockCount(info, dataEntity) && ValidateCount(info,dataEntity);
        }
        #endregion

        /// <summary>
        /// 验证出入库单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateStock(StockItemEntity info)
        {
            if (!info.HasSaveProperty(it => it.Stock.Id)) return true;
            if (info.Stock != null && info.Stock.SaveType == SaveType.Add)
                return true;
            if (info.Stock!=null && Repository.Get<StockEntity>(info.Stock.Id) != null)
                return true;
            info.AddErrorByName(typeof(StockEntity).FullName, "NoExist");
            return true;
        }

        /// <summary>
        /// 验证产品
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateProduct(StockItemEntity info)
        {
            if (!info.HasSaveProperty(it => it.Product.Id)) return true;
            if (info.Product != null && info.Product.SaveType == SaveType.Add)
                return true;
            if (info.Product != null && info.Product.Id != 0)
            {
                if (Repository.Get<ProductEntity>(info.Product.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(ProductEntity).FullName, "NoExist");
            return false;
        }


        /// <summary>
        /// 验证仓库
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateStorehouse(StockItemEntity info)
        {
            if (!info.HasSaveProperty(it => it.Storehouse.Id)) return true;
            if (info.Storehouse != null && info.Storehouse.SaveType == SaveType.Add)
                return true;
            if (info.Storehouse != null && info.Storehouse.Id != 0)
            {
                var storehouse = Repository.Get<StorehouseEntity>(info.Storehouse.Id);
                if (storehouse == null)
                {
                    info.AddErrorByName(typeof(StorehouseEntity).FullName, "NoExist");
                    return false;
                }
                if (!storehouse.IsUsed)
                {
                    info.AddErrorByName(typeof(StorehouseEntity).FullName, "UnUsed");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(StorehouseEntity).FullName, "NoExist");
            return false;
        }
        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateUser(StockItemEntity info, StockItemEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.User.Id) || info.User.Id==0)
                return true;
            if (dataEntity != null && dataEntity.User.Id == info.User.Id)
                return true;
            var user = Repository.Get<UserEntity>(info.User.Id);
            if (user == null)
            {
                info.AddErrorByName(typeof(UserEntity).FullName, "NoExist");
                return false;
            }
            if (!user.IsUsed)
            {
                info.AddErrorByName(typeof(UserEntity).FullName, "UnUsed");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateCount(StockItemEntity info, StockItemEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Count))
                return true;
            dataEntity.Stock = Repository.Get<StockEntity>(dataEntity.Stock.Id);
            if ((dataEntity.IsLockCount() || dataEntity.IsCount()) && dataEntity.Count != info.Count)
            {
                info.AddError("InStatusNotAllowModifyCount");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证锁库存
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateLockCount(StockItemEntity info, StockItemEntity dataEntity)
        {
            info.Stock = dataEntity != null
                            ? Repository.Get<StockEntity>(dataEntity.Stock.Id)
                            : info.Stock != null && info.Stock.SaveType == SaveType.Add
                                  ? info.Stock
                                  : Repository.Get<StockEntity>(info.Stock.Id);
            var count = dataEntity == null ? info.Count : dataEntity.Count;
            if (count > 0)
                return true;
            if (!info.IsLockCount()) return true;
            info.Product = dataEntity != null ? dataEntity.Product : info.Product;
            info.Storehouse = dataEntity != null ? dataEntity.Storehouse : info.Storehouse;
            if (info.Product != null && info.Storehouse != null)
            {
                var query = new QueryInfo();
                query.Query<InventoryEntity>()
                     .Where(it => it.Storehouse.Id == info.Storehouse.Id && it.Product.Id == info.Product.Id);
                var inventories = Repository.Gets<InventoryEntity>(query);
                if (inventories != null)
                    info.Inventory = inventories.FirstOrDefault();
            }
            if (info.Inventory == null)
                return true;
            if (info.Inventory.EnableCount < Math.Abs(count))
            {
                info.AddErrorByName(typeof(InventoryEntity).FullName, "EnoughCount");
                return false;
            }
            return true;
        }
        #endregion


    }
}
