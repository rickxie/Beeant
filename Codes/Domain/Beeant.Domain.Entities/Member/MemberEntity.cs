using System;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Agent;

namespace Beeant.Domain.Entities.Member
{
    [Serializable]
    public class MemberEntity : BaseEntity<MemberEntity>
    {
        /// <summary>
        /// 集成商
        /// </summary>
        public AgentEntity Agent { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }              
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdCardNumber { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string Postal { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        public string HeadUrl { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] HeadUrlByte { get; set; }
        /// <summary>
        /// 全部文件路径
        /// </summary>
        public string FullHeadUrl
        {
            get { return this.GetFullFileName(HeadUrl); }
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        /// 是否启用名称
        /// </summary>
        public string IsUsedName
        {
            get { return this.GetVerifyName(IsUsed); }
        }
    }
}
