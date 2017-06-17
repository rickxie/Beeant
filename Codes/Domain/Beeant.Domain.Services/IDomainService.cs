using System.Collections.Generic;
using Beeant.Domain.Entities;
using Winner.Persistence;

namespace Beeant.Domain.Services
{
    public interface IDomainService
    {
        /// <summary>
        /// 验证信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        IList<IUnitofwork> Handle<T>(T info) where T : BaseEntity;
        /// <summary>
        /// 验证信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        IList<IUnitofwork> Handle<T>(IList<T> infos) where T : BaseEntity;

        /// <summary>
        /// 得到加载内容
        /// </summary>
        void SetItemLoaders<T>(T info) where T : BaseEntity;
    }
}
