using System.Collections.Generic;
using Beeant.Application.Services;
using Dependent;
using Winner.Persistence;

namespace Beeant.Basic.Services.Common.Extension
{
    public class QueryBasicInfo<T> : QueryInfo<T>
    {
        #region 执行的查询

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <returns></returns>
        public override IList<T> ToList()
        {
            return Ioc.Resolve<IApplicationService, T>().GetEntities<T>(this);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public override TResult Execute<TResult>()
        {
            return Ioc.Resolve<IApplicationService, TResult>().Execute<TResult>(this);
        }

       

        #endregion

    
    }
 
}
