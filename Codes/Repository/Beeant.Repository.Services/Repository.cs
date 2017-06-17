using System;
using System.Collections.Generic;
using Beeant.Domain.Entities;
using Winner;
using Beeant.Domain.Services;
using Winner.Persistence;


namespace Beeant.Repository.Services
{

    public class Repository : MarshalByRefObject, IRepository 
    {

        /// <summary>
        /// 存储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual IList<IUnitofwork> Save<T>(T info) where T: BaseEntity
        {
            Creator.Get<IContext>().Set(info,info,info.SaveSequence);
            info.SaveType = info.SaveType;
            return Creator.Get<IContext>().Save();
        }
        /// <summary>
        /// 存储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="infos"></param>
        /// <returns></returns>
        public virtual IList<IUnitofwork> Save<T>(IList<T> infos) where T : BaseEntity
        {
            foreach (var info in infos)
            {
                info.SaveType = info.SaveType;
                Creator.Get<IContext>().Set(info, info, info.SaveSequence,info.IsBulkCopy);
            }
            return Creator.Get<IContext>().Save();
        }

        /// <summary>
        /// 附加实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual T Attach<T>(T info) where T : BaseEntity
        {
            return Creator.Get<IContext>().Attach(info);
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Get<T>(long id) where T : BaseEntity
        {
            return Creator.Get<IContext>().Get<T>(id);
        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public T Get<T>(long id, Type type)
        {
            return Creator.Get<IContext>().Get<T>(id, type);
        }


        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IList<T> Gets<T>(QueryInfo query) where T : BaseEntity
        {
            return Creator.Get<IContext>().Gets<T>(query);
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IList<T> GetEntities<T>(QueryInfo query)
        {
            return Creator.Get<IContext>().GetInfos<IList<T>>(query);
        }
        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual T Execute<T>(QueryInfo query)
        {
            return Creator.Get<IContext>().GetInfos<T>(query);
        }
    }
 
}
