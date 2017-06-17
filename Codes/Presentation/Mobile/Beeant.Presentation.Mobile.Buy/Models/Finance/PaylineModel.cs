using Beeant.Domain.Entities.Finance;

namespace Beeant.Presentation.Mobile.Buy.Models.Finance
{
    public class PaylineModel
    {
        /// <summary>
        /// 类型
        /// </summary>
        public PaylineType Type { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderIds { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}