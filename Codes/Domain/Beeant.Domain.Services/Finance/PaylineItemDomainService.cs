using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;

namespace Beeant.Domain.Services.Finance
{
    public class PaylineItemDomainService : RealizeDomainService<PaylineItemEntity>
    {
        /// <summary>
        /// 服务实例
        /// </summary>
        public virtual IDomainService OrderDomainService { get; set; }

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
                        {"Order", new UnitofworkHandle<OrderEntity>{DomainService= OrderDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PaylineItemEntity info)
        {
            var rev = ValidatePayline(info);
            return rev;
        }


        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidatePayline(PaylineItemEntity info)
        {
            if (!info.HasSaveProperty(it => it.Payline.Id))
                return true;
            if (info.Payline != null && info.Payline.SaveType == SaveType.Add)
                return true;
           
            if (info.Payline != null)
            {
                var payline = Repository.Get<PaylineEntity>(info.Payline.Id);
                if (payline != null)
                {
                    return true;
                }
            }
            info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
            return false;
        }




    }
}
