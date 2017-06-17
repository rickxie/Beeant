using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Account
{
    public class AccountNumberDomainService : RealizeDomainService<AccountNumberEntity>
    {

        #region 重写验证

        /// <summary>
        /// 批量验证
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<AccountNumberEntity> infos)
        {
            var temps =
                infos.Where(it => it.SaveType == SaveType.Add)
                     .ToList();
            if (temps.Count > 1)
            {
                foreach (var temp in temps)
                {
                    temp.AddErrorByName(typeof(BaseEntity).FullName, "NoAllowBatchSave");
                }
                return false;
            }
            return true;
        }
        

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(AccountNumberEntity info)
        {
            var rev = ValidateExist(info) && ValidateAccount(info);
            return rev;
        }

        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(AccountNumberEntity info)
        {
            if (info.Account == null || info.Account.SaveType == SaveType.Add)
                return true;
            var query = new QueryInfo();
            query.Query<AccountNumberEntity>().Where(it =>it.Account.Id==info.Account.Id && it.Name == info.Name && it.Number == info.Number)
                .Select(it => new object[] { it.Id });
            var infos= Repository.GetEntities<AccountNumberEntity>(query);
            if (infos == null || infos.Count == 0) return true;
            info.AddError("Exist");
            return info.Errors == null || info.Errors.Count == 0;
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(AccountNumberEntity info)
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
