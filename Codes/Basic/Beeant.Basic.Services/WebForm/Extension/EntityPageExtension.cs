using System.Collections.Generic;
using System.Web.UI;
using Beeant.Application.Services;
using Beeant.Domain.Entities;
using Dependent;
using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class EntityPageExtension
    {
        #region 非权限查询
        public static IList<T> GetEntities<T>(this Page page, QueryInfo query) where T : BaseEntity
        {
            return Ioc.Resolve<IApplicationService, T>().GetEntities<T>(query);
        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetEntity<T>(this Page page, long id) where T : BaseEntity
        {
            return Ioc.Resolve<IApplicationService, T>().GetEntity<T>(id);

        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool SaveEntity<T>(this Page page, T info) where T : BaseEntity
        {
            return Ioc.Resolve<IApplicationService, T>().Save(info);

        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="infos"></param>
        /// <returns></returns>
        public static bool SaveEntities<T>(this Page page, IList<T> infos) where T : BaseEntity
        {
            return Ioc.Resolve<IApplicationService, T>().Save(infos);

        }
        #endregion

   
    }
}
