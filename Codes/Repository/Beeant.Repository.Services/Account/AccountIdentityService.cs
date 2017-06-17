using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Services.Account;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Repository.Services.Account
{
    public class AccountIdentityService : Repository, IAccountIdentityContract
    {
        public virtual string Save(string info)
        {
            var accountIdentity= info.DeserializeJson<AccountIdentityEntity>();
           
            if (ValidateExist(accountIdentity))
            {
                var unitofworks = base.Save(accountIdentity);
                Winner.Creator.Get<IContext>().Commit(unitofworks);
            }
            return accountIdentity.SerializeJson();
        }
        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(AccountIdentityEntity info)
        {
            if (info == null || info.Account == null || info.Account.Id == 0)
            {
                info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
                return false;
            }
            var query = new QueryInfo();
            query.Query<AccountIdentityEntity>().Where(it => it.Id != info.Id
                                               && it.Number == info.Number);
            var infos = GetEntities<AccountIdentityEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError(string.Format("{0}Exist",info.Name));
                return false;
            }
            return true;
        }
 
    }
}
