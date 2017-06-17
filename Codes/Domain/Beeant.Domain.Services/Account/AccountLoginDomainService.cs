using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Security;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Security;
using Beeant.Domain.Services.Utility;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Account
{
    public class AccountLoginDomainService :  ILoginDomainService
    {
        /// <summary>
        /// 存储实例
        /// </summary>
        public virtual IRepository Repository { get; set; }
        /// <summary>
        /// 验证码错误
        /// </summary>
        public virtual ICodeDomainService CodeDomainService { get; set; }
        #region 登入
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public virtual LoginEntity Login(LoginEntity login)
        {
            login.Errors = new List<ErrorInfo>();
            if (string.IsNullOrEmpty(login.Type) &&( string.IsNullOrEmpty(login.Name) || string.IsNullOrEmpty(login.Password)))
                login.AddError("NameOrPasswordEmpty");
            if (login.Errors == null || login.Errors.Count == 0)
            {
                var info = CheckLogin(login) ;
                if(info==null)
                    info= CheckMobileCodeLogin(login);
                if (info != null) FillLoginEntity(login, info);
            }
            return login;
        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="login"></param>
        protected virtual AccountEntity CheckLogin(LoginEntity login)
        {
            if (!string.IsNullOrEmpty(login.Type))
                return null;
            var info = new AccountEntity { Name = login.Name, Password = login.Password };
            info.SetEncryptPassword();
            var query = new QueryInfo();
            query.Query<AccountIdentityEntity>().Where(it => it.Number==login.Name)
                .Select(it => new object[] { it.Account.Id,it.Account.Name,
                    it.Account.Password, it.Account.IsUsed,it.Account.AccountNumbers.Select(s=>new object[] {s.Tag,s.Number}) });
            var infos = Repository.GetEntities<AccountIdentityEntity>(query);
            var account = infos?.FirstOrDefault()?.Account;
            if (account != null && account.IsUsed 
                && (string.IsNullOrWhiteSpace(login.Type) && 
                !string.IsNullOrWhiteSpace(account.Password) &&
                !string.IsNullOrWhiteSpace(info.Password) && account.Password.ToLower() == info.Password.ToLower() || login.Type == "ThirdParty"))
            {
                    return account;
            }
            login.AddError("NameOrPasswordError");
            return null;
        }

   
        /// <summary>
        /// 检查OPENid登录
        /// </summary>
        /// <returns></returns>
        protected virtual AccountEntity CheckMobileCodeLogin(LoginEntity login)
        {
            if (string.IsNullOrEmpty(login.Type) || login.Type.ToLower()!="mobile")
                return null;
            if (CodeDomainService.ValidateCode(IdentityEntity.MobileLoginTag, login.Name, CodeType.Mobile, login.Password))
            {
                var query = new QueryInfo();
                query.Query<AccountIdentityEntity>().Where(it => it.Number == login.Name)
                    .Select(it => new object[] { it.Account.Id, it.Account.Name, it.Account.Password, it.Account.IsUsed });
                var infos = Repository.GetEntities<AccountIdentityEntity>(query);
                var account = infos?.FirstOrDefault()?.Account;
                if (account != null && account.IsUsed )
                {
                    return account;
                }
            }
            return null;
        }
        /// <summary>
        /// 填充登入信息
        /// </summary>
        /// <param name="login"></param>
        /// <param name="info"></param>
        protected virtual void FillLoginEntity(LoginEntity login, AccountEntity info)
        {
            login.Identity = new IdentityEntity
            {
                Id = info.Id,
                Name = info.Name
           };
            if (info.AccountNumbers != null)
            {
                login.Identity.Numbers=new Dictionary<string, string>();
                foreach (var accountNumber in info.AccountNumbers)
                {
                    if(string.IsNullOrEmpty(accountNumber.Tag) || string.IsNullOrEmpty(accountNumber.Number)
                        || login.Identity.Numbers.ContainsKey(accountNumber.Tag))
                        continue;
                    login.Identity.Numbers.Add(accountNumber.Tag, accountNumber.Number);

                }
            }
        }



   
        #endregion

    
    }
}
