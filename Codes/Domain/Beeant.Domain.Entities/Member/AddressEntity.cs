using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Member
{
    public class AddressEntity : BaseEntity<AddressEntity>
    {
        /// <summary>
        /// 所属账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string County { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string Recipient { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Postcode { get; set; }
        /// <summary>
        /// 接收地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 是否默认地址
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 地址标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public string IsDefaultName
        {
            get { return this.GetStatusName(IsDefault); }
        }
    }
}
