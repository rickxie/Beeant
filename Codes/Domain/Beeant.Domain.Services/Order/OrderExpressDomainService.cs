using System.Collections.Generic;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;

namespace Beeant.Domain.Services.Order
{
    public class OrderExpressDomainService : RealizeDomainService<OrderExpressEntity>
    {

        #region 加载
        private IDictionary<string, ItemLoader<OrderExpressEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<OrderExpressEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<OrderExpressEntity>>
                    {
                        {"Order", LoadOrder},
                        {"DataEntity", LoadDataEntity}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }
        /// <summary>
        /// 重写
        /// </summary>
        /// <param name="unitofworks"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool SetBusiness(IList<IUnitofwork> unitofworks, OrderExpressEntity info)
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
        protected virtual void LoadDataEntity(OrderExpressEntity info)
        {
            if (info.SaveType == SaveType.Add) return;
            info.DataEntity = Repository.Get<OrderExpressEntity>(info.Id);

        }
        /// <summary>
        /// 加载账户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadOrder(OrderExpressEntity info)
        {
            if (info.SaveType == SaveType.Add && info.Order != null && info.Order.Id != 0)
            {
                info.Order = info.Order.SaveType == SaveType.Add ? info.Order : Repository.Get<OrderEntity>(info.Order.Id);
            }
            else
            {
                LoadDataEntity(info);
                if (info.DataEntity != null && info.DataEntity.Order != null)
                {
                    info.Order = Repository.Get<OrderEntity>(info.DataEntity.Order.Id);
                }
            }

        }
    
        #endregion

        #region 重写验证



        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(OrderExpressEntity info)
        {
            return ValidateOrder(info);
        }
    
        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrder(OrderExpressEntity info)
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


    }
}
