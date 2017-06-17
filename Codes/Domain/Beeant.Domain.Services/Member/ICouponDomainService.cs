using System.Collections.Generic;
using Beeant.Domain.Entities.Member;

namespace Beeant.Domain.Services.Member
{
    public interface ICouponDomainService
    {
        /// <summary>
        /// 生成优惠券
        /// </summary>
        /// <param name="couponerId"></param>
        /// <returns></returns>
        IList<CouponEntity> CreateCoupon(long couponerId);
    }
}
