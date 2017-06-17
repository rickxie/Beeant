

using Beeant.Domain.Entities.Utility;

namespace Beeant.Application.Services.Utility
{
    public interface IEmailApplicationService
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="info"></param>
        string Send(EmailEntity info);
    }
}
