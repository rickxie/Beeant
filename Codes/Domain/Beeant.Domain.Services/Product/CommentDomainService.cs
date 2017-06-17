using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Product
{
    public class CommentDomainService : RealizeDomainService<CommentEntity>
    {


        #region 加载
        /// <summary>
        /// 目录商品
        /// </summary>
        public IDomainService OrderItemDomainService { get; set; }

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
                        {"OrderItems", new UnitofworkHandle<OrderItemEntity>{DomainService= OrderItemDomainService}},
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }

        private IDictionary<string, ItemLoader<CommentEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<CommentEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<CommentEntity>>
                {
                    {"Product", LoadProduct},
                    {"OrderProducts", LoadOrderProducts}
                });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

        /// <summary>
        /// 加载商品
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadProduct(CommentEntity info)
        {
            info.Product = info.Product.SaveType == SaveType.Add ? info.Product : Repository.Get<ProductEntity>(info.Product.Id);
        }
        /// <summary>
        /// 加载商品
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadOrderProducts(CommentEntity info)
        {
          var query=new QueryInfo();
            query.Query<OrderProductEntity>()
                .Where(it => it.Order.Id == info.Order.Id && it.Product.Id == info.Product.Id)
                .Select(it => new object[] {it.Id});
            info.OrderProducts = Repository.GetEntities<OrderProductEntity>(query);
        }
        #endregion

        #region 重写验证

        /// <summary>
        /// 批量验证
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<CommentEntity> infos)
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
        protected override bool ValidateAdd(CommentEntity info)
        {
            return ValidateAccount(info)
                   && ValidateOrder(info)
                   && ValidateProduct(info)
                   && ValidateOrderProduct(info)
                   && ValidateExist(info);
        }
        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(CommentEntity info)
        {
            var query = new QueryInfo();
            query.Query<CommentEntity>()
                 .Where(it => it.Order.Id == info.Order.Id && it.Product.Id==info.Product.Id && it.Account.Id == info.Account.Id);
            var infos = Repository.GetEntities<CommentEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("Exist");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证类型
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateAccount(CommentEntity info)
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
        /// <returns></returns>
        protected virtual bool ValidateOrder(CommentEntity info)
        {
            if (!info.HasSaveProperty(it => it.Order.Id))
                return true;
            if (info.Order != null && info.Order.SaveType == SaveType.Add)
                return true;
            if (info.Order != null && info.Order.Id != 0)
            {
                var order = GetOrderItem(info);
                if (order == null)
                {
                    info.AddErrorByName(typeof(OrderEntity).FullName, "NoExist");
                    return false;
                }
                if (order.Status != OrderStatusType.Finish)
                {
                    info.AddErrorByName(typeof(OrderEntity).FullName, "UnFinish");
                    return false;
                }
                if (order.Account.Id != info.Account.Id)
                {
                    info.AddError("AccountHasnotOrder");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(OrderEntity).FullName, "NoExist");
            return false;
        }
        /// <summary>
        /// 得到订单明细
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual OrderEntity GetOrderItem(CommentEntity info)
        {
            var query=new QueryInfo();
            query.Query<OrderEntity>()
                .Where(it => it.Id == info.Order.Id)
                .Select(it => new object[] {it.Id, it.Status,it.Account.Id});
            var infos = Repository.GetEntities<OrderEntity>(query);
            return infos?.FirstOrDefault();
        }
        /// <summary>
        /// 验证商品
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateProduct(CommentEntity info)
        {
            if (!info.HasSaveProperty(it => it.Product.Id))
                return true;
            if (info.Product != null && info.Product.SaveType == SaveType.Add)
                return true;
            if (info.Product != null && info.Product.Id != 0)
            {
                var product = Repository.Get<ProductEntity>(info.Product.Id);
                if (product == null)
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
        /// 验证订单是否有商品存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrderProduct(CommentEntity info)
        {
            var query = new QueryInfo();
            query.Query<OrderProductEntity>()
                 .Where(it => it.Order.Id == info.Order.Id && it.Product.Id == info.Product.Id)
                 .Select(it => it.Id);
            var infos = Repository.GetEntities<OrderProductEntity>(query);
            if (infos != null && infos.Count > 0)
                return true;
            info.AddError("OrderHasnotGoods");
            return false;
        }

  

        #endregion


 

        #endregion


    }
}
