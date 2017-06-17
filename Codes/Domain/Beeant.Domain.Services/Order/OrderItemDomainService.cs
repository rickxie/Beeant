using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;

namespace Beeant.Domain.Services.Order
{
    public class OrderItemDomainService : RealizeDomainService<OrderItemEntity>
    {

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
                        {"Order", new UnitofworkHandle<OrderEntity>{Repository= Repository}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        private IDictionary<string, ItemLoader<OrderItemEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<OrderItemEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<OrderItemEntity>>
                    {
                        {"DataEntity", LoadDataEntity},
                        {"Order", LoadOrder}
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
        protected virtual void LoadDataEntity(OrderItemEntity info)
        {
            if (info.SaveType == SaveType.Add || info.DataEntity != null) return;
            info.DataEntity = Repository.Get<OrderItemEntity>(info.Id);
        }

        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual void LoadOrder(OrderItemEntity info)
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
 



 
 


        #region 重写验证


        #region 验证添加

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(OrderItemEntity info)
        {
            OrderEntity order;
            var rev = ValidateOrder(info, out order);
            if (!rev) return false;
            return ValidateOrderAmount(info, null, order);
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrder(OrderItemEntity info, out OrderEntity order)
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
        protected virtual bool ValidateOrderAmount(OrderItemEntity info, OrderItemEntity dataEntity, OrderEntity order)
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
                info.AddErrorByName(typeof(OrderEntity).FullName, "PayAmountLessTotalPayAmount");
                return false;
            }
            var amount = info.Amount;
            if (dataEntity != null)
                amount = amount - dataEntity.Amount;
            if (order.TotalPayAmount + amount < order.PayAmount)
            {
                info.AddErrorByName(typeof(OrderEntity).FullName, "PayAmountLessTotalPayAmount");
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
        protected override bool ValidateModify(OrderItemEntity info)
        {
            var dataEntity = Repository.Get<OrderItemEntity>(info.Id);
            if (dataEntity == null) return false;
            var order = Repository.Get<OrderEntity>(dataEntity.Order.Id);
            var rev =  ValidateOrderAmount(info, dataEntity, order); 
            return rev;
        }
        #endregion

 
 
     
     
        #endregion


    }
}
