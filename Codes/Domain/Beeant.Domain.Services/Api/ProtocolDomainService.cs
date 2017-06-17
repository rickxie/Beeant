using System.Linq;
using Beeant.Domain.Entities.Api;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Api
{
    public class ProtocolDomainService : RealizeDomainService<ProtocolEntity>
    {


        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(ProtocolEntity info)
        {
            var rev = ValidateNameExist(info,null);
            return rev;
        }
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(ProtocolEntity info)
        {
            var dataEnitty = Repository.Get<ProtocolEntity>(info.Id);
            var rev = ValidateNameExist(info, dataEnitty);
            return rev;
        }

        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateNameExist(ProtocolEntity info, ProtocolEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Name))
                return true;
            if (dataEntity != null && dataEntity.Id == info.Id && dataEntity.Name == info.Name)
                return true;
            var query=new QueryInfo();
            query.Query<ProtocolEntity>().Where(it => it.Name == info.Name).Select(it => it.Id);
            var infos = Repository.GetEntities<ProtocolEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("NameExist");
                return false;
            }
            return true;
          
        }

    }
}
