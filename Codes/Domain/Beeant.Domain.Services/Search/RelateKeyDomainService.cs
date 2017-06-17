using System;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Search;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Search
{
    public class RelateKeyDomainService : RealizeDomainService<RelateKeyEntity>
    {
 

        #region 重写验证

     

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(RelateKeyEntity info)
        {
            var rev = ValidateKeyStep(info);
            return rev;
        }

        /// <summary>
        /// 验证词是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateKeyStep(RelateKeyEntity info)
        {
            var stepTime = GetKeyStepTime();
            var query = new QueryInfo();
            query.Query<RelateKeyEntity>().Where(it => it.Name == info.Name && it.KeyName==info.KeyName && it.InsertTime >= stepTime
                && it.Ip==info.Ip);
            var infos = Repository.GetEntities<RelateKeyEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("InRelateKeyStep");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <returns></returns>
        protected virtual DateTime GetKeyStepTime()
        {
            var query = new QueryInfo();
            query.SetCacheTime(DateTime.MaxValue).Query<Entities.Sys.ParameterEntity>().Where(it => it.Name == "RelateKeyStep");
            var infos = Repository.GetEntities<Entities.Sys.ParameterEntity>(query);
            if (infos == null || infos.Count == 0) return DateTime.MaxValue;
            return DateTime.Now.AddMinutes(-infos[0].Value.Convert<int>());
        }

        #endregion


    }
}
