using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;

namespace Beeant.Domain.Services.Order
{
    public class OrderNoteDomainService : RealizeDomainService<OrderNoteEntity>
    {

        #region 加载
        private IDictionary<string, ItemLoader<OrderNoteEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<OrderNoteEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<OrderNoteEntity>>
                    {
               
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

        #endregion

        #region 重写验证

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(OrderNoteEntity info)
        {
            return ValidateOrder(info) && ValidateAccount(info);
        }
        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrder(OrderNoteEntity info)
        {
            if (!info.HasSaveProperty(it => it.Order.Id))
                return true;
            if (info.Order != null && info.Order.SaveType == SaveType.Add)
                return true;
            if (info.Order != null && info.Order.Id!=0)
            {
                if (Repository.Get<OrderEntity>(info.Order.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(OrderEntity).FullName, "NoExist");
            return false;
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(OrderNoteEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id != 0)
            {
                if (Repository.Get<AccountEntity>(info.Account.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
            return false;
        }

        #endregion


    }
}
