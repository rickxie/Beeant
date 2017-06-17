using Beeant.Application.Dtos.Order;

namespace Beeant.Application.Services.Order
{
    public interface IOrderApplicationService
    {

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        SettlementDto Create(SettlementDto dto);

    }
}
