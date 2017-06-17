using System.Collections.Generic;
using Beeant.Domain.Entities.Security;
using Winner.Persistence;

namespace Beeant.Domain.Services.Security
{
    public interface ILockerDomainService
    {
        /// <summary>
        /// 验证合法
        /// </summary>
        /// <returns></returns>
        bool Check(LockerEntity locker);
        /// <summary>
        /// 验证合法
        /// </summary>
        /// <returns></returns>
        IList<IUnitofwork> Release(LockerEntity locker);

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="locker"></param>
        IList<IUnitofwork> Set(LockerEntity locker);
    }
}
