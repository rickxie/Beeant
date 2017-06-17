using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Winner.Persistence.Compiler.Common;
using Winner.Persistence.Data;
using Winner.Persistence.Delay;
using Winner.Persistence.Linq;
using Winner.Persistence.Relation;
using Winner.Persistence.Route;

namespace Winner.Persistence.Translation
{
    public class Executor : IExecutor
    {
      

        #region 属性

        /// <summary>
        /// 编译器实例
        /// </summary>
        public IDataBase DataBase { get; set; }

        /// <summary>
        /// 编译器实例
        /// </summary>
        public IDbRoute DbRoute { get; set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public Executor()
        { 
        }

        /// <summary>
        /// 缓存实例，同步信息实例，编译器实例,ORM信息实例
        /// </summary>
        /// <param name="dataBase"></param>
        public Executor(IDataBase dataBase)
        {
            DataBase = dataBase;

        }
        #endregion

        #region 接口的实现

        #region 查询

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="query"></param>
        /// <param name="context"></param>
        /// <param name="isLazyLoadExecute"></param>
        /// <returns></returns>
        public virtual T GetInfos<T>(OrmObjectInfo obj, QueryInfo query, IContext context,  bool isLazyLoadExecute = false)
        {
            query.Object = obj;
            query.GetDataBase = obj.GetDataBase;
            if (!string.IsNullOrEmpty(obj.RouteName))
            {
                var result = new List<T>();
                var queries = DbRoute.GetRouteQueries(query);
                if (queries != null)
                {
                    if (queries.Count == 1)
                    {
                        var infos= GetResult<T>(obj, queries[0], context, isLazyLoadExecute);
                        query.DataCount += queries[0].DataCount;
                        query.Sql = queries[0].Sql;
                        return infos;
                    }
                    query.DataCount = 0;
                    BeginInvokeQuery(obj,query,context,isLazyLoadExecute,result,queries,0);
                    query.Sql = queries[0].Sql;
                    return MergeResult(result, obj, query);
                }
            }
            return GetResult<T>(obj, query, context, isLazyLoadExecute);
        }

        /// <summary>
        /// 异步调用
        /// </summary>
        /// <param name="isLazyLoadExecute"></param>
        /// <param name="result"></param>
        /// <param name="queries"></param>
        /// <param name="i"></param>
        /// <param name="obj"></param>
        /// <param name="query"></param>
        /// <param name="context"></param>
        protected virtual void BeginInvokeQuery<T>(OrmObjectInfo obj, QueryInfo query, IContext context, bool isLazyLoadExecute,IList<T> result, IList<QueryInfo> queries, int i)
        {
            if(i>= queries.Count)
                return;
            Func<OrmObjectInfo, QueryInfo, IContext, bool, T> func = GetResult<T>;
            var asyncResult = func.BeginInvoke(obj, queries[i], context, isLazyLoadExecute, null, null);
            BeginInvokeQuery(obj, query, context, isLazyLoadExecute, result, queries, i + 1);
            var r = func.EndInvoke(asyncResult);
            result.Add(r);
            query.DataCount += queries[i].DataCount;
        }
        /// <summary>
        /// 得到结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="query"></param>
        /// <param name="context"></param>
        /// <param name="isLazyLoadExecute"></param>
        /// <returns></returns>
        protected virtual T GetResult<T>(OrmObjectInfo obj, QueryInfo query, IContext context,bool isLazyLoadExecute = false)
        {
            OrmDataBaseInfo db = DataBase.GetDataBase(query.GetDataBase).GetAllGetOrmDataBase().FirstOrDefault();
            var result = ExecuteQuery<T>(query, obj, db);
            if (query.IsLazyLoad)
                LazyLoad(result, obj, context, isLazyLoadExecute);
            RemoteLoad(result, query.RemoteQueries == null ? null : query.RemoteQueries.Values.ToList(), obj, isLazyLoadExecute);
            return result;
        }
        #region 合并路由
 

        /// <summary>
        /// 合并路由
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results"></param>
        /// <param name="obj"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual T MergeResult<T>(IList<T> results,OrmObjectInfo obj, QueryInfo query)
        {
            var infos = new ArrayList();
            foreach (var result in results)
            {
                var ts = result as IEnumerable<EntityInfo>;
                if (ts == null) continue;
                foreach (var t in ts)
                {
                    infos.Add(t);
                }
            }
            var type = Type.GetType(obj.ObjectName);
            return (T)FilterResult(infos.ToArray(type), type, query);
        }

        /// <summary>
        /// 过滤数据
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="type"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual object FilterResult(Array infos, Type type, QueryInfo query)
        {
            if (string.IsNullOrEmpty(query.OrderByExp) && query.PageSize <= 0)
                return infos;
            var temps = infos.AsQueryable();
            if (!string.IsNullOrEmpty(query.OrderByExp))
            {
                var orderExps = query.OrderByExp.Split(',');
                var i = 0;
                foreach (var orderExp in orderExps)
                {
                    var exps = orderExp.Split(' ');
                    var pName = exps[0].Trim();
                    var tyName = exps.Length > 1 ? exps[1].Trim().ToLower() : "asc";
                    switch (tyName)
                    {
                        case "asc":
                            temps = i == 0 ? temps.OrderBy(type,pName) : temps.ThenBy(type, pName);
                            break;
                        case "desc":
                            temps = i == 0 ? temps.OrderByDescending(type, pName) : temps.ThenByDescending(type, pName);
                            break;
                    }
                }
            }
            if (query.PageSize > 0)
                temps = temps.Skip(query.PageIndex * query.PageSize).Take(query.PageSize);
            var result = new ArrayList();
            foreach (var t in temps)
            {
                result.Add(t);
            }
            return result.ToArray(type);
        }
        #endregion

        #region 延迟加载

        /// <summary>
        /// 延迟加载
        /// </summary>
        /// <param name="result"></param>
        /// <param name="obj"></param>
        /// <param name="context"></param>
        /// <param name="isLoad"></param>
        protected virtual void LazyLoad(object result, OrmObjectInfo obj,IContext context, bool isLoad = false)
        {
            var ormProperties =
                obj.Properties.Where(it => it.Map != null && !it.Map.IsGreedyLoad && !it.Map.IsRemote && it.Map.IsLazyLoad)
                   .ToList();
            if (ormProperties.Count == 0) return;
            var infos = result as IEnumerable<EntityInfo>;
            if (infos == null || infos.Count()==0) return;
            foreach (var ormProperty in ormProperties)
            {
                var entities = new Dictionary<object, object>();
                foreach (var info in infos)
                {
                    entities.Add(info,
                                 ormProperty.Map.MapType == OrmMapType.OneToMany
                                     ? info
                                     : info.GetProperty(ormProperty.PropertyName));
                }
                var query = GetLazyLoadQuery(infos, ormProperty);
                foreach (var info in infos)
                {
                    var proxyObject = new Proxy(entities, ormProperty, context, this, isLoad, info, info.GetType().GetProperty(ormProperty.PropertyName).PropertyType, query);
                    var proxyValue = proxyObject.GetProxy();
                    info.GetType().GetProperty(ormProperty.PropertyName).SetValue(info, proxyValue, null);
                    if (isLoad)
                    {
                        proxyValue.ToString();
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 得到查询对象
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="ormProperty"></param>
        protected virtual QueryInfo GetLazyLoadQuery(IEnumerable<EntityInfo> infos, OrmPropertyInfo ormProperty)
        {
            var ids = infos.Select(it => it.GetProperty(ormProperty.Map.ObjectProperty.PropertyName)).ToList();
            var query = new QueryInfo { IsLazyLoad = true };
            query.From(ormProperty.Map.GetMapObject().ObjectName);
            if (ids.Count == 0)
            {
                query.Where(string.Format("{0}==@MapId", ormProperty.Map.MapObjectProperty.PropertyName))
                     .SetParameter("MapId", ids[0]);
            }
            else
            {
                query.Where(string.Format("@MapIds.Contains({0})", ormProperty.Map.MapObjectProperty.PropertyName))
                     .SetParameter("MapIds", ids.ToArray());
            }
            return query;
        }
        #endregion

        #region 远程加载

        /// <summary>
        /// 延迟加载
        /// </summary>
        /// <param name="result"></param>
        /// <param name="remoteQueries"></param>
        /// <param name="obj"></param>
        /// <param name="isExcuteLazy"></param>
        protected virtual void RemoteLoad(object result, IList<RemoteQueryInfo> remoteQueries, OrmObjectInfo obj,bool isExcuteLazy)
        {
            if (remoteQueries == null || remoteQueries.Count == 0 || result == null)
                return;
            var infos = result as IEnumerable<EntityInfo>;
            if (infos == null)
                return;
            foreach (var remoteQuery in remoteQueries)
            {
                OrmPropertyInfo ormProperty;
                var tempInfos = GetRemoteEntities(infos, remoteQuery.PropertyName, obj, out ormProperty);
                if (ormProperty == null || ormProperty.Map == null || tempInfos==null || tempInfos.Count()==0)
                    continue;
                if (remoteQuery.IsLazyLoad && ormProperty.Map.IsLazyLoad)
                {
                    var entities = new Dictionary<object, object>();
                    foreach (var info in tempInfos)
                    {
                        entities.Add(info, info.GetProperty(ormProperty.PropertyName));
                    }
                    var query = GetRemoteQuery(infos, remoteQuery, ormProperty);
                    foreach (var info in tempInfos)
                    {
                        var proxyObject = new Proxy(entities, ormProperty, null, this, isExcuteLazy, info, info.GetType().GetProperty(ormProperty.PropertyName).PropertyType,
                            query);
                        var proxyValue = proxyObject.GetProxy();
                        info.GetType().GetProperty(ormProperty.PropertyName).SetValue(info, proxyValue, null);
                        if (isExcuteLazy)
                        {
                            proxyValue.ToString();
                            break;
                        }
                    }
                }
                else
                {
                    var routeInfos = GetRemoteInfos(tempInfos, remoteQuery, ormProperty);
                    SetRemoteProperies(tempInfos, routeInfos, ormProperty);
                }
            }
        }

        /// <summary>
        /// 得到远程查询实体
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="propertyName"></param>
        /// <param name="obj"></param>
        /// <param name="ormProperty"></param>
        /// <returns></returns>
        protected virtual IEnumerable<EntityInfo> GetRemoteEntities(IEnumerable<EntityInfo> infos, string propertyName, OrmObjectInfo obj, out OrmPropertyInfo ormProperty)
        {
            if (!propertyName.Contains("."))
            {
                ormProperty = obj.GetPropertyInfo(propertyName);
                return infos;
            }
            var name = propertyName.Substring(0, propertyName.IndexOf("."));
            ormProperty = obj.GetPropertyInfo(name);
            propertyName = propertyName.Substring(propertyName.IndexOf(".") + 1,
                                                  propertyName.Length - propertyName.IndexOf(".") - 1);
            if (ormProperty.Map == null) return infos;
            var result = new List<EntityInfo>();
            foreach (var info in infos)
            {
                if (ormProperty.Map.MapType == OrmMapType.OneToOne)
                {
                    var t = info.GetProperty(name) as EntityInfo;
                    if(t==null) continue;
                    result.Add(t);
                }
                else if (ormProperty.Map.MapType == OrmMapType.OneToMany)
                {
                    var ts = info.GetProperty(name) as IEnumerable<EntityInfo>;
                    if(ts==null) continue;
                    result.AddRange(ts);
                }
            }
            if (!string.IsNullOrEmpty(propertyName))
            {
                return GetRemoteEntities(result, propertyName, ormProperty.Map.GetMapObject(), out ormProperty);
            }
            return result;
        }

        /// <summary>
        /// 得到延迟的加载条件
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="remoteQuery"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        protected virtual IList<EntityInfo> GetRemoteInfos(IEnumerable<EntityInfo> infos, RemoteQueryInfo remoteQuery,
                                                           OrmPropertyInfo property)
        {
            var query = GetRemoteQuery(infos, remoteQuery, property);
            var remoteInfos = GetInfos<IList<EntityInfo>>(property.Map.GetMapObject(), query, null);
            return remoteInfos;
        }
        /// <summary>
        /// 得到远程查询条件
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="remoteQuery"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        protected virtual QueryInfo GetRemoteQuery(IEnumerable<EntityInfo> infos, RemoteQueryInfo remoteQuery, OrmPropertyInfo property)
        {
            var query = new QueryInfo { SelectExp = remoteQuery.SelectExp,Parameters = remoteQuery.Parameters,OrderByExp=remoteQuery.OrderByExp};
            var pName = "_Ids";
            var ids =
                (from object info in infos select info.GetProperty(property.Map.ObjectProperty.PropertyName)).ToArray();
            query.Where(string.Format("@{0}.Contains({1})", pName, property.Map.MapObjectProperty.PropertyName))
                 .SetParameter(pName, ids);
            if (!string.IsNullOrEmpty(remoteQuery.WhereExp))
            {
                query.WhereExp = string.Format("{0} && {1}",query.WhereExp, remoteQuery.WhereExp);
            }
            if (!string.IsNullOrEmpty(query.SelectExp))
                query.SelectExp = string.Format("{0},{1}", property.Map.MapObjectProperty.PropertyName,
                                                query.SelectExp);
            return query;
        }

        /// <summary>
        /// 设置延迟属性
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="remoteInfos"></param>
        /// <param name="property"></param>
        protected virtual void SetRemoteProperies(IEnumerable<EntityInfo> infos, IList<EntityInfo> remoteInfos,
                                                  OrmPropertyInfo property)
        {
            if (remoteInfos == null || remoteInfos.Count == 0)
                return;
            if (property.Map.MapType == OrmMapType.OneToOne)
            {
                foreach (var info in infos)
                {
                    var id = info.GetProperty(property.Map.ObjectProperty.PropertyName);
                    if(id==null) continue;
                    foreach (var entityInfo in remoteInfos)
                    {
                        if (id.Equals(entityInfo.GetProperty(property.Map.MapObjectProperty.PropertyName)))
                        {
                            info.SetProperty(property.PropertyName, entityInfo);
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (var info in infos)
                {
                    var id = info.GetProperty(property.Map.ObjectProperty.PropertyName);
                    if (id == null) continue;
                    var arrayList = new ArrayList();
                    foreach (var entityInfo in remoteInfos)
                    {
                        if (id.Equals(entityInfo.GetProperty(property.Map.MapObjectProperty.PropertyName)))
                        {
                            arrayList.Add(entityInfo);
                        }
                    }
                    var type = Type.GetType(property.Map.GetMapObject().ObjectName);
                    if (type == null) continue;
                    info.SetProperty(property.PropertyName, arrayList.ToArray(type));
                }
            }
        }

        #endregion
        #endregion

        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="unitOfWorks"></param>
        /// <returns></returns>
        public virtual IList<IUnitofwork> Save(IList<SaveInfo> infos, IList<IUnitofwork> unitOfWorks=null)
        {
            IDictionary<OrmDataBaseInfo,IList<SaveInfo>> dbs = GetOrmDataBases(infos);
            return GetUnitofworks(dbs, unitOfWorks);
        }

   

        /// <summary>
        /// 执行查询存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual T ExecuteQuery<T>(string name, string commandText, CommandType commandType, params object[] parameters)
        {
            var db = DataBase.GetDataBase(name).GetAllGetOrmDataBase().FirstOrDefault();
            var compiler = DataBase.GetCompiler(db);
            return compiler.ExecuteQuery<T>(db, commandText,commandType, parameters);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="name"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual int ExecuteCommand(string name, string commandText, CommandType commandType, params object[] parameters)
        {
            var db = DataBase.GetDataBase(name).GetAllSetOrmDataBase().FirstOrDefault();
            var compiler = DataBase.GetCompiler(db);
            return compiler.ExecuteCommand(db, commandText, commandType,parameters);
        }

        #endregion

        #region 查询操作

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        protected virtual T ExecuteQuery<T>(QueryInfo query, OrmObjectInfo obj, OrmDataBaseInfo db)
        {
            var compiler = DataBase.GetCompiler(db);
            return compiler.GetInfos<T>(db, obj, query);
        }

   
    
        #endregion

        #region  存储操作

        /// <summary>
        /// 设置DB信息
        /// </summary>
        /// <param name="infos"></param>
        protected virtual IDictionary<OrmDataBaseInfo, IList<SaveInfo>> GetOrmDataBases(IList<SaveInfo> infos)
        {
            IDictionary<OrmDataBaseInfo, IList<SaveInfo>> dbs = new Dictionary<OrmDataBaseInfo, IList<SaveInfo>>();
            foreach (var info in infos)
            {
                FillDataBase(dbs, info);
            }
            return dbs;
        }

        /// <summary>
        /// 得到事务
        /// </summary>
        /// <param name="dbs"></param>
        /// <param name="unitOfWorks"></param>
        /// <returns></returns>
        protected virtual IList<IUnitofwork> GetUnitofworks(IDictionary<OrmDataBaseInfo, IList<SaveInfo>> dbs, IList<IUnitofwork>  unitOfWorks)
        {
            unitOfWorks =unitOfWorks?? new List<IUnitofwork>();
            foreach (var db in dbs)
            {
                var compiler = DataBase.GetCompiler(db.Key);
                compiler.AddUnitofwork(db.Key,db.Value, unitOfWorks);
            }
            return unitOfWorks;
        }

        /// <summary>
        /// 填充需要连接的数据库
        /// </summary>
        /// <param name="dbs"></param>
        /// <param name="info"></param>
        protected virtual void FillDataBase(IDictionary<OrmDataBaseInfo, IList<SaveInfo>> dbs, SaveInfo info)
        {
            OrmObjectInfo obj = info.Object;
            if (!string.IsNullOrEmpty(obj.RouteName))
            {
                DbRoute.SetRouteSaveInfo(info);
            }
            var db = DataBase.GetDataBase(info.SetDataBase).GetAllSetOrmDataBase().FirstOrDefault();
            if (dbs.ContainsKey(db))
            {
                dbs[db].Add(info);
            }
            else
            {
                var ts = new List<SaveInfo> { info };
                dbs.Add(db, ts);
            }
        }


        #endregion


    }
}
