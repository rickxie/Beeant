using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Finance;
using System.Linq;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Finance
{
    public class BankDomainService : RealizeDomainService<BankEntity>
    {
        /// <summary>
        /// 批量验证
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<BankEntity> infos)
        {
            var temps =
                infos.Where(it => it.HasSaveProperty(s => s.Number))
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
        protected override bool ValidateAdd(BankEntity info)
        {
            var rev = ValidateAccount(info);
            if (rev) rev = ValidateNumberExist(info);
            return rev;
        }
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(BankEntity info)
        {
            var rev = ValidateAccount(info);
            if (rev) rev = ValidateNumberExist(info);
            return rev;
        }
        /// <summary>
        /// 检查是否存在名称
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateNumberExist(BankEntity info)
        {
            if (!info.HasSaveProperty(it => it.Number)) return true;
            var query = new QueryInfo();
            query.Query<BankEntity>().Where(it => it.Number == info.Number);
            var banks = Repository.GetEntities<BankEntity>(query);
            if (banks == null || banks.Count(it => !it.Id.Equals(info.Id)) == 0)
                return true;
            info.AddError("NumberExist");
            return false;
        }
        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(BankEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id!=0)
            {
                var account = Repository.Get<AccountEntity>(info.Account.Id);
                if (account == null)
                {
                    info.AddErrorByName(typeof (AccountEntity).FullName, "NoExist");
                    return false;
                }
                if (!account.IsUsed)
                {
                    info.AddErrorByName(typeof (AccountEntity).FullName, "UnUsed");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
            return false;
        }
      

    }
}
