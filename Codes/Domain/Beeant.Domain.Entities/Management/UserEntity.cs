using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Workflow;

namespace Beeant.Domain.Entities.Management
{

    [Serializable]
    public class UserEntity : BaseEntity<UserEntity>
    {
        /// <summary>
        /// 对应账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string Name{get;set;}
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string IsUsedName
        {
            get { return this.GetVerifyName(IsUsed); }
        }
        /// <summary>
        /// 
        /// </summary>
        public IList<RoleAccountEntity> RoleAccounts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<OwnerAccountEntity> OwnerAccounts { get; set; }
        /// <summary>
        /// 授权用户
        /// </summary>
        public IList<AuditorAccountEntity> AuditorAccounts { get; set; }
        /// <summary>
        /// 授权用户
        /// </summary>
        public IList<GroupAccountEntity> GroupAccounts { get; set; }


    }

}
