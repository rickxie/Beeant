using System.Linq;
using Beeant.Domain.Entities.Wms;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Wms
{
    public class StorehouseDomainService : RealizeDomainService<StorehouseEntity>
    {

   
   

        /// <summary>
        /// 删除验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(StorehouseEntity info)
        {
            return ValidateStock(info);
        }



        /// <summary>
        /// 验证商品
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateStock(StorehouseEntity info)
        {
            var query = new QueryInfo();
            query.SetPageIndex(0).SetPageSize(1).Query<StockItemEntity>().Where(it => it.Storehouse.Id == info.Id);
            var infos = Repository.GetEntities<StockItemEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("HasStocksNotAllowRemove");
                return false;  
            }
            return true;
        }
 


    }
}
