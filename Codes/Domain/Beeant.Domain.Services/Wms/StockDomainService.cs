using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Purchase;
using Beeant.Domain.Entities.Wms;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Wms
{
    public class StockDomainService : RealizeDomainService<StockEntity>
    {
        /// <summary>
        /// 库存商品
        /// </summary>
        public IDomainService StockItemDomainService { get; set; }

    
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
                        {"StockItems", new UnitofworkHandle<StockItemEntity>{DomainService= StockItemDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        private IDictionary<string, ItemLoader<StockEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<StockEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<StockEntity>>
                    {
                        {"DataEntity", LoadDataEntity},
                        {"StockItems", LoadStockItems}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }



        /// <summary>
        /// 加载原始数据
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadDataEntity(StockEntity info)
        {
            info.DataEntity = Repository.Get<StockEntity>(info.Id);
        }
        /// <summary>
        /// 填充库存商品
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadStockItems(StockEntity info)
        {
            if(info.SaveType==SaveType.Add)return;
            var query = new QueryInfo();
            query.Query<StockItemEntity>()
                 .Where(it => it.Stock.Id == info.Id).Select(it=>new object[]{it,it.Product.Count,it.Stock.Type,it.Product.Id});
            info.StockItems = Repository.GetEntities<StockItemEntity>(query);
            FillStockItemInventories(info.StockItems);
        }
        /// <summary>
        /// 加载清单
        /// </summary>
        /// <param name="infos"></param>
        protected virtual void FillStockItemInventories(IList<StockItemEntity> infos)
        {
            if (infos == null || infos.Count == 0)
                return;
            var productIds =
                infos.Where(it => it.Product != null && it.Product.Id > 0).Select(it => it.Product.Id).ToArray();
            var storehouseIds =
               infos.Where(it => it.Storehouse != null && it.Storehouse.Id > 0).Select(it => it.Storehouse.Id).ToArray();
            var query = new QueryInfo();
            query.Query<InventoryEntity>()
                 .Where(it => storehouseIds.Contains(it.Storehouse.Id)  && productIds.Contains(it.Product.Id));
            var inventories = Repository.Gets<InventoryEntity>(query);
           if (inventories != null)
           {
               foreach (var info in infos)
               {
                   if (info.Product == null || info.Storehouse==null) continue;
                   info.Inventory =
                       inventories.FirstOrDefault(
                           it => it.Product.Id == info.Product.Id && it.Storehouse.Id == info.Storehouse.Id);
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
        protected override bool ValidateAdd(StockEntity info)
        {
            var rev =ValidateType(info,null) && ValidatePurchase(info, null);
            if (rev) rev = ValidateOrder(info, null);
            return rev;
        }


        #endregion

        #region 修改验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(StockEntity info)
        {
            var dataEntity = Repository.Get<StockEntity>(info.Id);
            var rev = ValidateType(info, dataEntity) && ValidatePurchase(info, dataEntity);
            if (rev) rev = ValidateOrder(info, dataEntity);
            return rev;
        }
        #endregion

        /// <summary>
        /// 验证采购单
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidatePurchase(StockEntity info, StockEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Purchase.Id)) return true;
            if (info.Purchase == null || info.Purchase.Id==0) return true;
            if (dataEntity != null && dataEntity.Purchase != null && dataEntity.Purchase.Id.Equals(info.Purchase.Id))
                return true;
            if (info.Purchase != null && info.Purchase.SaveType == SaveType.Add)
                return true;
            if (dataEntity != null && dataEntity.Purchase != null && dataEntity.Purchase.Id == info.Purchase.Id)
                return true;
            if (Repository.Get<PurchaseEntity>(info.Purchase.Id) != null)
                return true;
            info.AddErrorByName(typeof(PurchaseEntity).FullName, "NoExist");
            return true;
        }

        /// <summary>
        /// 验证销售类型
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateType(StockEntity info, StockEntity dataEntity)
        {
            var type = info.HasSaveProperty(it => it.Type) || dataEntity == null ? info.Type : dataEntity.Type;
            if (type == StockType.PurchaseIn || type == StockType.ProduceOut)
            {
                var purchaseId = info.HasSaveProperty(it => it.Purchase.Id) || dataEntity == null || dataEntity.Purchase==null ? info.Purchase.Id : dataEntity.Purchase.Id;
                if (purchaseId == 0)
                {
                    info.AddError("PurchaseTypeMustPurchaseId");
                    return false;
                }
            }
            if (type == StockType.SalesIn || type == StockType.SalesOut)
            {
                var orderId = info.HasSaveProperty(it => it.Order.Id) || dataEntity == null || dataEntity.Order == null ? info.Order.Id : dataEntity.Order.Id;
                if (orderId == 0)
                {
                    info.AddError("SalesTypeMustOrderId");
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrder(StockEntity info, StockEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Order.Id) || info.Order == null || info.Order.Id==0)
                return true;
            if (info.Order != null && info.Order.SaveType == SaveType.Add)
                return true;
            if (dataEntity != null && dataEntity.Order != null && dataEntity.Order.Id == info.Order.Id)
                return true;
            if (Repository.Get<OrderEntity>(info.Order.Id) != null)
                return true;
            info.AddErrorByName(typeof (OrderEntity).FullName, "NoExist");
            return false;
        }


        #endregion


    }
}
