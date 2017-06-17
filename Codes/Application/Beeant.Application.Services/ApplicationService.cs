using System;
using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Services;
using Winner.Persistence;

namespace Beeant.Application.Services
{
    public class ApplicationService: MarshalByRefObject, IApplicationService 
    {
        #region 属性
        /// <summary>
        /// 存储实例
        /// </summary>
        public virtual IRepository Repository { get; set; }
        /// <summary>
        /// 服务实例
        /// </summary>
        public virtual IDomainService DomainService { get; set; }
        #endregion


        #region 接口的实现
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IList<T> GetEntities<T>(QueryInfo query)
        {
            return Repository.GetEntities<T>(query);
        }
        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual T Execute<T>(QueryInfo query)
        {
            return Repository.Execute<T>(query);
        }
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool Save<T>(T info) where T : BaseEntity
        {
            IList<T> infos=new List<T>();
            infos.Add(info);
            return Save(infos);
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public virtual bool Save<T>(IList<T> infos) where T : BaseEntity
        {
            if (infos == null)
                return false;
            try
            {
                var rev = Handle(infos);
                if (rev != null)
                    return Commit(rev);
                return false;
            }
            catch (Exception ex)
            {
                foreach (var info in infos)
                {
                    var rev = HandleException(ex, info);
                    if (!rev)
                        throw;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据ID得到对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetEntity<T>(long id) where T : BaseEntity
        {
            return Repository.Get<T>(id);
         
        }
        #endregion

        #region 方法

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool HandleException<T>(Exception ex,T info) where T : BaseEntity
        {
            switch (ex.Message)
            {
                case "Version Expired":
                    info.AddErrorByName(typeof(BaseEntity).FullName, "VersionExpired");
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        /// 得到验证成功的事务处理对象
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected virtual IList<IUnitofwork> Handle<T>(IList<T> infos) where T : BaseEntity
        {
            return DomainService.Handle(infos);
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        protected virtual bool Commit(IList<IUnitofwork> unitofworks )
        {
            return Winner.Creator.Get<IContext>().Commit(unitofworks);
        }



        #endregion


    }
}
