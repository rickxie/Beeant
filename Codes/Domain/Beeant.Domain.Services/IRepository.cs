using System;
using System.Collections.Generic;
using Beeant.Domain.Entities;
using Winner.Persistence;

namespace Beeant.Domain.Services
{
    public interface IRepository
    {
        /// <summary>
        /// 存储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        IList<IUnitofwork> Save<T>(T info) where T : BaseEntity;
        /// <summary>
        /// 存储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="infos"></param>
        /// <returns></returns>
        IList<IUnitofwork> Save<T>(IList<T> infos) where T : BaseEntity; 

        /// <summary>
        /// 设置实体，如果存在改变当前实体，如果不存在添加到实体中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        T Attach<T>(T info) where T : BaseEntity;

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get<T>(long id) where T : BaseEntity;

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="type"></param>
        T Get<T>(long id, Type type);
        /// <summary>
        /// 得到实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<T> Gets<T>(QueryInfo query) where T : BaseEntity;

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<T> GetEntities<T>(QueryInfo query);
        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        T Execute<T>(QueryInfo query);
    }
}
