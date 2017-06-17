using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Account
{
    public class AccountIdentityDomainService : RealizeDomainService<AccountIdentityEntity>
    {
        /// <summary>
        /// 批量验证
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<AccountIdentityEntity> infos)
        {
            var temps =
                infos.Where(it => it.SaveType == SaveType.Add)
                    .GroupBy(it => it.Number)
                    .ToDictionary(it => it.Key,s=>s.Count());
            if (temps.Count(it => it.Value>1) > 1)
            {
                infos[0].AddErrorByName(typeof(BaseEntity).FullName, "NoAllowBatchSave");
                return false;
            }
            return true;
        }
        #region 重写验证

 

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(AccountIdentityEntity info)
        {
            var rev = ValidateExist(info) && ValidateAccount(info);
            return rev;
        }

        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(AccountIdentityEntity info)
        {
            var query = new QueryInfo();
            query.Query<AccountIdentityEntity>().Where(it => it.Number==info.Number)
                .Select(it => new object[] { it.Id });
            var infos= Repository.GetEntities<AccountIdentityEntity>(query);
            if (infos == null || infos.Count == 0) return true;
            info.AddError(string.Format("{0}Exist", info.Name));
            return info.Errors == null || info.Errors.Count == 0;
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(AccountIdentityEntity info)
        {
           var account = info.Account==null|| info.Account.SaveType == SaveType.Add
                                 ? info.Account
                                 : Repository.Get<AccountEntity>(info.Account.Id);
            if (account == null)
            {
                info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }
        #endregion

    }
}
