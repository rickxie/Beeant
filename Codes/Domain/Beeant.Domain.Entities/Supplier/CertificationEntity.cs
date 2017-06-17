
namespace Beeant.Domain.Entities.Supplier
{
    public class CertificationEntity : BaseEntity<CertificationEntity>
    {
        /// <summary>
        /// 供应商
        /// </summary>
        public SupplierEntity Supplier { set; get; }
        /// <summary>
        /// 其他证书图片
        /// </summary>
        public string Certification { set; get; }
        /// <summary>
        /// 其他证书图片流
        /// </summary>
        public byte[] CertificationByte { set; get; }
        /// <summary>
        /// 其他证书全名
        /// </summary>
        public string FullCertification
        {
            get { return this.GetFullFileName(Certification); }
        }
    }
}

