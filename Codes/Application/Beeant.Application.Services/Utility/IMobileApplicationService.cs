using Beeant.Domain.Entities.Utility;

namespace Beeant.Application.Services.Utility
{
    public interface IMobileApplicationService
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="info"></param>
        string Send(MobileEntity info);
    }
}
