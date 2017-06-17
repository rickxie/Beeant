using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Sys;
using Beeant.Domain.Services;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Sys
{
    public class QueueJobApplicationService :RealizeApplicationService<QueueEntity>, IJobApplicationService
    {
 
        private static bool IsExecute { get; set; }
        /// <summary>
        /// 设置名称
        /// </summary>
        protected virtual string Name { get; set; }
        /// <summary>
        /// 设置页的大小
        /// </summary>
        public virtual int PageSize { get; set; } = 50;
        public virtual bool Execute(object[] args)
        {
            if (IsExecute)
                return false;
            try
            {
                IsExecute = true;
                var entities = GetQueues();
                foreach (var entity in entities)
                {
                    try
                    {
                        if (Handle(entity))
                        {
                            entity.SaveType = SaveType.Remove;
                        }
                    }
                    catch (Exception ex)
                    {
                        Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
                    }
                    finally
                    {
                        entity.SaveType = SaveType.Remove;
                    }
                   
                }
                Remove(entities);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            finally
            {
                IsExecute = false;
            }
            return true;
        }
        /// <summary>
        /// 得到队列
        /// </summary>
        /// <returns></returns>
        protected virtual IList<QueueEntity> GetQueues()
        {
            var query = new QueryInfo {IsReturnCount = false};
            query.SetPageSize(PageSize)
                .Query<QueueEntity>()
                .Where(it => it.Name == Name)
                .Select(it => new object[] {it.Id, it.Name, it.Value });
            return Repository.GetEntities<QueueEntity>(query);
        }

        /// <summary>
        /// 设置实体
        /// </summary>
        /// <param name="entity"></param>
        protected virtual bool Handle(QueueEntity entity)
        {
            return true;
        }
        /// <summary>
        /// 设置实体
        /// </summary>
        /// <param name="entities"></param>
        protected virtual void Remove(IList<QueueEntity> entities)
        {
          
            var unitofworks = Repository.Save(entities);
            Winner.Creator.Get<IContext>().Commit(unitofworks);
        }
        /// <summary>
        /// 添加队列
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Add(string value)
        {
 
            var entity = new QueueEntity
            {
                Name = Name,
                Value = value,
                SaveType = SaveType.Add
            };
            return Save(entity);
        }
    }
}
