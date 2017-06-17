using System;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Management;

namespace Beeant.Domain.Entities.Merchant
{
    [Serializable]
    public class PartnerEntity : BaseEntity<PartnerEntity>
    {
        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Linkman { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string Qq { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Postcode { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 服务客户编号
        /// </summary>
        public UserEntity Service { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] FileByte { get; set; }

        /// <summary>
        /// 网站风格
        /// </summary>
        public StyleEntity WebsiteStyle { get; set; }

        /// <summary>
        /// 手机端风格
        /// </summary>
        public StyleEntity MobileStyle { get; set; }

        /// <summary>
        /// 全部文件路径
        /// </summary>
        public string FullFileName
        {
            get { return this.GetFullFileName(FileName); }
        }

        /// <summary>
        /// 是否启用名称
        /// </summary>
        public string IsUsedName
        {
            get { return this.GetVerifyName(IsUsed); }
        }
    }
}
