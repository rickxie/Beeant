using System.Collections.Generic;
using Winner.Persistence.Translation;

namespace Winner.Persistence.Route
{
    public interface IDbRoute
    {
        /// <summary>
        /// 得到读路由
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<QueryInfo> GetRouteQueries(QueryInfo query);

        /// <summary>
        /// 得到写路由
        /// </summary>
        /// <param name="save"></param>
        /// <returns></returns>
        void SetRouteSaveInfo(SaveInfo save);
    }
}
