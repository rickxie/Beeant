using Beeant.Domain.Entities.Utility;

namespace Beeant.Domain.Services.Utility
{

    public interface IEmailRepository
     {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="info"></param>
        string Send(EmailEntity info);
     }
}
