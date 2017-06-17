using Beeant.Domain.Entities.Workflow;
using Winner.Persistence;

namespace Beeant.Domain.Services.Workflow
{
    public class ConditionDomainService : RealizeDomainService<ConditionEntity>
    {
        

        #region 重写验证

        /// <summary>
        /// 重写验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(ConditionEntity info)
        {
            var rev = ValidateNodeExist(info);
            return rev;
        }
  
       
        /// <summary>
        /// 验证状态是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateNodeExist(ConditionEntity info)
        {
            if (!info.HasSaveProperty(it => it.Node.Id))
                return true;
            if (info.Node != null && info.Node.SaveType == SaveType.Add)
                return true;
            if (info.Node != null && info.Node.Id!=0)
            {
                if (Repository.Get<NodeEntity>(info.Node.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(NodeEntity).FullName, "NoExist");
            return false;
        }
        #endregion
    }
}
