using System;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Authority
{

    [Serializable]
    public class OwnerAccountEntity : BaseEntity<OwnerAccountEntity>
    {
       

        /// <summary>
        ///数据组
        /// </summary>
        public OwnerEntity Owner { get;set;}
    
        /// <summary>
        ///账户
        /// </summary>
        public AccountEntity Account { get;set;}
    
      

    }
    
}
