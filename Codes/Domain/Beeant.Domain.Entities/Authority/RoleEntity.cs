using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;


namespace Beeant.Domain.Entities.Authority
{

    [Serializable]
    public class RoleEntity : BaseEntity<RoleEntity>
    {
        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 功能角色
        /// </summary>
        public IList<RoleAbilityEntity> RoleAbilities { get; set; }
        /// <summary>
        /// 角色用户
        /// </summary>
        public IList<RoleAccountEntity> RoleAccounts { get; set; }
    }
    
}
