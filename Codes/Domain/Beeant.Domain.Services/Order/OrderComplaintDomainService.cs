using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;

namespace Beeant.Domain.Services.Order
{
    public class OrderComplaintDomainService : RealizeDomainService<OrderComplaintEntity>
    {

        #region 重写事务
   
        private IDictionary<string, ItemLoader<OrderComplaintEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<OrderComplaintEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<OrderComplaintEntity>>());
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

 
        #endregion

        #region 重写验证

        #region 添加验证
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(OrderComplaintEntity info)
        {
            return ValidateOrder(info); 
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrder(OrderComplaintEntity info)
        {
            if (!info.HasSaveProperty(it => it.Order.Id))
                return true;
            if (info.Order != null && info.Order.SaveType == SaveType.Add)
                return true;
            if (info.Order != null && info.Order.Id != 0)
            {
                if (Repository.Get<OrderEntity>(info.Order.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(OrderEntity).FullName, "NoExist");
            return false;
        }

   
        #endregion


        #endregion


    }
}
