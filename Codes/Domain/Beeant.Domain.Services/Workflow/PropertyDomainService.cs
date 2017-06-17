using Beeant.Domain.Entities.Workflow;
using Winner.Persistence;

namespace Beeant.Domain.Services.Workflow
{
    public class PropertyDomainService : RealizeDomainService<PropertyEntity>
    {
     
        #region 重新验证
      

        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PropertyEntity info)
        {
            return ValidateNode(info); 
        }

        /// <summary>
        /// 验证类型
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateNode(PropertyEntity info)
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
