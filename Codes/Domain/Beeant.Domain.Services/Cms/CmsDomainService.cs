using System.Linq;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Cms;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Cms
{
    public class CmsDomainService : RealizeDomainService<CmsEntity>
    {

 
        #region 重写验证

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(CmsEntity info)
        {
            var rev = ValidateAccount(info) && ValidateAccountExist(info);
            return rev;
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(CmsEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account == null ||
                info.Account.Id == 0)
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
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
 
        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccountExist(CmsEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            var query = new QueryInfo();
            query.Query<CmsEntity>().Where(it => it.Account.Id == info.Account.Id).Select(it => it.Id);
            var infos = Repository.GetEntities<CmsEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("AccountHasCms");
                return false;
            }
            return true;
        }
        #endregion

    }
}
