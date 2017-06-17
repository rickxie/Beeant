using System;
using System.Collections.Generic;
using System.Data;
using Winner.Persistence.Relation;

namespace Winner.Persistence
{
    public interface IContext
    {
  

        /// <summary>
        /// 保存对象
        /// </summary>
        ContextInfo Local { get; }
        /// <summary>
        /// 初始化
        /// </summary>
        void Initialize();
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        T Get<T>(object key, OrmObjectInfo obj = null);

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        T Get<T>(object key,Type type,  OrmObjectInfo obj = null);
        /// <summary>
        /// 得到实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="obj"></param>
        IList<T> Gets<T>(QueryInfo query, OrmObjectInfo obj = null);

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inforamtion"></param>
        /// <param name="sequence"></param>
        /// <param name="isBulkCopy"></param>
        /// <param name="obj"></param>
        /// <param name="entity"></param>
        void Set<T>(T entity,EntityInfo inforamtion,int sequence=0, bool isBulkCopy = false, OrmObjectInfo obj=null);

        /// <summary>
        /// 设置实体，如果存在改变当前实体，如果不存在添加到实体中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Attach<T>(T entity);
        /// <summary>
        /// 存储对象
        /// </summary>
        /// <returns></returns>
        IList<IUnitofwork> Save();
        /// <summary>
        /// 存储对象
        /// </summary>
        /// <returns></returns>
        bool Commit(IList<IUnitofwork> unitofworks);
        /// <summary>
        /// 得到实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        T GetInfos<T>(QueryInfo query, OrmObjectInfo obj=null);
        /// <summary>
        /// 刷新缓存
        /// </summary>
        /// <param name="key"></param>
        bool Flush(string key);
        /// <summary>
        /// 执行查询存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        T ExecuteQuery<T>(string name, string commandText, CommandType commandType, params object[] parameters);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="name"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteCommand(string name, string commandText, CommandType commandType, params object[] parameters);
 
    }
}
