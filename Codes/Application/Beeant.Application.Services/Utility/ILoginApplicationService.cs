
using Beeant.Domain.Entities.Utility;

namespace Beeant.Application.Services.Utility
{
    public interface ILoginApplicationService
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        LoginEntity Login(LoginEntity login);
 

    }
}
