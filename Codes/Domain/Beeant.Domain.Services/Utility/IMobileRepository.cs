using Beeant.Domain.Entities.Utility;

namespace Beeant.Domain.Services.Utility
{

    public interface IMobileRepository
     {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="info"></param>
        string Send(MobileEntity info);
     }
}
