using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Cms;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;

namespace Beeant.Domain.Services.Cms
{
    public class ContentDomainService : RealizeDomainService<ContentEntity>
    {

        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
               new FileEntity {FilePropertyName = "FileName",BytePropertyName = "FileByte"},
               new FileEntity {FilePropertyName = "AttachmentName",BytePropertyName = "AttachmentByte"}
            };

        public override IList<FileEntity> FileProperties
        {
            get { return _fileProperties; }
            set { _fileProperties = value; }
        }
        #region 加载
        private IDictionary<string, ItemLoader<ContentEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<ContentEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<ContentEntity>>
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
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(ContentEntity info)
        {
            var rev = ValidateClass(info, null) ;
            return rev;
        }
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(ContentEntity info)
        {
            var dataEntity = Repository.Get<ContentEntity>(info.Id);
            var rev = ValidateClass(info, dataEntity) ;
            return rev;
        }

        

        #endregion
        /// <summary>
        /// 验证员工
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateClass(ContentEntity info, ContentEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Class.Id))
                return true;
            if (info.Class != null && info.Class.SaveType == SaveType.Add)
                return true;
            if (info.Class != null && info.Class.Id!=0)
            {
                if (dataEntity != null && dataEntity.Class != null && dataEntity.Class.Id == info.Class.Id)
                    return true;
                info.Class = Repository.Get<ClassEntity>(info.Class.Id);
                if (info.Class == null)
                {
                    info.AddErrorByName(typeof(ClassEntity).FullName, "NoExist");
                    return false;
                }
                if (!info.Class.IsUsed)
                {
                    info.AddErrorByName(typeof(ClassEntity).FullName, "UnUsed");
                    return false;
                }
                if (!info.Class.IsPublish)
                {
                    info.AddErrorByName(typeof(ClassEntity).FullName, "UnPublish");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(ClassEntity).FullName, "NoExist");
            return false;
        }
    
    }
}
