using Beeant.Domain.Entities.Wms;

namespace Beeant.Application.Services.Purchase
{
    public interface IPurchaseApplicationService
    {
        StockEntity CreateStock(long purchaseId);        
    }
}
