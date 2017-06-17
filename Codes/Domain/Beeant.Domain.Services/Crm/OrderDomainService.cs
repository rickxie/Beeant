using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Crm;
using Winner.Persistence;

namespace Beeant.Domain.Services.Crm
{
    public class OrderDomainService : RealizeDomainService<OrderEntity>
    {
        private IDictionary<string, ItemLoader<OrderEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<OrderEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<OrderEntity>>
                    {
                        {"Customer", LoadCustomer}
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
        protected virtual void LoadCustomer(OrderEntity info)
        {
            info.Customer = info.Customer.SaveType == SaveType.Add ? info.Customer : Repository.Get<CustomerEntity>(info.Customer.Id);

        }

        #region 重写验证



        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(OrderEntity info)
        {
            return ValidateCustomer(info,null) && ValidateStatus(info, null) && ValidateStatusAndCusotmerCrm(info);
        }

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(OrderEntity info)
        {
            var dataInfo = Repository.Get<OrderEntity>(info.Id);
            return ValidateCustomer(info, dataInfo) && ValidateStatus(info, dataInfo);
        }

        /// <summary>
        /// 验证集成类型
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateCustomer(OrderEntity info, OrderEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Customer.Id))
                return true;
            if (info.Customer != null && info.Customer.SaveType == SaveType.Add)
                return true;
            if (info.Customer != null && info.Customer.Id!=0)
            {
                info.Customer = Repository.Get<CustomerEntity>(info.Customer.Id);
                if (dataEntity == null && info.Customer != null)
                {
                    return true;
                }
                if (dataEntity != null && dataEntity.Crm != null && info.Customer != null && info.Customer.Crm != null &&
                    info.Crm.Id == info.Customer.Crm.Id)
                {
                    return true;
                }
            }
            info.AddErrorByName(typeof(CustomerEntity).FullName, "NoExist");
            return false;
        }

        /// <summary>
        /// 验证集成类型
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateStatus(OrderEntity info, OrderEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Status.Id))
                return true;
            if (info.Status != null && info.Status.SaveType == SaveType.Add)
                return true;
            if (info.Status != null && info.Status.Id != 0)
            {
                info.Status = Repository.Get<OrderStatusEntity>(info.Status.Id);
                if (dataEntity == null && info.Status != null)
                {
                        return true;
                }
                if (dataEntity!=null && dataEntity.Crm != null && info.Status != null && info.Status.Crm != null &&
                    info.Crm.Id == info.Status.Crm.Id)
                {
                    return true;
                }
            }
            info.AddErrorByName(typeof(OrderStatusEntity).FullName, "NoExist");
            return false;
        }

        /// <summary>
        /// 验证集成类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateStatusAndCusotmerCrm(OrderEntity info)
        {
            var statuscrm = info.Status==null?null: info.Status.Crm;
            var custoemrcrm = info.Customer == null ? null : info.Customer.Crm;
            if (statuscrm != null && custoemrcrm != null && statuscrm.Id == custoemrcrm.Id)
                return true;
            info.AddErrorByName(typeof(OrderStatusEntity).FullName, "NoExist");
            return false;
        }
        #endregion


    }
}
