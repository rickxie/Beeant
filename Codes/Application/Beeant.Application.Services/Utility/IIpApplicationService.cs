using Beeant.Domain.Entities.Utility;

namespace Beeant.Application.Services.Utility
{
    public interface IIpApplicationService
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="ip"></param>
        IpEntity Get(string ip);
    }
}
