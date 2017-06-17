using System.Collections.Generic;
using Beeant.Application.Dtos.Order;
using Beeant.Application.Services.Order;
using Beeant.Domain.Entities.Merchant;
using Beeant.Domain.Entities.Member;

namespace Beeant.Presentation.Website.Buy.Models.Home
{
    public class SettlementModel
    {
        /// <summary>
        /// 结算DTO
        /// </summary>
        public SettlementDto SettlementDto { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public IList<AddressEntity> Addresses { get; set; }
        /// <summary>
        /// 优惠券
        /// </summary>
        public IList<CouponEntity> Coupons { get; set; }
        /// <summary>
        /// 发票
        /// </summary>
        public IList<InvoiceEntity> Invoices { get; set; }
        /// <summary>
        /// 加盟商
        /// </summary>
        public IList<PartnerEntity> Partners { get; set; } 
    }
}