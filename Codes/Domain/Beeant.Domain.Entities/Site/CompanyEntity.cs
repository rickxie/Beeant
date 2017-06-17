using System;
using Beeant.Domain.Entities.Basedata;

namespace Beeant.Domain.Entities.Site
{
    [Serializable]
    public class CompanyEntity : BaseEntity<CompanyEntity>
    {
        /// <summary>
        /// 账户
        /// </summary>
        public SiteEntity Site { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///邮箱必填
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string Qq { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Linkman { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 微信公众号
        /// </summary>
        public string WeixinQrCodeFileName { get; set; }
        /// <summary>
        /// 备案号
        /// </summary>
        public string RecordNumber { get; set; }
        /// <summary>
        /// 微信公众号
        /// </summary>
        public byte[] WeixinQrCodeFileByte { get; set; }
        /// <summary>
        /// 微信公众号
        /// </summary>
        public string WeixinQrCodeFullFileName  
        {
            get { return this.GetFullFileName(WeixinQrCodeFileName); }
        }
        /// <summary>
        /// 目录本
        /// </summary>
        public AlbumEntity Album { get; set; }

    }
}
