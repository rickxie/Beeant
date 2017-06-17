using System.Collections.Generic;
using Beeant.Domain.Entities;
using Winner.Persistence;

namespace Beeant.Application.Services
{
    public interface IApplicationService
    {
        /// <summary>
        /// 根据ID得到对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetEntity<T>(long id) where T : BaseEntity;
        /// <summary>
        ///存储对象
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        bool Save<T>(IList<T> infos) where T : BaseEntity;
        /// <summary>
        ///存储对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        bool Save<T>(T info) where T : BaseEntity;

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
