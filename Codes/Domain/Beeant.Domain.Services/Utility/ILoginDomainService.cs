using Beeant.Domain.Entities.Utility;

namespace Beeant.Domain.Services.Utility
{
    public interface ILoginDomainService
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        LoginEntity Login(LoginEntity login);
    }
}
