using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Winner.Cache;
using Winner.Persistence.Compiler.Common;
using Winner.Persistence.ContextStorage;
using Winner.Persistence.Relation;
using Winner.Persistence.Translation;
using Winner.Persistence.Works;

namespace Winner.Persistence
{
    /// <summary>
    /// 执行上下文
    /// </summary>
    [Serializable]
    public class Context : IContext
    {
        /// <summary>
        /// 执行实例
        /// </summary>
        public IExecutor Executor { get; set; }

        /// <summary>
        /// 事务实例
        /// </summary>
        public ITransaction Transaction { get; set; }

        /// <summary>
        /// Orm实例
        /// </summary>
        public IOrm Orm { get; set; }

        /// <summary>
        /// Orm实例
        /// </summary>
        public ICache Cacher { get; set; }

        /// <summary>
        /// ContextStorage实例
        /// </summary>
        public IContextStorage ContextStorage { get; set; }
        /// <summary>
        /// 存储对象
        /// </summary>
        public ContextInfo Local
        {
            get
            {
                var rev = ContextStorage.Get();
                if (rev == null)
                {
                    rev = new ContextInfo();
                    ContextStorage.Set(rev);
                }
                return rev;

            }
            set { ContextStorage.Set(value); }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {
            var objs = Orm.GetOrms().Values;
            objs = objs.Distinct().ToList();
            foreach (var obj in objs)
            {
                if (!obj.IsCache)
                    continue;
                var handle = new LoadCacheHandle(LoadCache);
                handle.BeginInvoke(obj, null, null);
            }
        }

        public delegate void LoadCacheHandle(OrmObjectInfo obj);

        /// <summary>
        /// 加载缓存
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void LoadCache(OrmObjectInfo obj)
        {
            for (int i = 0;; i++)
            {
                var query = new QueryInfo {IsGreedyLoad = true, IsLazyLoad = true};
                query.From(obj.ObjectName).SetPageSize(1000).SetPageIndex(i);
                var entities = ExecuteInfos<IList<EntityInfo>>(obj, query,true);
                if (entities == null || entities.Count == 0)
                    break;
                foreach (var entity in entities)
                {
                    var value = entity.GetType().GetProperty(obj.PrimaryProperty.PropertyName).GetValue(entity, null);
                    var cacheKey = GetEntityCacheKey(obj, value);
                    var dataEntity = GetCache<EntityInfo>(cacheKey);
                    if (dataEntity != null)
                    {
                        InsertCommonCache(obj, value, entity);
                    }
                }
            }
        }

        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        protected virtual string GetEntityCacheKey(OrmObjectInfo obj, object key)
        {
            return string.Format("{0}{1}", obj.ObjectName, key);
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public virtual T Get<T>(object key, OrmObjectInfo obj = null)
        {
            return Get<T>(key, typeof (T), obj);
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public T Get<T>(object key, Type type, OrmObjectInfo obj = null)
        {
            if (key == null) return default(T);
            if (key.GetType().FullName.Equals(type.FullName))
            {
                if (Local.HasStorage(key)) return (T) Local.Storages[key].Entity;
                return default(T);
            }
            obj = obj ?? Orm.GetOrm(type.FullName);
            string cacheKey = GetEntityCacheKey(obj, key);
            if (!Local.HasEntity(cacheKey))
            {
                object entity = null;
                if (obj.IsCache)
                {
                    entity = GetCache<T>(cacheKey);
                }
                if (entity == null)
                {
                    var query = new QueryInfo {IsLazyLoad = true}.From(type.FullName);
                    query.Where(string.Format("{0}==@{0}", obj.PrimaryProperty.PropertyName))
                         .SetParameter(obj.PrimaryProperty.PropertyName, key);
                    var entities = ExecuteInfos<IList<T>>(obj, query, obj.IsCache);
                    entity = entities.FirstOrDefault();
                    if (obj.IsCache && entity != null)
                    {
                        InsertCommonCache(obj, key, entity);
                    }
                }
                Local.Entities.Add(cacheKey, entity);
            }
            return (T) Local.GetEntity(cacheKey);
        }

        /// <summary>
        /// 得到实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<T> Gets<T>(QueryInfo query, OrmObjectInfo obj = null)
        {
            obj = obj ?? Orm.GetOrm(query.FromExp);
            var infos = ExecuteInfos<IList<T>>(obj, query,obj.IsCache);
            if (infos != null)
            {
                for (var i = 0; i < infos.Count; i++)
                {
                    infos[i] = Attach(infos[i]);
                }
            }
            return infos;
        }


        /// <summary>
        /// 实在实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="sequence"></param>
        /// <param name="isBulkCopy"></param>
        /// <param name="obj"></param>
        /// <param name="inforamtion"></param>
        public virtual void Set<T>(T entity, EntityInfo inforamtion, int sequence = 0, bool isBulkCopy = false, OrmObjectInfo obj = null)
        {
            obj = obj ?? Orm.GetOrm(entity.GetType().FullName);
            if (inforamtion.SaveType == SaveType.Modify && string.IsNullOrEmpty(inforamtion.WhereExp) &&
                obj.VersionProperty != null)
            {
                var dics=new Dictionary<string,string>();
                if (inforamtion.Properties != null)
                {
                    foreach (var property in inforamtion.Properties)
                    {
                        dics.Add(property,property);
                    }
                }
                var hasLocker =
                    obj.Properties.Any(
                        it =>
                        it.IsOptimisticLocker &&
                        (inforamtion.Properties == null || dics.ContainsKey(it.PropertyName)));
                if (hasLocker)
                {
                    var key = entity.GetProperty(obj.PrimaryProperty.PropertyName);
                    var value = Get<T>(key, obj);
                    if (value != null)
                    {
                        var version = value.GetProperty(obj.VersionProperty.PropertyName);
                        entity.SetProperty(obj.VersionProperty.PropertyName, version);
                    }
                }
            }
            if (!Local.HasStorage(entity))
            {
                Local.Storages.Add(entity, new SaveInfo { Entity = entity,Sequence=sequence,IsBulkCopy= isBulkCopy, Information = inforamtion, Object = obj });
            }
        }



        /// <summary>
        /// 附加到实体中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual T Attach<T>(T entity)
        {
            object key = null;
            object value = null;
            var obj = Orm.GetOrm(entity.GetType().FullName);
            var property =
                entity.GetType().GetProperties().FirstOrDefault(it => it.Name.Equals(obj.PrimaryProperty.PropertyName));
            if (property != null)
            {
                value = property.GetValue(entity, null);
                if (property.PropertyType.IsValueType && !value.Equals(0)
                    || !property.PropertyType.IsValueType && value != null)
                {
                    key = GetEntityCacheKey(obj, value);
                }
            }
            if (key == null) return entity;
            if (!Local.HasEntity(key))
            {
                Local.Entities.Add(key, entity);
            }
            if (obj.IsCache)
            {
                InsertCommonCache(obj, value, entity);
            }
            return (T) Local.GetEntity(key);
        }


        /// <summary>
        /// 存储对象
        /// </summary>
        /// <returns></returns>
        public virtual IList<IUnitofwork> Save()
        {
            var count = Local.Unitofworks.Count;
            Executor.Save(Local.GetSaves(), Local.Unitofworks);
            var units = new List<IUnitofwork>();
            for (int i = count; i < Local.Unitofworks.Count; i++)
            {
                units.Add(Local.Unitofworks[i]);
            }
            return units;
        }


        #region 公共缓存
        /// <summary>
        /// 设置公共缓存
        /// </summary>
        protected virtual void InsertCommonCache(OrmObjectInfo obj,object key,object entity)
        {
            SetCache(GetEntityCacheKey(obj, key), entity, obj.CacheTime);
            var ormProperties =
             obj.Properties.Where(it => it.Map != null && (it.Map.IsGreedyLoad || it.Map.IsLazyLoad));
            foreach (var ormProperty in ormProperties)
            {
                var mapValue = entity.GetProperty(ormProperty.PropertyName);
                if (ormProperty.Map.MapType == OrmMapType.OneToMany)
                {
                    if(mapValue==null)continue;
                    var values = mapValue as IEnumerable<EntityInfo>;
                    if(values==null) continue;
                    foreach (var value in values)
                    {
                        InsertCommonCache(ormProperty.Map.GetMapObject(), value.GetProperty(ormProperty.Map.GetMapObject().PrimaryProperty.PropertyName), value);    
                    }
                }
                else
                {
                    InsertCommonCache(ormProperty.Map.GetMapObject(), mapValue.GetProperty(ormProperty.Map.GetMapObject().PrimaryProperty.PropertyName), mapValue); 
                }
            }
        }
        /// <summary>
        /// 移除公共缓存
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        protected virtual void RemoveCommonCache(OrmObjectInfo obj, object key)
        {
            var cacheKey = GetEntityCacheKey(obj, key);
            var cacheValue =Cacher.Get(cacheKey,Type.GetType(obj.ObjectName));
            if (cacheValue == null) return;
            if (obj.IsCache)
            {
                Cacher.Remove(cacheKey);
            }
            var ormMaps =
            obj.Properties.Where(it => it.Map != null && it.Map.IsRemoveCache)
               .Select(it => it.Map).ToList();
            if (ormMaps.Count == 0) return;
            foreach (var ormMap in ormMaps)
            {
                var value = cacheValue.GetProperty(ormMap.ObjectProperty.PropertyName);
                if (!obj.IsCache)
                {
                    Cacher.Remove(cacheKey);
                }
                RemoveCommonCache(ormMap.GetMapObject(), value);
            }
        }
        #endregion

        #region 提交

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="unitofworks"></param>
        /// <returns></returns>
        public bool Commit(IList<IUnitofwork> unitofworks)
        {
            var rev = Transaction.Commit(unitofworks);
            if (rev)
            {
                foreach (var entity in Local.Storages)
                {
                    if (entity.Value.Object.IsCache || entity.Value.Object.Properties.Count(it=>it.Map!=null && it.Map.IsRemoveCache)>0)
                    {
                        if (entity.Value.Information.SaveType == SaveType.None)
                            continue;
                        var id =entity.Key.GetProperty(entity.Value.Object.PrimaryProperty.PropertyName);
                        if (id == null || id.GetType().IsValueType && id.Equals(0))
                            continue;
                        try
                        {
                            RemoveCommonCache(entity.Value.Object, id);
                            var ormMaps =
                                entity.Value.Object.Properties.Where(it => it.Map != null && it.Map.IsRemoveCache)
                                      .Select(it => it.Map).ToList();
                            if (ormMaps.Count > 0)
                            {
                                foreach (var ormMap in ormMaps)
                                {
                                    var key = entity.Key.GetProperty(ormMap.ObjectProperty.PropertyName);
                                    if (key == null || key.GetType().IsValueType && key.Equals(0))
                                        continue;
                                    RemoveCommonCache(ormMap.GetMapObject(), key);
                                }
                            }
                        }
                        catch 
                        {

                        }
                    }
                }
            }
            Local = null;
            return rev;
        }

        #endregion

        #region 查询

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual T GetInfos<T>(QueryInfo query, OrmObjectInfo obj = null)
        {
            obj = obj ?? Orm.GetOrm(query.FromExp);
            var infos = ExecuteInfos<T>(obj, query);
            return infos;
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T ExecuteQuery<T>(string name, string commandText, CommandType commandType, params object[] parameters)
        {
            return Executor.ExecuteQuery<T>(name, commandText, commandType, parameters);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="name"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteCommand(string name, string commandText, CommandType commandType,
                                  params object[] parameters)
        {
            return Executor.ExecuteCommand(name, commandText, commandType, parameters);
        }

        #endregion

        #region 自定义缓存或DB加载

        /// <summary>
        /// 缓存值
        /// </summary>
        private static readonly object KeyLoker = new object();


        private string _cacheTag = "Winner_PersistenceQuery_{0}_";

        /// <summary>
        /// 缓存前缀
        /// </summary>
        public virtual string CacheTag
        {
            get { return _cacheTag; }
            set { _cacheTag = value; }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="query"></param>
        /// <param name="isLazyLoadExecute"></param>
        /// <returns></returns>
        protected virtual T ExecuteInfos<T>(OrmObjectInfo obj, QueryInfo query, bool isLazyLoadExecute = false)
        {
            if (query.Cache != null)
                return GetInfosByCache<T>(query, obj);
            var result = Executor.GetInfos<T>(obj, query, this, isLazyLoadExecute);

            return result;
        }

        /// <summary>
        /// 从缓存中查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual T GetInfosByCache<T>(QueryInfo query, OrmObjectInfo obj)
        {
            SetQueryCustomCacheKey(query, obj);
            QueryCacheInfo<T> cacheResult = GetQueryCacheInfo<T>(query);
            if (null == cacheResult || cacheResult.Result == null)
            {
                lock (KeyLoker)
                {
                    cacheResult = GetQueryCacheInfo<T>(query);
                    if (null == cacheResult || cacheResult.Result == null)
                    {
                        cacheResult = new QueryCacheInfo<T>
                        {
                            Result = Executor.GetInfos<T>(obj, query, this, true),
                            DataCount = query.DataCount
                        };
                        //if (query.Cache.TimeSpan != 0)
                        //{
                        //    SetLocalCache(query.Cache.Key, cacheResult, query.Cache.TimeSpan);
                        //}
                        //else if (query.Cache.Time != DateTime.MinValue)
                        //{
                        //    SetLocalCache(query.Cache.Key, cacheResult, query.Cache.Time);
                        //}
                        if (query.Cache.TimeSpan != 0)
                        {
                            SetCache(query.Cache.Key, cacheResult, query.Cache.TimeSpan);
                        }
                        else if (query.Cache.Time != DateTime.MinValue)
                        {
                            SetCache(query.Cache.Key, cacheResult, query.Cache.Time);
                        }
                    }
                }
            }
            query.DataCount = cacheResult.DataCount;
            return cacheResult.Result;
        }
        /// <summary>
        /// 得到缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual QueryCacheInfo<T> GetQueryCacheInfo<T>(QueryInfo query)
        {
            QueryCacheInfo<T> cacheResult = null;
            if (query.Cache.Time != DateTime.MinValue || query.Cache.TimeSpan != 0)
            {
                cacheResult = GetCache<QueryCacheInfo<T>>(query.Cache.Key);
            }
            //if ((cacheResult == null || cacheResult.Result==null) && (query.Cache.Time2 != DateTime.MinValue || query.Cache.TimeSpan2 != 0))
            //{
            //    cacheResult = GetCache<QueryCacheInfo<T>>(query.Cache.Key);
            //}
            return cacheResult;
        }

        /// <summary>
        /// 得到缓存的Key
        /// </summary>
        /// <param name="query"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual void SetQueryCustomCacheKey(QueryInfo query, OrmObjectInfo obj)
        {
            var sb = new StringBuilder();
            sb.Append(query.SelectExp);
            sb.Append(query.FromExp);
            sb.Append(query.WhereExp);
            sb.Append(query.GroupByExp);
            sb.Append(query.HavingExp);
            sb.Append(query.OrderByExp);
            sb.Append(query.PageIndex);
            sb.Append(query.PageSize);
            if (query.Parameters != null)
            {
                foreach (var p in query.Parameters)
                {
                    sb.Append(p.Key);
                    var keys = p.Value as Array;
                    if (keys != null)
                    {
                        foreach (var key in keys)
                        {
                            sb.Append(key);
                        }
                    }
                    else
                    {
                        sb.Append(p.Value);
                    }
                }
            }
            var value = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sb.ToString(), "MD5");
            query.Cache.Key = string.Format("{0}{1}", string.Format(CacheTag, query.Cache.Name), value);
        }

        /// <summary>
        /// 刷新缓存
        /// </summary>
        /// <returns></returns>
        public bool Flush(string key)
        {
            lock (KeyLoker)
            {
                //RemoveLocalCache(key);
                Cacher.Remove(key);
                return true;
            }
        }

     
        #endregion

        #region 缓存
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        protected virtual bool SetCache(string key, object value, DateTime time)
        {
            return Cacher.Set(key, value, time);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        protected virtual bool SetCache(string key, object value, long timeSpan)
        {
            return Cacher.Set(key, value, timeSpan);
        }
        /// <summary>
        /// 得到缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual T GetCache<T>(string key)
        {
            var value= Cacher.Get<T>(key);
            return value;
        }

       
        #endregion

    }
}
