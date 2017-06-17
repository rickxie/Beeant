using System;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Product;

namespace Beeant.Domain.Entities.Cart
{
    [Serializable]
    public class ShopcartEntity : BaseEntity<ShopcartEntity>
    {

        /// <summary>
        /// 产品
        /// </summary>
        public ProductEntity Product { get; set; }
        /// <summary>
        ///账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string Tag { set; get; }

    }
}
