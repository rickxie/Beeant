using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Editor;
using Beeant.Domain.Entities.Finance;
using Winner.Persistence;

namespace Beeant.Domain.Services.Editor
{
    public class FolderDomainService : RealizeDomainService<FolderEntity>
    {
      


        #region 重写验证


        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(FolderEntity info)
        {
            return ValidateAccount(info);
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(FolderEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account == null || info.Account.Id==0)
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id!=0)
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
 
        #endregion

        #endregion


    }
}
