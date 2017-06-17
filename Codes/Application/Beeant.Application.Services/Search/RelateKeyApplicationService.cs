using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Search;

namespace Beeant.Application.Services.Search
{
    public class RelateKeyApplicationService : RealizeApplicationService<RelateKeyEntity>
    {
        #region 只读属性

        static protected readonly object Locker = new object();

        #endregion

        #region 实例方法

        /// <summary>
        /// 重写Save
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public override bool Save(IList<RelateKeyEntity> infos)
        {
            lock (Locker)
            {
                return base.Save(infos);
            }
        }

        #endregion
    }
}
