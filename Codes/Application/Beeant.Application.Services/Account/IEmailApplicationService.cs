
using Beeant.Application.Dtos.Account;
using Beeant.Domain.Entities.Security;

namespace Beeant.Application.Services.Account
{
 
    public interface IEmailApplicationService
    {
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        EmailDto Load(EmailDto dto);
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        EmailDto SendCode(EmailDto dto);
        /// <summary>
        /// 绑定邮箱
        /// </summary>
        /// <param name="dto"></param>
        EmailDto Action(EmailDto dto);

    }
}
