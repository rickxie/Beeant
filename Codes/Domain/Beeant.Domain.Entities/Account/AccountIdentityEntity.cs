using System;

namespace Beeant.Domain.Entities.Account
{
    [Serializable]
    public class AccountIdentityEntity : BaseEntity<AccountIdentityEntity>
    {
         
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        public string Number { set; get; }
        /// <summary>
        /// 对应账户
        /// </summary>
        public AccountEntity Account { set; get; }
     
    }
}
