using Beeant.Domain.Entities.Utility;

namespace Beeant.Domain.Services.Utility
{

    public interface IIpRepository
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="ip"></param>
        IpEntity Get(string ip);
     }
}
