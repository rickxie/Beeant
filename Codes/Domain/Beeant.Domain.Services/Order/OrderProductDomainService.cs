using System.Collections.Generic;
using System.IO;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Promotion;
using Winner.Persistence;
using FileEntity = Beeant.Domain.Entities.Utility.FileEntity;

namespace Beeant.Domain.Services.Order
{
    public class OrderProductDomainService : RealizeDomainService<OrderProductEntity>
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
                        {"Goods", new UnitofworkHandle<GoodsEntity>{Repository= Repository}},
                        {"Product", new UnitofworkHandle<ProductEntity>{Repository= Repository}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }

        private IDictionary<string, ItemLoader<OrderProductEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<OrderProductEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<OrderProductEntity>>
                    {
                        {"DataEntity", LoadDataEntity},
                        {"Order", LoadOrder},
                        {"Product", LoadProduct}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }
   
        /// <summary>
        /// 重写处理事件
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override IList<IUnitofwork> Handle(OrderProductEntity info)
        {
            SetFileName(info);
            return base.Handle(info);
        }
        /// <summary>
        /// 设置业务
        /// </summary>
        /// <param name="unitofworks"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool SetBusiness(IList<IUnitofwork> unitofworks, OrderProductEntity info)
        {
            var rev = base.SetBusiness(unitofworks, info);
            if (info.Order != null && info.Order.SaveType != SaveType.None)
                unitofworks.AddList(Repository.Save(info.Order));
            return rev;
        }
        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual void LoadDataEntity(OrderProductEntity info)
        {
            if (info.SaveType == SaveType.Add || info.DataEntity != null) return;
            info.DataEntity = Repository.Get<OrderProductEntity>(info.Id);
        }

        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual void LoadOrder(OrderProductEntity info)
        {
            if (info.SaveType == SaveType.Add && info.Order != null && info.Order.Id != 0)
            {
                info.Order = info.Order.SaveType == SaveType.Add ? info.Order : Repository.Get<OrderEntity>(info.Order.Id);
            }
            else
            {
                LoadDataEntity(info);
                if (info.DataEntity != null)
                    info.Order = Repository.Get<OrderEntity>(info.DataEntity.Order.Id);
            }
        }
        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual void LoadProduct(OrderProductEntity info)
        {
            if (info.SaveType == SaveType.Add && info.Product != null && info.Product.Id != 0)
            {
                info.Product = info.Product.SaveType == SaveType.Add
                                    ? info.Product
                                    : Repository.Get<ProductEntity>(info.Product.Id);
            }
            else
            {
                LoadDataEntity(info);
                if (info.DataEntity != null)
                    info.Product = Repository.Get<ProductEntity>(info.DataEntity.Product.Id);
            }
            if (info.Product != null && info.Product.Goods != null)
                info.Product.Goods = Repository.Get<GoodsEntity>(info.Product.Goods.Id);
       
        }



        /// <summary>
        /// 设置文件名
        /// </summary>
        /// <param name="info"></param>
        protected virtual void SetFileName(OrderProductEntity info)
        {
            if (info == null || !info.HasSaveProperty(it => it.Product.Id) || info.Product==null || info.Product.Id==0 || info.FileByte != null)
                return;
            if (info.SaveType == SaveType.Modify)
            {
                var dataEntity = Repository.Get<OrderProductEntity>(info.Id);
                if (dataEntity.Product != null && dataEntity.Product.Id == info.Product.Id)
                {
                    info.FileName = dataEntity.FileName;
                    return;
                }
            }
            var product = Repository.Get<ProductEntity>(info.Product.Id);
            if (product == null)
                return;
            info.FileName = product.FileName;
            if (string.IsNullOrEmpty(info.FileName))
            {
                product.Goods = Repository.Get<GoodsEntity>(product.Goods.Id);
                info.FileName = product.Goods.FileName;
            }
            if (string.IsNullOrEmpty(info.FileName))
                return;
            info.FileName = string.Format("{0}.l{1}", info.FileName, Path.GetExtension(info.FileName));
            info.FileByte = FileRepository.Download(info.FileName);
            if(info.FileByte==null)
                return;
            info.FileName = string.Format("Files/Images/OrderItem/copy{0}", Path.GetExtension(info.FileName));
        }

        #endregion


        #region 重写验证


        #region 验证添加

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(OrderProductEntity info)
        {
            OrderEntity order;
            var rev = ValidateOrder(info, out order);
            if (!rev) return false;
            return ValidateProduct(info) && ValidatePromotion(info) && ValidateOrderAmount(info, null, order);
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrder(OrderProductEntity info, out OrderEntity order)
        {
            order = info.Order == null ? null
                           : info.Order.SaveType == SaveType.Add
                                 ? info.Order
                                 : Repository.Get<OrderEntity>(info.Order.Id);
            if (order == null)
            {
                info.AddErrorByName(typeof(OrderEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证金额
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrderAmount(OrderProductEntity info, OrderProductEntity dataEntity, OrderEntity order)
        {
            if (order == null || order.Type == OrderType.Return) return true;
            if (!info.HasSaveProperty(it => it.Amount))
                return true;
            if (dataEntity != null && dataEntity.Amount == info.Amount)
                return true;
            if (order.SaveType == SaveType.Add)
            {
                if (order.OrderItems == null || order.OrderItems.Sum(it => it.Count * it.Price) >= 0)
                    return true;
                info.AddErrorByName(typeof(OrderEntity).FullName, "AmountLessReceivedAmount");
                return false;
            }
            var amount = info.Amount;
            if (dataEntity != null)
                amount = amount - dataEntity.Amount;
            if (order.TotalAmount + amount < order.PayAmount)
            {
                info.AddErrorByName(typeof(OrderEntity).FullName, "AmountLessReceivedAmount");
                return false;
            }
            return true;
        }
        #endregion

        #region 验证修改
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(OrderProductEntity info)
        {
            var dataEntity = Repository.Get<OrderProductEntity>(info.Id);
            if (dataEntity == null) return false;
            var order = Repository.Get<OrderEntity>(dataEntity.Order.Id);
            var rev = ValidateOrderAmount(info, dataEntity, order); 
            return rev;
        }
        #endregion

        /// <summary>
        /// 验证商品
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateProduct(OrderProductEntity info)
        {
            if (!info.HasSaveProperty(it => it.Product.Id) || info.Product == null || info.Product.Id == 0)
            {
                return true;
            }
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

        /// <summary>
        /// 验证商品
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidatePromotion(OrderProductEntity info)
        {
            if (!info.HasSaveProperty(it => it.Promotion.Id) || info.Promotion == null || info.Promotion.Id == 0)
            {
                return true;
            }
            if (info.Promotion != null && info.Promotion.SaveType == SaveType.Add)
                return true;

            var promotionItem = Repository.Get<PromotionEntity>(info.Promotion.Id);
            if (promotionItem == null)
            {
                info.AddErrorByName(typeof(PromotionEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }
 
 
      
        #endregion


    }
}
