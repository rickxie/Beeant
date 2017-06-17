using System.Linq;
using Beeant.Domain.Entities.Purchase;
using Beeant.Domain.Entities.Wms;
using Beeant.Domain.Services;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Purchase
{
    public class PurchaseApplicationSerivce : IPurchaseApplicationService
    {
        #region 属性
        /// <summary>
        /// 存储实例
        /// </summary>
        public virtual IRepository Repository { get; set; }
        #endregion

        /// <summary>
        /// 组合入库单需要的各类数据明细
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <returns></returns>
        public StockEntity CreateStock(long purchaseId)
        {
            var query = new QueryInfo();
            query.Query<PurchaseEntity>().Where(it => it.Id == purchaseId)
                 .Select(
                     it =>
                     new object[]
                         {
                             it,it.Storehouse,
                             it.PurchaseItems.Select(
                                 items =>
                                 new object[]
                                     {
                                         items, items.Product,
                                         items.Product.Inventories.Select(
                                             inv => new object[] {inv, inv.Product, inv.Storehouse})
                                     })
                         });
            var purchase = Repository.GetEntities<PurchaseEntity>(query).FirstOrDefault();
            if (purchase != null)
                return purchase.CreateStocks();
            return null;
        }

    }
}
