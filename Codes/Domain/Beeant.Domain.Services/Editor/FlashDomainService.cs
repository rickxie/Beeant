using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Editor;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;

namespace Beeant.Domain.Services.Editor
{
    public class FlashDomainService : RealizeDomainService<FlashEntity>
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

 

        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(FlashEntity info)
        {
            return ValidateAccount(info) && ValidateFolder(info,null);
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(FlashEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account == null || info.Account.Id == 0)
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id != 0)
            {
                var account = Repository.Get<AccountEntity>(info.Account.Id);
                if (account == null)
                {
                    info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
                    return false;
                }
                if (!account.IsUsed)
                {
                    info.AddErrorByName(typeof(AccountEntity).FullName, "UnUsed");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
            return false;
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateFolder(FlashEntity info, FlashEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Folder.Id) || info.Folder == null || info.Folder.Id == 0)
                return true;
            if (info.Folder != null && info.Folder.SaveType == SaveType.Add)
                return true;
            if (info.Folder != null && info.Folder.Id != 0)
            {
                if (dataEntity != null && dataEntity.Folder != null && dataEntity.Folder.Id == info.Folder.Id)
                    return true;
                var folder = Repository.Get<FolderEntity>(info.Folder.Id);
                if (folder == null)
                {
                    info.AddErrorByName(typeof(FolderEntity).FullName, "NoExist");
                    return false;
                }
                if (folder.Type != FolderType.Flash)
                {
                    info.AddErrorByName(typeof(FolderEntity).FullName, "FolderTypeNotMatch");
                    return false;
                }
                info.Account = info.Account == null && dataEntity != null ? dataEntity.Account : info.Account;
                if (info.Account != null && folder.Account == null || info.Account == null && folder.Account != null ||
                    info.Account != null && folder.Account != null && folder.Account.Id != info.Account.Id)
                {
                    info.AddErrorByName(typeof(FolderEntity).FullName, "FolderAccountNotMatch");
                    return false; 
                }
                return true;
            }
            info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
            return false;
        }
        #endregion

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(FlashEntity info)
        {
            var dataEntity = Repository.Get<FlashEntity>(info.Id);
            return ValidateFolder(info, dataEntity);
        }





        #endregion
    }
}
