using System.Collections.Generic;
using Beeant.Domain.Entities.Supplier;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;

namespace Beeant.Domain.Services.Supplier
{
    public class QualificationDomainService : RealizeDomainService<QualificationEntity>
    {
        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
                new FileEntity {FilePropertyName = "BusinessLicense",BytePropertyName = "BusinessLicenseByte"},
                new FileEntity {FilePropertyName = "AgencyLicense",BytePropertyName = "AgencyLicenseByte"},
                new FileEntity {FilePropertyName = "BankLicense",BytePropertyName = "BankLicenseByte"},
                new FileEntity {FilePropertyName = "TaxLicense",BytePropertyName = "TaxLicenseByte"},
                new FileEntity {FilePropertyName = "TrademarkLicense",BytePropertyName = "TrademarkLicenseByte"},
            };
        public override IList<FileEntity> FileProperties
        {
            get { return _fileProperties; }
            set { _fileProperties = value; }
        }


        #region 重写验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(QualificationEntity info)
        {
            var rev = ValidateSupplier(info);
            return rev;
        }
        /// <summary>
        /// 验证供应商是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateSupplier(QualificationEntity info)
        {
            if (info.SaveType != SaveType.Add && info.SaveType != SaveType.Modify)
                return true;
            if (!info.HasSaveProperty(it => it.Supplier.Id))
                return false;
            var supplier = Repository.Get<SupplierEntity>(info.Supplier.Id);

            if (supplier == null)
            {
                info.AddErrorByName(typeof(QualificationEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }
        #endregion
    }
}
