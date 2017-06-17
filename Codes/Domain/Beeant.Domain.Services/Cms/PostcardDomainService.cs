using System.Collections.Generic;
using Beeant.Domain.Entities.Cms;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;

namespace Beeant.Domain.Services.Cms
{
    public class PostcardDomainService : RealizeDomainService<PostcardEntity>
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

        #region 重写验证

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PostcardEntity info)
        {
            var rev = ValidateCms(info) ;
            return rev;
        }
  

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateCms(PostcardEntity info)
        {
            if (!info.HasSaveProperty(it => it.Cms.Id) || info.Cms == null ||
                info.Cms.Id == 0)
                return true;
            if (info.Cms != null && info.Cms.SaveType == SaveType.Add)
                return true;
            var account = Repository.Get<CmsEntity>(info.Cms.Id);
            if (account == null)
            {
                info.AddErrorByName(typeof(CmsEntity).FullName, "NoExist");
                return false;
            }
            if (!account.IsUsed)
            {
                info.AddErrorByName(typeof(CmsEntity).FullName, "UnUsed");
                return false;
            }
            return true;
        }

        #endregion
    }
}
