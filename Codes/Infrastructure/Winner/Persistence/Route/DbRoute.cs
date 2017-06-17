using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Winner.Persistence.Compiler.Common;
using Winner.Persistence.Exceptions;
using Winner.Persistence.Translation;

namespace Winner.Persistence.Route
{
  

    public class DbRoute : IDbRoute
    {
        private IDictionary<string, DbRouteInfo> _dbRoutes = new Dictionary<string, DbRouteInfo>();

        public IDictionary<string, DbRouteInfo> DbRoutes
        {
            get { return _dbRoutes; }
            set
            {
                _dbRoutes = value;
                InitlizeRoutes();
            }
        }
        /// <summary>
        /// 活动路由
        /// </summary>
        private IDictionary<string, IList<ShardingInfo>> _dbPropertyRoutes = new Dictionary<string, IList<ShardingInfo>>();
        public IDictionary<string, IList<ShardingInfo>> DbPropertyRoutes
        {
            get { return _dbPropertyRoutes; }
            set { _dbPropertyRoutes = value; }
        }
  
        /// <summary>
        /// 初始化路由
        /// </summary>
        protected virtual void InitlizeRoutes()
        {
            if(DbRoutes==null)return;
            foreach (var dbRoute in DbRoutes)
            {
                if(dbRoute.Value.Shardings==null)continue;
                foreach (var sharding in dbRoute.Value.Shardings)
                {
                    if(sharding.ShardProperties==null)continue;
                    foreach (var shardProperty in sharding.ShardProperties)
                    {
                        var key = string.Format("{0}{1}", dbRoute.Key, shardProperty.PropertyName);
                        if (!DbPropertyRoutes.ContainsKey(key))
                            DbPropertyRoutes.Add(key, new List<ShardingInfo>());
                        DbPropertyRoutes[key].Add(sharding);
                    }
                }
            }
        }
        #region 读路由

        /// <summary>
        /// 得到读的路由
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IList<QueryInfo> GetRouteQueries(QueryInfo query)
        {
            if (string.IsNullOrEmpty(query.Object.RouteName) || !DbRoutes.ContainsKey(query.Object.RouteName))
                return null;
            var dbRoute = DbRoutes[query.Object.RouteName];
            query.DbRoute = dbRoute;
            var handleQueries = GetHandleQueries(dbRoute, query);
            if (handleQueries != null && handleQueries.Count > 0)
                return handleQueries;
            if (dbRoute.Rules == null || dbRoute.Rules.Count == 0 || dbRoute.Shardings == null || dbRoute.Shardings.Count == 0)
                return null;
            var names = query.GetRouteParameters(query.WhereExp);
            if (names != null && names.Count > 0)
            {
                foreach (var rule in dbRoute.Rules)
                {
                    if (!names.ContainsKey(rule.PropertyName))
                        continue;
                    var paramterValues = names[rule.PropertyName];
                    var result = new List<QueryInfo>();
                    foreach (var paramterValue in paramterValues)
                    {
                        if (paramterValue is Array)
                        {
                            var arr = paramterValue as Array;
                            foreach (var ar in arr)
                            {
                                if(ar==null)
                                    continue;
                                var value = rule.IsHash ? GenerateLongId(ar.ToString()) : ar;
                                var shardings = GetShardings(dbRoute, rule.PropertyName, value, false);
                                if (shardings == null || shardings.Count == 0) continue;
                                foreach (var sharding in shardings)
                                {
                                    var cloneQuery = GetQuery(dbRoute, query, sharding);
                                    if (cloneQuery != null &&
                                        result.Count(
                                            it =>
                                                it.GetDataBase == cloneQuery.GetDataBase &&
                                                it.GetTableName == cloneQuery.GetTableName) == 0)
                                        result.Add(cloneQuery);

                                }
                            }
                        }
                        else
                        {
                            if(paramterValue==null)
                                continue;
                            var value = rule.IsHash ? GenerateLongId(paramterValue.ToString()) : paramterValue;
                            var shardings = GetShardings(dbRoute, rule.PropertyName, value, false);
                            if (shardings == null || shardings.Count == 0) continue;
                            AppendQueryByShardings(result, dbRoute, shardings, query);
                        }
                    }
                    if (result.Count == 1)
                    {
                        result[0].PageIndex = query.PageIndex;
                        result[0].PageSize = query.PageSize;
                    }
                    else if (result.Count > 1 && dbRoute.TopCount > 0 && (query.PageSize + query.PageIndex * query.PageSize)>dbRoute.TopCount)
                    {
                        throw new LimitCountOverflowException(string.Format("Limit Count Is {0}", dbRoute.TopCount));
                    }
                    if (result.Count > 0)
                        return result;
                }
            }
            if (dbRoute.TopCount > 0 && (query.PageSize + query.PageIndex * query.PageSize) > dbRoute.TopCount)
            {
                throw new LimitCountOverflowException(string.Format("Limit Count Is {0}", dbRoute.TopCount));
            }
       
            return GetAllQueries(dbRoute,query);
        }

        /// <summary>
        /// 得到查询
        /// </summary>
        /// <param name="dbRoute"></param>
        /// <param name="shardings"></param>
        /// <param name="query"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual void AppendQueryByShardings(IList<QueryInfo> result, DbRouteInfo dbRoute, IList<ShardingInfo> shardings, QueryInfo query)
        {
            if (shardings.Count == 1)
            {
                var sharding = shardings[0];
                query.TableIndex = string.IsNullOrWhiteSpace(sharding.TableIndex)
                    ? sharding.TableIndex
                    : sharding.TableIndex.ToLower();
                query.GetDataBase = string.IsNullOrWhiteSpace(sharding.GetDataBase)
                    ? sharding.GetDataBase
                    : sharding.GetDataBase.ToLower();
                result.Add(query);
            }
            else
            {
                foreach (var sharding in shardings)
                {
                    var cloneQuery = GetQuery(dbRoute, query, sharding);
                    if (cloneQuery != null &&
                        result.Count(
                            it =>
                            it.GetDataBase == cloneQuery.GetDataBase &&
                            it.GetTableName == cloneQuery.GetTableName) == 0)
                        result.Add(cloneQuery);
                }
            }
        }
        /// <summary>
        /// 得到查询
        /// </summary>
        /// <param name="dbRoute"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual IList<QueryInfo> GetHandleQueries(DbRouteInfo dbRoute, QueryInfo query)
        {
            if (dbRoute.GetQueryShardingHandle == null)
                return null;
            var shardings = dbRoute.GetQueryShardingHandle(query);
            if (shardings != null && shardings.Count > 0)
            {
                var result = new List<QueryInfo>();
                if (shardings.Count == 1)
                {
                    var sharding = shardings[0];
                    query.TableIndex = string.IsNullOrWhiteSpace(sharding.TableIndex)
                        ? sharding.TableIndex
                        : sharding.TableIndex.ToLower();
                    query.GetDataBase = string.IsNullOrWhiteSpace(sharding.GetDataBase)
                        ? sharding.GetDataBase
                        : sharding.GetDataBase.ToLower();
                    result.Add(query);
                }
                else
                {
                    MergeQueries(result, dbRoute, query, shardings);
                }
                return result;
            }
            return null;
        }
        /// <summary>
        /// 得到所有路由
        /// </summary>
        /// <param name="dbRoute"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual IList<QueryInfo> GetAllQueries(DbRouteInfo dbRoute, QueryInfo query)
        {
            if (!dbRoute.IsReturnAllShardings)
                return null;
            var result = new List<QueryInfo>();
            foreach (var readSharding in dbRoute.Shardings)
            {
                var shardings = readSharding.GetAllShardings();
                if (shardings == null)
                    continue;
                MergeQueries(result, dbRoute, query, shardings);
            }
            return result;
        }

        /// <summary>
        /// 合并查询
        /// </summary>
        /// <param name="result"></param>
        /// <param name="dbRoute"></param>
        /// <param name="query"></param>
        /// <param name="shardings"></param>
        /// <returns></returns>
        protected virtual void MergeQueries(IList<QueryInfo> result, DbRouteInfo dbRoute,QueryInfo query,
            IList<ShardingInfo> shardings)
        {
            foreach (var sharding in shardings)
            {
                var cloneQuery = GetQuery(dbRoute, query, sharding);
                if (cloneQuery != null &&
                    result.Count(
                        it => it.GetDataBase == cloneQuery.GetDataBase && it.GetTableName == cloneQuery.GetTableName) ==
                    0)
                    result.Add(cloneQuery);
            }
     
        }
        /// <summary>
        /// 得到查询
        /// </summary>
        /// <param name="dbRoute"></param>
        /// <param name="query"></param>
        /// <param name="sharding"></param>
        /// <returns></returns>
        protected virtual QueryInfo GetQuery(DbRouteInfo dbRoute,QueryInfo query, ShardingInfo sharding)
        {
            if (sharding == null) return null;
             var result = new QueryInfo
                 {
                     DataCount = query.DataCount,
                     FromExp = query.FromExp,
                     GroupByExp = query.GroupByExp,
                     HavingExp = query.HavingExp,
                     IsDisinct = query.IsDisinct,
                     IsGreedyLoad = query.IsGreedyLoad,
                     IsLazyLoad = query.IsLazyLoad,
                     OrderByExp = query.OrderByExp,
                     PageSize = query.PageSize+query.PageIndex*query.PageSize,
                     RemoteQueries = query.RemoteQueries,
                     SelectExp = query.SelectExp,
                     WhereExp = query.WhereExp,
                     TableIndex = string.IsNullOrWhiteSpace(sharding.TableIndex) ? sharding.TableIndex : sharding.TableIndex.ToLower(),
                     GetDataBase =string.IsNullOrWhiteSpace(sharding.GetDataBase)? sharding.GetDataBase: sharding.GetDataBase.ToLower(),
                     Object=query.Object,
                     DbRoute= query.DbRoute
             };
            if (query.Parameters != null)
            {
                result.Parameters = new Dictionary<string, object>();
                foreach (var parameter in query.Parameters)
                {
                    result.Parameters.Add(parameter.Key, parameter.Value);
                }
            }
            if (query.Cache != null)
            {
                result.Cache = new CacheInfo
                {
                    Time = query.Cache.Time,
                    TimeSpan = query.Cache.TimeSpan
                };
            }
            return result;
        }

     

        #endregion

        #region 写路由

        /// <summary>
        /// 得到写的路由
        /// </summary>
        /// <param name="save"></param>
        /// <returns></returns>
        public virtual void SetRouteSaveInfo(SaveInfo save)
        {
            if (string.IsNullOrEmpty(save.Object.RouteName) || !DbRoutes.ContainsKey(save.Object.RouteName))
                return;
            var dbRoute = DbRoutes[save.Object.RouteName];
            if (dbRoute.GetSaveShardingHandle != null)
            {
                var sharding = dbRoute.GetSaveShardingHandle(save.Entity);
                if (sharding != null)
                {
                    save.SetDataBase =string.IsNullOrWhiteSpace(sharding.SetDataBase)? sharding.SetDataBase: sharding.SetDataBase.ToLower();
                    save.TableIndex = string.IsNullOrWhiteSpace(sharding.TableIndex) ? sharding.TableIndex : sharding.TableIndex.ToLower();
                    return;
                }
            }
            if (dbRoute.Rules == null || dbRoute.Rules.Count == 0 || dbRoute.Shardings == null ||
                dbRoute.Shardings.Count == 0)
                return;
            var rules = dbRoute.Rules.Where(it => it.IsSave);
            foreach (var rule in rules)
            {
                var valueObj = save.Entity.GetProperty(rule.PropertyName);
                if(valueObj==null)
                    continue;
                var value = rule.IsHash
                    ? GenerateLongId(valueObj.ToString())
                    : (long) valueObj;
                value = Math.Abs(value);
                var shardings = GetShardings(dbRoute, rule.PropertyName, value, true);
                if (shardings == null || shardings.Count == 0)
                    continue;
                var sharding = shardings.FirstOrDefault(it => it.IsWrite);
                if (sharding == null)
                    continue;
                save.SetDataBase = string.IsNullOrWhiteSpace(sharding.SetDataBase) ? sharding.SetDataBase : sharding.SetDataBase.ToLower(); 
                save.TableIndex = string.IsNullOrWhiteSpace(sharding.TableIndex) ? sharding.TableIndex : sharding.TableIndex.ToLower();
                break;
            }
        }

        #endregion
       
       
        /// <summary>
        /// 得到分片
        /// </summary>
        /// <returns></returns>
        protected virtual IList<ShardingInfo> GetShardings(DbRouteInfo dbRoute,string proertyName, object value,bool isSave)
        {
            var key = string.Format("{0}{1}", dbRoute.Name, proertyName);
            if (DbPropertyRoutes.ContainsKey(key))
            {
                var result = new List<ShardingInfo>();
                var shardings = DbPropertyRoutes[key];
                foreach (var sharding in shardings)
                {
                    var shardProperty = sharding.ShardProperties.FirstOrDefault(it => it.PropertyName == proertyName);
                    var tableIndex = shardProperty.GetTableIndex(value, sharding);
                    sharding.TableIndex = tableIndex;
                    var getDataBase= isSave ? sharding.GetDataBase : shardProperty.GetQueryDataBaseIndex(value, sharding);
                    var setDataBase=isSave? shardProperty.GetSaveDataBaseIndex(value, sharding) : sharding.SetDataBase;
                    if (tableIndex!=null)
                    {
                        result.Add(new ShardingInfo
                        {
                            TableIndex=tableIndex,
                            MaxTableIndex= sharding.MaxTableIndex,
                            SetDataBase= setDataBase,
                            GetDataBase= getDataBase,
                            IsWrite= sharding.IsWrite
                        });
                    }
                }
                if (result.Count > 0) return result;
            }
            return null;
        }

        /// <summary>
        /// 得到MD5加密
        /// </summary>
        /// <returns></returns>
        protected virtual string EncryptMd5(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var md5 = new MD5CryptoServiceProvider();
            byte[] bytValue = Encoding.UTF8.GetBytes(input);
            byte[] bytHash = md5.ComputeHash(bytValue);
            var sTemp = new StringBuilder();
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp.Append(bytHash[i].ToString("X").PadLeft(2, '0'));
            }
            return sTemp.ToString().ToLower();
        }
        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual long GenerateLongId(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;
            byte[] buffer = Encoding.Default.GetBytes(EncryptMd5(input));
            var i = BitConverter.ToInt64(buffer, 0);
            return i;
        }
    }

   
}
