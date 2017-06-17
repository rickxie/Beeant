using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Crm;
using Winner.Persistence;

namespace Beeant.Domain.Services.Crm
{
    public class CustomerNoteDomainService : RealizeDomainService<CustomerNoteEntity>
    {
        private IDictionary<string, ItemLoader<CustomerNoteEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<CustomerNoteEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<CustomerNoteEntity>>
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
        protected virtual void LoadCustomer(CustomerNoteEntity info)
        {
            info.Customer = info.Customer.SaveType == SaveType.Add ? info.Customer : Repository.Get<CustomerEntity>(info.Customer.Id);

        }

        #region 重写验证



        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(CustomerNoteEntity info)
        {
            return ValidateCustomer(info);
        }
     

        /// <summary>
        /// 验证集成类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateCustomer(CustomerNoteEntity info)
        {
            if (!info.HasSaveProperty(it => it.Customer.Id))
                return true;
            if (info.Customer != null && info.Customer.SaveType == SaveType.Add)
                return true;
            if (info.Customer != null && info.Customer.Id!=0)
            {
                if (Repository.Get<CustomerEntity>(info.Customer.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(CustomerEntity).FullName, "NoExist");
            return false;
        }
        #endregion


    }
}
