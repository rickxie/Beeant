using System.Linq;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Hr;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Hr
{
    public class HrDomainService : RealizeDomainService<HrEntity>
    {

 
        #region 重写验证

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(HrEntity info)
        {
            var rev = ValidateAccount(info) && ValidateAccountExist(info);
            return rev;
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(HrEntity info)
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
        protected virtual bool ValidateAccountExist(HrEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            var query = new QueryInfo();
            query.Query<HrEntity>().Where(it => it.Account.Id == info.Account.Id).Select(it => it.Id);
            var infos = Repository.GetEntities<HrEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("AccountHasHr");
                return false;
            }
            return true;
        }
        #endregion

    }
}
