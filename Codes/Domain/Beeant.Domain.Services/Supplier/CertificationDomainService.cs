using System.Collections.Generic;
using Beeant.Domain.Entities.Supplier;
using Beeant.Domain.Entities.Utility;

namespace Beeant.Domain.Services.Supplier
{
    public class CertificationDomainService : RealizeDomainService<CertificationEntity>
    {
        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
               new FileEntity {FilePropertyName = "Certification",BytePropertyName = "CertificationByte"}
            };

        public override IList<FileEntity> FileProperties
        {
            get { return _fileProperties; }
            set { _fileProperties = value; }
        }
      

        #region 重写验证
        /// <summary>
        /// 重写验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(CertificationEntity info)
        {
            return ValidateSupplier(info);
        }
  
        /// <summary>
        /// 验证供应商
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateSupplier(CertificationEntity info)
        {
            if (!info.HasSaveProperty(it => it.Supplier.Id))
                return false;
            var supplier = Repository.Get<SupplierEntity>(info.Supplier.Id);
            if (supplier == null)
            {
                info.AddErrorByName(typeof(SupplierEntity).FullName, "NoExist");
                return false;
            }
            return true;    
        }
        #endregion
      
    }
}
