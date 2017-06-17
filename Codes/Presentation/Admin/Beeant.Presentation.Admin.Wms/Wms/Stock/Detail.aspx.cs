using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Application.Services;
using Dependent;
using Beeant.Domain.Entities.Purchase;
using Beeant.Domain.Entities.Order;

namespace Beeant.Presentation.Admin.Wms.Wms.Stock
{
    public partial class Detail : WorkflowPageBase<StockEntity>
    {
        public int Index = 0;
        public long PurchaseId { get; set; }
        public long OrdetId { get; set; }
       
        protected override StockEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Purchase != null&&info.Order!=null)
            {
                PurchaseId = info.Purchase.Id;
                OrdetId = info.Order.Id;
                info.Purchase = Ioc.Resolve<IApplicationService, PurchaseEntity>().GetEntity<PurchaseEntity>(info.Purchase.Id);
                info.Order = Ioc.Resolve<IApplicationService, OrderEntity>().GetEntity<OrderEntity>(info.Order.Id);
            }
            return info;

        }
  
    
    }
}