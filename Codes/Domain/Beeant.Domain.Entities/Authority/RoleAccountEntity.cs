using System;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Authority
{

    [Serializable]
    public class RoleAccountEntity : BaseEntity<RoleAccountEntity>
    {
       

        /// <summary>
        ///角色ID
        /// </summary>
        public RoleEntity Role{get;set;}
    
        /// <summary>
        ///用户ID
        /// </summary>
        public AccountEntity Account { get;set;}
    
      

    }
    
}
