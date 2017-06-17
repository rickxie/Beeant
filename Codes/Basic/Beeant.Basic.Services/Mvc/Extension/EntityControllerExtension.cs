using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Application.Services;
using Beeant.Domain.Entities;
using Dependent;
using Beeant.Basic.Services.Mvc.Bases;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Basic.Services.Mvc.Extension
{
    static public class EntityControllerExtension
    {
        #region 非权限查询
        public static IList<T> GetEntities<T>(this Controller controller, QueryInfo query) where T : BaseEntity
        {
            return Ioc.Resolve<IApplicationService, T>().GetEntities<T>(query);
        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetEntity<T>(this Controller controller, long id) where T : BaseEntity
        {
            return Ioc.Resolve<IApplicationService, T>().GetEntity<T>(id);

        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool SaveEntity<T>(this Controller controller, T info) where T : BaseEntity
        {
            return Ioc.Resolve<IApplicationService, T>().Save(info);

        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="infos"></param>
        /// <returns></returns>
        public static bool SaveEntities<T>(this Controller controller, IList<T> infos) where T : BaseEntity
        {
            return Ioc.Resolve<IApplicationService, T>().Save(infos);

        }
        #endregion

        #region 权限查询
        /// <summary>
        /// 得到实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IList<T> GetEntitiesByIdentity<T>(this BaseController controller, QueryInfo query) where T : BaseEntity
        {
            return GetEntitiesByIdentity<T>(controller, query, "Account.Id");
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetEntityByIdentity<T>(this BaseController controller, long id) where T : BaseEntity
        {
            var query = new QueryInfo();
            query.Query<T>().Where(it=>it.Id==id);
            var infos= GetEntitiesByIdentity<T>(controller,query);
            if (infos == null) return null;
            return infos.FirstOrDefault();
        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetEntityByIdentity<T>(this BaseController controller) where T : BaseEntity
        {
            var query = new QueryInfo();
            query.Query<T>();
            var infos = GetEntitiesByIdentity<T>(controller, query);
            if (infos == null) return null;
            return infos.FirstOrDefault();
        }
        /// <summary>
        /// 得到实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="query"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IList<T> GetEntitiesByIdentity<T>(this BaseController controller, QueryInfo query,string propertyName) where T : BaseEntity
        {
            var exp = string.Format("{0}==@AccountId", propertyName);
            query.WhereExp = !string.IsNullOrEmpty(query.WhereExp)
                                    ? string.Format("{0} && {1}", query.WhereExp, exp)
                                    : exp;
            query.SetParameter("AccountId",controller.Identity==null?0: controller.Identity.Id);
            return Ioc.Resolve<IApplicationService, T>().GetEntities<T>(query);
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="id"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetEntityByIdentity<T>(this BaseController controller, long id, string propertyName) where T : BaseEntity
        {
            var query = new QueryInfo();
            query.Query<T>().Where(it => it.Id == id);
            var infos = GetEntitiesByIdentity<T>(controller, query, propertyName);
            if (infos == null) return null;
            return infos.FirstOrDefault();
        }
 
        #endregion
    }
}
