using System.Collections.Generic;
using Beeant.Domain.Entities;

namespace Beeant.Distributed.Outside.Pay.Models
{
    public class PaylineModel
    {
        /// <summary>
        /// 渠道
        /// </summary>
        public ChannelType ChannelType { get; set; }
        /// <summary>
        /// 充值类型
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderIds { get; set; }
        /// <summary>
        /// 支付
        /// </summary>
        public string OrderPayId { get; set; }
        /// <summary>
        /// 输出
        /// </summary>
        public IDictionary<string, string> Forms { get; set; }

    }
}