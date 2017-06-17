using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Utility;

namespace Beeant.Domain.Services.Utility
{

     public interface IIdentityRepository
     {
         /// <summary>
         /// 设置
         /// </summary>
         /// <param name="info"></param>
         /// <returns></returns>
         TokenEntity Set<T>(T info) where T : IdentityEntity;
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
