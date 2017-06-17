using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Purchase;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Wms;
using Winner.Persistence;
using Winner.Persistence.Linq;
using FileEntity = Beeant.Domain.Entities.Utility.FileEntity;

namespace Beeant.Domain.Services.Purchase
{
    public class PurchaseItemDomainService : RealizeDomainService<PurchaseItemEntity>
    {
        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
               new FileEntity {FilePropertyName = "FileName",BytePropertyName = "FileByte"}
            };

        public override IList<FileEntity> FileProperties
        {
            get { return _fileProperties; }
            set { _fileProperties = value; }
        }
        /// <summary>
        /// 库存
        /// </summary>
        public IDomainService InventoryDomainService { get; set; }
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
                        {"Inventory", new UnitofworkHandle<InventoryEntity>{DomainService= InventoryDomainService}},
                        {"Product", new UnitofworkHandle<ProductEntity>{DomainService= ProductDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        private IDictionary<string, ItemLoader<PurchaseItemEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<PurchaseItemEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<PurchaseItemEntity>>
                    {
                        {"Purchase", LoadPurchase},
                        {"DataEntity", LoadDataEntity},
                        {"Inventory", LoadInventory},
                        {"Product", LoadProduct}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

        /// <summary>
        /// 设置业务
        /// </summary>
        /// <param name="unitofworks"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool SetBusiness(IList<IUnitofwork> unitofworks, PurchaseItemEntity info)
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
        protected virtual void LoadDataEntity(PurchaseItemEntity info)
        {
            if(info.SaveType==SaveType.Add)return;
            info.DataEntity = Repository.Get<PurchaseItemEntity>(info.Id);

        }
        /// <summary>
        /// 加载账户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadPurchase(PurchaseItemEntity info)
        {
           if (info.SaveType==SaveType.Add && info.Purchase != null && info.Purchase.Id!=0)
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
     
        /// <summary>
        /// 加载清单
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadInventory(PurchaseItemEntity info)
        {
            if (info.Inventory == null && info.Purchase != null && info.Product != null)
            {
                LoadPurchase(info);
                if(info.Purchase==null || info.Purchase.Storehouse==null)return;
                var query = new QueryInfo();
                query.Query<InventoryEntity>()
                     .Where(it => it.Storehouse.Id == info.Purchase.Storehouse.Id && it.Product.Id == info.Product.Id);
                var inventories = Repository.Gets<InventoryEntity>(query);
                if (inventories != null)
                    info.Inventory = inventories.FirstOrDefault();
            }
        }

        /// <summary>
        /// 加载用户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadProduct(PurchaseItemEntity info)
        {
            if (info.Product != null && info.Product.Id > 0)
            {
                info.Product = Repository.Get<ProductEntity>(info.Product.Id);
                if (info.Product == null)
                    return;
                LoadPurchase(info);
                var query = new QueryInfo();
                query.Query<InventoryEntity>()
                     .Where(it => it.Product.Id == info.Product.Id).Select(it => it.Count);
                info.Product.Inventories = Repository.GetEntities<InventoryEntity>(query);
              
            }   
        }


        #endregion
    

        #region 重写验证

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PurchaseItemEntity info)
        {
            PurchaseEntity purchase;
            var rev = ValidatePurchase(info, out purchase);
            if (!rev) return false;
            return ValidateProduct(info)
                && ValidatePurchasePaidAmount(info, null, purchase);
        }
        /// <summary>
        /// 验证修改
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(PurchaseItemEntity info)
        {
            var dataEntity = Repository.Get<PurchaseItemEntity>(info.Id);
            var purchase=Repository.Get<PurchaseEntity>(dataEntity.Purchase.Id);
            return ValidatePurchasePaidAmount(info, dataEntity, purchase) ;
        }

        #region 验证删除
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(PurchaseItemEntity info)
        {
            var dataEntity = Repository.Get<PurchaseItemEntity>(info.Id);
            if (dataEntity == null) return false;
            var purchase = Repository.Get<PurchaseEntity>(dataEntity.Purchase.Id);
            var rev = ValidatePurchasePaidAmount(info, dataEntity, purchase);
            return rev;
        }
        #endregion

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <param name="purchase"></param>
        /// <returns></returns>
        protected virtual bool ValidatePurchase(PurchaseItemEntity info,out PurchaseEntity purchase)
        {
            purchase = info.Purchase == null ? null
                          : info.Purchase.SaveType == SaveType.Add
                                ? info.Purchase
                                : Repository.Get<PurchaseEntity>(info.Purchase.Id);
            if (purchase == null)
            {
                info.AddErrorByName(typeof(PurchaseEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证金额
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <param name="purchase"></param>
        /// <returns></returns>
        protected virtual bool ValidatePurchasePaidAmount(PurchaseItemEntity info, PurchaseItemEntity dataEntity, PurchaseEntity purchase)
        {
            if (purchase == null || purchase.Type==PurchaseType.Return) return true;
            if (!info.HasSaveProperty(it => it.Count) && !info.HasSaveProperty(it => it.Price) && info.SaveType!=SaveType.Remove)
                return true;
            var count = !info.HasSaveProperty(it => it.Count) && dataEntity != null ? dataEntity.Count : info.Count;
            var price = !info.HasSaveProperty(it => it.Price) && dataEntity != null ? dataEntity.Price : info.Price;
            var amount = price * count;
            if (info.SaveType != SaveType.Remove && dataEntity != null && dataEntity.Amount == amount)
                return true;
            if (info.SaveType == SaveType.Remove)
                amount = 0 - amount;
            else if (dataEntity != null)
                amount = amount - dataEntity.Amount;
            if (purchase.TotalAmount + amount < purchase.PayAmount)
            {
                info.AddErrorByName(typeof(PurchaseEntity).FullName, "AmountLessPaidAmount");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证商品
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateProduct(PurchaseItemEntity info)
        {

            if (!info.HasSaveProperty(it => it.Product.Id) || info.Product == null ||
               info.Product.Id == 0)
                return true;
            if (info.Product != null && info.Product.SaveType == SaveType.Add)
                return true;
            var product = Repository.Get<ProductEntity>(info.Product.Id);
            if (product == null)
            {
                info.AddErrorByName(typeof(ProductEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }

        #endregion


    }
}
