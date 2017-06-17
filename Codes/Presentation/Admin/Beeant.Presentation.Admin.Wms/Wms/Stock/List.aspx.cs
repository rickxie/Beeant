using System.Collections.Generic;
using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Purchase;
using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Wms.Wms.Stock
{
    public partial class List :  ListPageBase<StockEntity>
    {

       

        protected override IList<StockEntity> GetEntities()
        {
            var infos = base.GetEntities();
            if (infos != null)
            {
                SetPaO(infos);
            }
            return infos;
        }

        protected virtual void SetPaO(IList<StockEntity> infos)
        {
            var purchaseIds =
                   infos.Where(it => it.Purchase != null && it.Purchase.Id != 0)
                        .Select(it => it.Purchase.Id)
                        .ToArray();
            if (purchaseIds.Length > 0)
            {
                var query = new QueryInfo();
                query.Query<PurchaseEntity>().Where(it => purchaseIds.Contains(it.Id));
                var purchases = Ioc.Resolve<IApplicationService, PurchaseEntity>().GetEntities<PurchaseEntity>(query);
                if (purchases != null)
                {
                    foreach (var info in infos)
                    {
                        if (info.Purchase == null) continue;
                        info.Purchase= purchases.FirstOrDefault(it => it.Id == info.Purchase.Id);
                    }
                }
            }

            var orderIds =
                   infos.Where(it => it.Order != null && it.Order.Id != 0)
                        .Select(it => it.Order.Id)
                        .ToArray();
            if (orderIds.Length > 0)
            {
                var query = new QueryInfo();
                query.Query<OrderEntity>().Where(it => orderIds.Contains(it.Id));
                var orders = Ioc.Resolve<IApplicationService, OrderEntity>().GetEntities<OrderEntity>(query);
                if (orders != null)
                {
                    foreach (var info in infos)
                    {
                        if (info.Order == null) continue;
                        info.Order = orders.FirstOrDefault(it => it.Id == info.Order.Id);
                    }
                }
            }
        }
    }
}