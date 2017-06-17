using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Crm;
using Winner.Persistence;

namespace Beeant.Domain.Services.Crm
{
    public class OrderItemDomainService : RealizeDomainService<OrderItemEntity>
    {
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
        protected virtual void LoadOrder(OrderItemEntity info)
        {
            info.Order = info.Order.SaveType == SaveType.Add ? info.Order : Repository.Get<OrderEntity>(info.Order.Id);

        }

        #region 重写验证



        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(OrderItemEntity info)
        {
            return ValidateOrder(info) ;
        }


        /// <summary>
        /// 验证集成类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrder(OrderItemEntity info)
        {
            if (!info.HasSaveProperty(it => it.Order.Id))
                return true;
            if (info.Order != null && info.Order.SaveType == SaveType.Add)
                return true;
            if (info.Order != null && info.Order.Id!=0)
            {
                info.Order = Repository.Get<OrderEntity>(info.Order.Id);
                if (info.Order != null)
                {
                    return true;
                }
            }
            info.AddErrorByName(typeof(OrderEntity).FullName, "NoExist");
            return false;
        }

 
        #endregion


    }
}
