using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Purchase;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;

namespace Beeant.Domain.Services.Purchase
{
    public class PurchaseAttachmentDomainService : RealizeDomainService<PurchaseAttachmentEntity>
    {
        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
               new FileEntity {FilePropertyName = "FileName",BytePropertyName = "FileByte"}
            };

        public override IList<FileEntity> FileProperties
        {
            get { return _fileProperties; }
            set { _fileProperties = value; }
        }
        #region 重写事务
        private IDictionary<string, ItemLoader<PurchaseAttachmentEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<PurchaseAttachmentEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<PurchaseAttachmentEntity>>
                    {
                     
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

     
        #endregion
        #region 重写验证

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PurchaseAttachmentEntity info)
        {
            return ValidatePurchase(info);
        }
    
        /// <summary>
        /// 验证采购
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidatePurchase(PurchaseAttachmentEntity info)
        {
            if (!info.HasSaveProperty(it => it.Purchase.Id))
                return true;
            if (info.Purchase != null && info.Purchase.SaveType == SaveType.Add)
                return true;
            if (info.Purchase != null && info.Purchase.Id!=0)
            {
                if (Repository.Get<PurchaseEntity>(info.Purchase.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(PurchaseEntity).FullName, "NoExist");
            return false;
        }
    
        #endregion


    }
}
