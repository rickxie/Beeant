
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Utility;

namespace Beeant.Application.Services.Utility
{
    public interface IIdentityApplicationService
    {
  
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        TokenEntity Set<T>(T info) where T: IdentityEntity;
        /// <summary>
        /// 得到
        /// </summary>
        /// <returns></returns>
        T Get<T>() where T : IdentityEntity;
        /// <summary>
        /// 移除
        /// </summary>
        /// <returns></returns>
        bool Remove();
        /// <summary>
        /// 得到凭证
        /// </summary>
        TokenEntity GetToken();
        /// <summary>
        /// 得到
        /// </summary>
        /// <returns></returns>
        T Get<T>(string ticket) where T : IdentityEntity;
        /// <summary>
        /// 移除
        /// </summary>
        /// <returns></returns>
        bool Remove(string ticket);
   
    }
}
