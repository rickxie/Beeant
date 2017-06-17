using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Purchase;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Wms;
using Winner.Persistence;
using Winner.Persistence.Linq;
 
namespace Beeant.Domain.Services.Purchase
{
    public class PurchaseDomainService : RealizeDomainService<PurchaseEntity>
    {
        /// <summary>
        /// 采购明细
        /// </summary>
        public IDomainService PurchaseItemDomainService { get; set; }
        /// <summary>
        /// 付款明细
        /// </summary>
        public IDomainService PurchasePayDomainService { get; set; }
 
        /// <summary>
        /// 附件
        /// </summary>
        public IDomainService PurchaseAttachmentDomainService { get; set; }
        /// <summary>
        /// 采购发票
        /// </summary>
        public IDomainService PurchaseInvoiceDomainService { get; set; }
        /// <summary>
        /// 快递信息
        /// </summary>
        public IDomainService PurchaseExpressDomainService { get; set; }

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
                        {"PurchaseItems", new UnitofworkHandle<PurchaseItemEntity>{DomainService= PurchaseItemDomainService}},
                        {"PurchasePays", new UnitofworkHandle<PurchasePayEntity>{DomainService= PurchasePayDomainService}},
                        {"PurchaseAttachments",new UnitofworkHandle<PurchaseAttachmentEntity>{DomainService= PurchaseAttachmentDomainService} },
                        {"PurchaseExpresses", new UnitofworkHandle<PurchaseExpressEntity>{DomainService= PurchaseExpressDomainService}},
                        {"PurchaseInvoices", new UnitofworkHandle<PurchaseInvoiceEntity>{DomainService=PurchaseInvoiceDomainService}}
                       
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        private IDictionary<string, ItemLoader<PurchaseEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<PurchaseEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<PurchaseEntity>>
                    {
                        {"DataEntity", LoadDataEntity},
                        {"PurchaseItems", LoadPurchaseItems},
                        {"PurchasePays", LoadPurchasePays},
                        {"Stocks", LoadStocks},
                        {"Account", LoadAccount}
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
        protected virtual void LoadDataEntity(PurchaseEntity info)
        {
            if(info.SaveType==SaveType.Add)return;
            info.DataEntity = Repository.Get<PurchaseEntity>(info.Id);
        }
        /// <summary>
        /// 加载原始数据
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadStocks(PurchaseEntity info)
        {
            var query = new QueryInfo();
            query.Query<StockEntity>()
                 .Where(it => it.Purchase.Id == info.Id)
                 .Select(it => new object[] {it.Status, it.StockItems.Select(s => new object[] {s.Count, s.Product.Id})});
            info.Stocks = Repository.GetEntities<StockEntity>(query);
        }
        /// <summary>
        /// 填充订单明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadPurchaseItems(PurchaseEntity info)
        {
            if (info.SaveType == SaveType.Add) return;
            var purchaseItems = info.PurchaseItems == null
                               ? null
                               : info.PurchaseItems.Where(it => it.SaveType == SaveType.Add).ToList();
            var query = new QueryInfo();
            query.Query<PurchaseItemEntity>().Where(it => it.Purchase.Id == info.Id);
            info.PurchaseItems = Repository.GetEntities<PurchaseItemEntity>(query);
            FillPurchaseItemProducts(info.PurchaseItems);
            FillPurchaseItemInventories(info, info.PurchaseItems);
            if (info.PurchaseItems != null)
            {
                info.PurchaseItems = info.PurchaseItems.ToList();
                info.PurchaseItems.AddList(purchaseItems);
            }
     
        }

        /// <summary>
        /// 加载清单
        /// </summary>
        /// <param name="purchase"></param>
        /// <param name="infos"></param>
        protected virtual void FillPurchaseItemInventories(PurchaseEntity purchase,IList<PurchaseItemEntity> infos)
        {
            if (infos == null || infos.Count == 0)
                return;
            LoadDataEntity(purchase);
            var storehouse = purchase.DataEntity == null ? purchase.Storehouse : purchase.DataEntity.Storehouse;
            if (storehouse == null) return;
            purchase.Storehouse = storehouse;
            var productIds =
                infos.Where(it => it.Product != null && it.Product.Id > 0).Select(it => it.Product.Id).ToArray();
            var query = new QueryInfo();
            query.Query<InventoryEntity>()
                 .Where(it => it.Storehouse.Id == storehouse.Id && productIds.Contains(it.Product.Id));
            var inventories = Repository.Gets<InventoryEntity>(query);
            if (inventories != null)
            {
                foreach (var info in infos)
                {
                    if (info.Product == null) continue;
                    info.Inventory =
                        inventories.FirstOrDefault(
                            it => it.Product.Id == info.Product.Id);
                }
            }
        }

        /// <summary>
        /// 加载用户
        /// </summary>
        /// <param name="infos"></param>
        protected virtual void FillPurchaseItemProducts(IList<PurchaseItemEntity> infos)
        {
            var productIds =
                infos.Where(it => it.Product != null && it.Product.Id > 0).Select(it => it.Product.Id).ToArray();
            var query = new QueryInfo();
            query.Query<ProductEntity>()
                 .Where(it =>  productIds.Contains(it.Id)).Select(it=>new object[]{it,it.Inventories.Select(s=>s)});
            var products = Repository.Gets<ProductEntity>(query);
            if (products != null)
            {
                foreach (var info in infos)
                {
                    if(info.Product==null)continue;
                    info.Product =
                        products.FirstOrDefault(
                            it => it.Id ==info.Product.Id);
                }
            }
        }

        /// <summary>
        /// 填充订单明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadPurchasePays(PurchaseEntity info)
        {
            if (info.SaveType == SaveType.Add) return;
            var receiveds = info.PurchasePays == null
                                ? null
                                : info.PurchasePays.Where(it => it.SaveType == SaveType.Add).ToList();
            var query = new QueryInfo();
            query.Query<PurchasePayEntity>().Where(it => it.Purchase.Id == info.Id);
            info.PurchasePays = Repository.GetEntities<PurchasePayEntity>(query).ToList();
            if (info.PurchasePays != null)
            {
                info.PurchasePays = info.PurchasePays.ToList();
                info.PurchasePays.AddList(receiveds);
            }
        }


        /// <summary>
        /// 填充订单明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadAccount(PurchaseEntity info)
        {
            info.Account = Repository.Get<AccountEntity>(info.Account.Id);
        }
 
        #endregion

        #region 重写验证


        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PurchaseEntity info)
        {
            return ValidateAccount(info, null) && ValidateOrder(info,null) &&
                ValidateDeliveryDate(info, null)
                && ValidateOriginalPurchase(info, null) && ValidateStore(info)
                && ValidateStocks(info,null);
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(PurchaseEntity info, PurchaseEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id!=0)
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
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrder(PurchaseEntity info, PurchaseEntity dataEntity)
        {
            if (info.Type != PurchaseType.Sales && !info.HasSaveProperty(it => it.Order.Id))
                return true;
            if (dataEntity != null && dataEntity.Order != null && info.Order != null && info.Order.Id == dataEntity.Order.Id)
                return true;
            if (info.Type == PurchaseType.Sales && (info.Order == null || info.Order.Id == 0))
            {
                info.AddError("PurchaseSalesMustSelectOrder");
                return false;
            }
            if (info.Order != null && (info.Order.SaveType == SaveType.Add || info.Order.Id==0))
                return true;
            if (info.Order == null)
                return true;
            var order = Repository.Get<OrderEntity>(info.Order.Id);
            if (order == null)
            {
                info.AddErrorByName(typeof (OrderEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateStore(PurchaseEntity info)
        {
            if (info.Type!=PurchaseType.Store && !info.HasSaveProperty(it => it.Storehouse.Id))
                return true;
            if (info.Type == PurchaseType.Store && (info.Storehouse == null || info.Storehouse.Id == 0))
            {
                info.AddError("PurchaseStoreMustSelectStore");
                return false;
            }
            var sorehouse = Repository.Get<StorehouseEntity>(info.Storehouse.Id);
            if (info.Type != PurchaseType.Sales && sorehouse == null)
            {
                info.AddErrorByName(typeof(StorehouseEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证原始订单
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateOriginalPurchase(PurchaseEntity info, PurchaseEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.OriginalPurchase.Id) || info.OriginalPurchase.Id == 0)
                return true;
            if (dataEntity != null && dataEntity.OriginalPurchase != null && dataEntity.OriginalPurchase.Id == info.OriginalPurchase.Id)
                return true;
            
            var purchase = Repository.Get<PurchaseEntity>(info.OriginalPurchase.Id);
            if (purchase == null)
            {
                info.AddError("OriginalPurchaseNoExist");
                return false;
            }
            var account = dataEntity == null ? info.Account : dataEntity.Account;
            if (purchase.Account != null && account != null && purchase.Account.Id != account.Id)
            {
                info.AddError("OriginalAccountNotEqueal");
                return false;
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
        protected override bool ValidateModify(PurchaseEntity info)
        {
            var dataEntity = Repository.Get<PurchaseEntity>(info.Id);
            return ValidateAccount(info, dataEntity) && ValidateOrder(info, dataEntity) &&  ValidateDeliveryDate(info, dataEntity)
                && ValidateOriginalPurchase(info, dataEntity)
                && ValidateStocks(info, dataEntity);
        }

  
        #endregion
        /// <summary>
        /// 验证交货日期
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateDeliveryDate(PurchaseEntity info, PurchaseEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.PurchaseDate) && !info.HasSaveProperty(it => it.DeliveryDate))
                return true;
            var orderData = dataEntity != null && !info.HasSaveProperty(it => it.PurchaseDate)
                                ? dataEntity.PurchaseDate
                                : info.PurchaseDate;
            var deliveryDate = dataEntity != null && !info.HasSaveProperty(it => it.DeliveryDate)
                           ? dataEntity.DeliveryDate
                           : info.DeliveryDate;
            if (orderData > deliveryDate)
            {
                info.AddError("DeliveryDateLessPurchaseDate");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证stocks
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateStocks(PurchaseEntity info, PurchaseEntity dataEntity)
        {
            if (info.HasSaveProperty(it => it.Status) && info.Status != PurchaseStatusType.WaitSettle)
                return true;
            if (dataEntity != null && dataEntity.Status == info.Status)
                return true;
            var type = dataEntity != null ? dataEntity.Type : info.Type;
            if (type != PurchaseType.Store)
                return true;
            var query = new QueryInfo();
            query.Query<StockEntity>()
                 .Where(it => it.Purchase.Id == info.Id)
                 .Select(it => new object[] { it.Status});
            var stocks = Repository.GetEntities<StockEntity>(query);
            if (stocks == null || stocks.Count==0 || stocks.Count(it=>it.Status!= StockStatusType.Finish) > 0)
            {
                info.AddError("HasStockNoFinish");
                return false;
            }
            return true;
        }
        #endregion


    }
}
