using System;


namespace Beeant.Domain.Entities.Supplier
{
    [Serializable]
    public class QualificationEntity : BaseEntity<QualificationEntity>
    {
        /// <summary>
        /// 供应商标识
        /// </summary>
        public SupplierEntity Supplier { get; set; }
        /// <summary>
        /// 营业执照
        /// </summary>
        public string BusinessLicense { get; set; }
        /// <summary>
        /// 营业执照文件流
        /// </summary>
        public byte[] BusinessLicenseByte { set; get; }
        /// <summary>
        /// 营业执照全名
        /// </summary>
        public string FullBusinessLicense
        {
            get { return this.GetFullFileName(BusinessLicense); }
        }
        /// <summary>
        /// 组织机构代码证
        /// </summary>
        
        public string AgencyLicense { get; set; }
        /// <summary>
        /// 组织机构代码证文件流
        /// </summary>
        public byte[] AgencyLicenseByte { get; set; }
        /// <summary>
        /// 组织机构代码证全名
        /// </summary>
        public string FullAgencyLicense
        {
            get { return this.GetFullFileName(AgencyLicense); }
        }
        /// <summary>
        /// 银行开户许可证
        /// </summary>
        
        public string BankLicense { get; set; }
        /// <summary>
        /// 银行开户许可证文件流
        /// </summary>
        public byte[] BankLicenseByte { get; set; }
        /// <summary>
        /// 银行开户许可证全名
        /// </summary>
        public string FullBankLicense
        {
            get { return this.GetFullFileName(BankLicense); }
        }
        /// <summary>
        /// 税务登记证
        /// </summary>
        
        public string TaxLicense { get; set; }
        /// <summary>
        /// 税务登记证文件流
        /// </summary>
        public byte[] TaxLicenseByte { get; set; }
        /// <summary>
        /// 税务登记证全名
        /// </summary>
        public string FullTaxLicense
        {
            get { return this.GetFullFileName(TaxLicense); }
        }
        /// <summary>
        /// 品牌授权
        /// </summary>
        public QualificationType BrandAuthorization { get; set; }
        /// <summary>
        /// 品牌授权名称
        /// </summary>
        public string BrandAuthorizationName
        {
            get { return BrandAuthorization.GetName(); }
        }
        /// <summary>
        /// 商标注册证
        /// </summary>
        
        public string TrademarkLicense { get; set; }
        /// <summary>
        /// 商标注册证文件流
        /// </summary>
        public byte[] TrademarkLicenseByte { get; set; }
        /// <summary>
        /// 商标注册证全名
        /// </summary>
        public string FullTrademarkLicense
        {
            get { return this.GetFullFileName(TrademarkLicense); }
        }
    }
}
