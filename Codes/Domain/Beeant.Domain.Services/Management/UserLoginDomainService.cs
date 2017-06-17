using System.Linq;
using Beeant.Domain.Entities.Management;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Account;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Management
{
    public class UserLoginDomainService : AccountLoginDomainService
    {

   
        #region 登入
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public override LoginEntity Login(LoginEntity login)
        {
            login = base.Login(login);
            if (login == null || login.Identity==null || login.Errors != null && login.Errors.Count > 0)
                return login;
            var user = GetUser(login.Identity.Id);
            if (user == null)
            {
                login.AddError("NameOrPasswordError");
                login.Identity = null;
                return login;
            }
            return login;
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        protected virtual UserEntity GetUser(long accountId)
        {
            var query=new QueryInfo();
            query.Query<UserEntity>().Where(it => it.Account.Id == accountId && it.IsUsed).Select(it => it.Id);
            var infos = Repository.GetEntities<UserEntity>(query);
            return infos?.FirstOrDefault();
        }

        #endregion



    }
}
