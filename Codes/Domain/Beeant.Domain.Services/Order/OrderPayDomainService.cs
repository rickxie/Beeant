using System.Collections.Generic;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;

namespace Beeant.Domain.Services.Order
{
    public class OrderPayDomainService : RealizeDomainService<OrderPayEntity>
    {
         

        #region 重写事务
       
        private IDictionary<string, ItemLoader<OrderPayEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<OrderPayEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<OrderPayEntity>>
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
        protected override bool SetBusiness(IList<IUnitofwork> unitofworks, OrderPayEntity info)
        {
            var rev= base.SetBusiness(unitofworks, info);
            if(info.Order!=null && info.Order.SaveType!=SaveType.None)
                unitofworks.AddList(Repository.Save(info.Order));
            return rev;
        }



        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual void LoadDataEntity(OrderPayEntity info)
        {
            if(info.SaveType==SaveType.Add)return;
            info.DataEntity = Repository.Get<OrderPayEntity>(info.Id);

        }
        /// <summary>
        /// 加载账户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadOrder(OrderPayEntity info)
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
 
        #region 验证添加
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(OrderPayEntity info)
        {
            OrderEntity order;
            var rev = ValidateOrder(info, out order);
            if (!rev) return false;
            rev = ValidateOrderReceivedAmount(info, null, order);
            return rev;
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrder(OrderPayEntity info, out OrderEntity order)
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
            if (order.Type == OrderType.Return) return true;
            if (order.PayAmount == 0 && info.Amount < order.Deposit)
            {
                info.AddError("FirstPayThanDeposit", info.Order.Id);
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
        protected virtual bool ValidateOrderReceivedAmount(OrderPayEntity info, OrderPayEntity dataEntity, OrderEntity order)
        {
            if (order == null) return true;
            if (!info.HasSaveProperty(it => it.Amount)) 
                return true;
            if (dataEntity != null && dataEntity.Amount == info.Amount)
                return true;
            var amount = info.Amount;
            if (dataEntity != null)
                amount = amount - dataEntity.Amount;
            if (order.TotalAmount < order.PayAmount + amount)
            {
                info.AddErrorByName(typeof(OrderEntity).FullName,"AmountLessPayAmount");
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
        protected override bool ValidateModify(OrderPayEntity info)
        {
            var dataEntity = Repository.Get<OrderPayEntity>(info.Id);
            if (dataEntity == null) return false;
            var order = Repository.Get<OrderEntity>(dataEntity.Order.Id);
            var rev = ValidateOrderReceivedAmount(info, dataEntity, order);
            return rev;
        }
        #endregion
   
        #endregion

  

    }
}
