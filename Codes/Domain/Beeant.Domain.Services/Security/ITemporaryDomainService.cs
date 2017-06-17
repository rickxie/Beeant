using System.Collections.Generic;
using Beeant.Domain.Entities.Security;
using Winner.Persistence;

namespace Beeant.Domain.Services.Security
{
    public interface ITemporaryDomainService
    {
        /// <summary>
        /// 验证合法
        /// </summary>
        /// <returns></returns>
        bool Check(TemporaryEntity temporary);
        /// <summary>
        /// 验证合法
        /// </summary>
        /// <returns></returns>
        IList<IUnitofwork> Release(TemporaryEntity temporary);

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="temporary"></param>
        IList<IUnitofwork> Set(TemporaryEntity temporary);
    }
}
