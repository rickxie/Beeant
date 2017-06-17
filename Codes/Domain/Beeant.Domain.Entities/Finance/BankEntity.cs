using System;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Finance
{

  
    [Serializable]
    public class BankEntity : BaseEntity<BankEntity>
    {
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 银行账户
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 开户人
        /// </summary>
        public string Holder { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Linkman { set; get; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { set; get; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string Email { set; get; }
        /// <summary>
        /// 是否认证
        /// </summary>
        public bool IsAuthentication { get; set; }
        /// <summary>
        /// 是否认证
        /// </summary>
        public string IsAuthenticationName
        {
            get { return this.GetStatusName(IsAuthentication); }
        }

    }
    
}
