using System;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Order;

namespace Beeant.Domain.Entities.Merchant
{
    [Serializable]
    public class MerchantOrderEntity : BaseEntity<MerchantOrderEntity>
    {
        /// <summary>
        /// 父类
        /// </summary>
        public AccountEntity Account { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public OrderEntity Order { get; set; }
    }

}
