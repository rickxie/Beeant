using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Product
{
    public class PlatformJobApplicationService : PlatformApplicationService,IJobApplicationService
    {
        public virtual bool Execute(object[] args)
        {
            var infos = GetEntities();
            if (infos == null || infos.Count == 0)
                return true;
            foreach (var info in infos)
            {
                Synchronize(info.Id);
            }
            return true;
        }
      
        /// <summary>
        /// 得到商品
        /// </summary>
        /// <returns></returns>
        protected virtual IList<GoodsEntity> GetEntities()
        {
            var query = new QueryInfo();
            query.Query<GoodsEntity>().Select(it=>it.Id);
            return Repository.GetEntities<GoodsEntity>(query);
        }

    }
}
