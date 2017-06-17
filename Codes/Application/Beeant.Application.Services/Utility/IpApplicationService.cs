using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Utility;

namespace Beeant.Application.Services.Utility
{
    public class IpApplicationService :IIpApplicationService
    {
        public IIpRepository IpRepository { get; set; }
        /// <summary>
        /// 得到IP地址信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public IpEntity Get(string ip)
        {
            return IpRepository.Get(ip);
        }
    }
}
