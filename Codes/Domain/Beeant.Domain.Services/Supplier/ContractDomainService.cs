using System.Collections.Generic;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;
using Beeant.Domain.Entities.Supplier;

namespace Beeant.Domain.Services.Supplier
{
    public class ContractDomainService : RealizeDomainService<ContractEntity>
    {
        private IList<FileEntity> _fileProperties = new List<FileEntity>
        {
           new FileEntity {FilePropertyName = "Attachment", BytePropertyName = "AttachmentByte"}
        };

        public override IList<FileEntity> FileProperties
        {
            get { return _fileProperties; }
            set { _fileProperties = value; }
        }
      

        #region 重写验证

   

        #region 添加验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(ContractEntity info)
        {
            var rev = ValidateSupplier(info) && ValidateDate(info);
            return rev;
        }

        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(ContractEntity info)
        {            
            var rev = ValidateDate(info);
            return rev;
        }
        #endregion

        /// <summary>
        /// 验证合同日期
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateDate(ContractEntity info)
        {
            if (info.HasSaveProperty(it => it.StartDate) && info.HasSaveProperty(it => it.EndDate))
            {
                if (info.StartDate >= info.EndDate)
                {
                    info.AddErrorByName(typeof(ContractEntity).FullName, "InValidContractDate");
                    return false;
                }
            }
            return true;
        }

   
        /// <summary>
        /// 验证供应商
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateSupplier(ContractEntity info)
        {
            if (info.SaveType != SaveType.Add && info.SaveType != SaveType.Modify)
                return true;
            if (!info.HasSaveProperty(it => it.Supplier.Id))
                return true;
            if (info.Supplier != null && info.Supplier.Id != 0)
            {
                var supplier = Repository.Get<SupplierEntity>(info.Supplier.Id);
                if (supplier == null)
                {
                    info.AddErrorByName(typeof(ContractEntity).FullName, "NoExist");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(ContractEntity).FullName, "NoExist");
            return false;
        }
        #endregion
      
    }
}
