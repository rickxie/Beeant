using System.Collections.Generic;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;

namespace Beeant.Domain.Services.Order
{
    public class OrderInvoiceDomainService : RealizeDomainService<OrderInvoiceEntity>
    {

        #region 重写事务
 
        private IDictionary<string, ItemLoader<OrderInvoiceEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<OrderInvoiceEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<OrderInvoiceEntity>>
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
        protected override bool SetBusiness(IList<IUnitofwork> unitofworks, OrderInvoiceEntity info)
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
        protected virtual void LoadDataEntity(OrderInvoiceEntity info)
        {
            if(info.SaveType==SaveType.Add)return;
            info.DataEntity = Repository.Get<OrderInvoiceEntity>(info.Id);

        }
        /// <summary>
        /// 加载账户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadOrder(OrderInvoiceEntity info)
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
        protected override bool ValidateAdd(OrderInvoiceEntity info)
        {
            OrderEntity order;
            var rev = ValidateOrder(info, out order);
            if (!rev) return false;
            rev = ValidateOrderReceiptAmount(info, null, order);
            return rev;
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrder(OrderInvoiceEntity info, out OrderEntity order)
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
        protected virtual bool ValidateOrderReceiptAmount(OrderInvoiceEntity info, OrderInvoiceEntity dataEntity, OrderEntity order)
        {
            if (order == null) return true;
            if (!info.HasSaveProperty(it => it.Amount)) 
                return true;
            if (order.TotalInvoiceAmount < order.InvoiceAmount + info.Amount)
            {
                info.AddErrorByName(typeof(OrderEntity).FullName, "AmountLessReceiptAmount");
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
        protected override bool ValidateModify(OrderInvoiceEntity info)
        {
            var dataEntity = Repository.Get<OrderInvoiceEntity>(info.Id);
            if (dataEntity == null) return false;
            var order = Repository.Get<OrderEntity>(dataEntity.Order.Id);
            var rev = ValidateOrderReceiptAmount(info, dataEntity, order);
            return rev;
        }
        #endregion
   
 
        #endregion

  

    }
}
