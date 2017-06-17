using System.Linq;
using Beeant.Domain.Entities.Basedata;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Basedata
{
    public class TagGroupDomainService : RealizeDomainService<TagGroupEntity>
    {
     
       

        /// <summary>
        /// 删除验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(TagGroupEntity info)
        {
            return ValidateTag(info); 
        }

    
        /// <summary>
        /// 验证Menu是否是页节点
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateTag(TagGroupEntity info)
        {
            var query = new QueryInfo();
            query.Query<TagEntity>().Where(it => it.TagGroup.Id == info.Id)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<TagEntity>(query);
            if (infos != null && infos.Count == 0) return true;
            info.AddError("ExistTag");
            return false;
        }

    }
}
