using Beeant.Domain.Entities;
using Configuration;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Management;
using Beeant.Domain.Entities.Utility;

namespace Beeant.Domain.Services.Utility
{
    public class LoginDomainService : ILoginDomainService
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public virtual LoginEntity Login(LoginEntity login)
        {
            login=Check(login);
            if (login.Errors==null || login.Errors.Count == 0)
                login.Identity = new IdentityEntity { Id = 0,Name = "System"};
            return login;
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="login"></param>
        protected virtual LoginEntity Check(LoginEntity login)
        {
            if (string.IsNullOrEmpty(login.Name) || string.IsNullOrEmpty(login.Password))
                login.AddError("NameOrPasswordEmpty");
            string key = string.Format("uid={0};pwd={1}", login.Name, login.Password);
            if (!key.Equals(ConfigurationManager.GetSetting<string>("LoginKey")))
                login.AddError("NameOrPasswordError");
            return login;
        }
    }
}
