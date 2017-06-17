using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Authority
{
    [Serializable]
    public class OwnerEntity : BaseEntity<OwnerEntity>
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
        /// 别名
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 提交代码
        /// </summary>
        public string SubmitCode { get; set; }
        /// <summary>
        /// 阅读代码
        /// </summary>
        public string ReadCodes { get; set; }
        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<OwnerAccountEntity> OwnerAccounts { get; set; }
        /// <summary>
        /// 角色用户
        /// </summary>
        public IList<RoleAccountEntity> RoleAccounts { get; set; }

    }
}
