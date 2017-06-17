using System.Collections.Generic;
using Beeant.Domain.Entities.Security;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Security;
using Beeant.Domain.Services.Utility;
using Component.Extension;
using Winner;
using Winner.Filter;
using Winner.Persistence;

namespace Beeant.Application.Services.Utility
{
    public class LoginApplicationService : ILoginApplicationService
    {
        /// <summary>
        /// 登入实例
        /// </summary>
        public ILoginDomainService LoginDomainService { get; set; }
        /// <summary>
        /// 登入锁
        /// </summary>
        public ILockerDomainService LockerDomainService { get; set; }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public virtual LoginEntity Login(LoginEntity login)
        {
            var locker = new LockerEntity
            {
                Name = login.Name,
                Tag = login.LockerTag
            };
            if (!LockerDomainService.Check(locker))
            {
                login.Errors = login.Errors ?? new List<ErrorInfo>();
                login.Errors.AddList(locker.Errors);
                return login;
            }
            login = LoginDomainService.Login(login);
            if (login.Errors != null && login.Errors.Count != 0)
            {
                var unitofworks = LockerDomainService.Set(locker);
                if (unitofworks != null)
                {
                    Creator.Get<IContext>().Commit(unitofworks);
                }
            }
            return login;
        }

     

    }
}
