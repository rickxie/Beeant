using System.Linq;
using Beeant.Domain.Entities.Basedata;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Basedata
{
    public class DistrictDomainService : RealizeDomainService<DistrictEntity>
    {

        /// <summary>
        /// 验证修改
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateModify(DistrictEntity info)
        {
            var dataEntity = Repository.Get<DistrictEntity>(info.Id);
            return ValidateBranch(info, dataEntity);
        }

    
   
        /// <summary>
        /// 验证支点
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        protected virtual bool ValidateBranch(DistrictEntity info, DistrictEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Parent.Id)) return true;
            if (dataEntity.Parent != null && info.Parent.Id == dataEntity.Parent.Id) return true;
            var data = Repository.Get<DistrictEntity>(info.Parent.Id);
            do
            {
                if(data==null)break;
                if (data.Id == info.Id || data.Parent != null && data.Parent.Id == info.Id)
                {
                    info.AddError("NotAllowParent");
                    return false;
                }
                if (data.Parent == null || data.Parent.Id==0)
                    break;
                data = Repository.Get<DistrictEntity>(data.Parent.Id);
            } while (data.Parent != null && dataEntity.Parent.Id!=0);
            return true;
        }

        /// <summary>
        /// 删除验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(DistrictEntity info)
        {
            return ValidateMenuLeaf(info); 
        }

    
        /// <summary>
        /// 验证Menu是否是页节点
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateMenuLeaf(DistrictEntity info)
        {
            var query = new QueryInfo();
            query.Query<DistrictEntity>().Where(it => it.Parent.Id == info.Id)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<DistrictEntity>(query);
            if (infos != null && infos.Count == 0) return true;
            info.AddError("ExistChild");
            return false;
        }

    }
}
