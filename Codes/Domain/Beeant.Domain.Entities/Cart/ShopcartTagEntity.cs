using System;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Cart
{
    [Serializable]
    public class ShopcartTagEntity : BaseEntity<ShopcartTagEntity>
    {
        /// <summary>
        /// 对应账号
        /// </summary>
        public AccountEntity Account { set; get; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string Name { set; get; }
    }
}
