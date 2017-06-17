using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Authority;
using Winner.Persistence;

namespace Beeant.Domain.Services.Authority
{
    public class RoleDomainService : RealizeDomainService<RoleEntity>
    {
        protected override bool ValidateAdd(RoleEntity info)
        {
            return ValidateAccount(info);
        }
        /// <summary>
        /// 验证角色
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(RoleEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && (info.Account.SaveType == SaveType.Add || info.Account.Id==0))
                return true;
            if (info.Account != null && info.Account.Id != 0)
            {
                if (Repository.Get<AccountEntity>(info.Account.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
            return false;
        }
    }
}
