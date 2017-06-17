
using Beeant.Application.Dtos.Account;
using Beeant.Domain.Entities.Security;

namespace Beeant.Application.Services.Account
{
 
    public interface IMobileApplicationService
    {
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        MobileDto Load(MobileDto dto);
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        MobileDto SendCode(MobileDto dto);
        /// <summary>
        /// 绑定邮箱
        /// </summary>
        /// <param name="dto"></param>
        MobileDto Action(MobileDto dto);

    }
}
