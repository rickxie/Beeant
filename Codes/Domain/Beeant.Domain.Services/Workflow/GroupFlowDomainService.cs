using System.Linq;
using Beeant.Domain.Entities.Workflow;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Workflow
{
    public class GroupFlowDomainService : RealizeDomainService<GroupFlowEntity>
    {
        

        #region 重写验证
 
        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(GroupFlowEntity info)
        {
            return ValidateGroupFlowExist(info) && ValidateFlowExist(info) && ValidateGroupExist(info);
        }

 
        /// <summary>
        /// 验证GroupAbility是否已经存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateGroupFlowExist(GroupFlowEntity info)
        {
            if (info.Flow != null && info.Flow.SaveType == SaveType.Add ||
                info.Group != null && info.Group.SaveType == SaveType.Add)
                return true;
            var query = new QueryInfo();
            query.Query<GroupFlowEntity>().Where(it => it.Flow.Id == info.Flow.Id
                                                      && it.Group.Id == info.Group.Id && it.Flow.Id==info.Flow.Id);
            var infos = Repository.GetEntities<GroupFlowEntity>(query);
            if (infos == null || infos.Count == 0) return true;
            info.AddError("ExistGroupFlow");
            return false;
        }

        /// <summary>
        /// 验证角色类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateFlowExist(GroupFlowEntity info)
        {
            if (!info.HasSaveProperty(it => it.Flow.Id))
                return true;
            if (info.Flow != null && (info.Flow.SaveType == SaveType.Add || info.Flow.Id == 0))
                return true;
            if (info.Flow != null && info.Flow.Id!=0)
            {
                if (Repository.Get<FlowEntity>(info.Flow.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(FlowEntity).FullName, "NoExist");
            return false;
        }

        /// <summary>
        /// 验证功能类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateGroupExist(GroupFlowEntity info)
        {
            if (!info.HasSaveProperty(it => it.Group.Id))
                return true;
            if (info.Group != null && info.Group.SaveType == SaveType.Add)
                return true;
            if (info.Group != null && info.Group.Id!=0)
            {
                if (Repository.Get<GroupEntity>(info.Group.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(GroupEntity).FullName, "NoExist");
            return false;
        }
   
        #endregion
    }
}
