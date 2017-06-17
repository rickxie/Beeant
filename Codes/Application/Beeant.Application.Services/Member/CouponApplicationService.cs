using System.Collections.Generic;
using Beeant.Domain.Entities.Member;
using Beeant.Domain.Services.Member;

namespace Beeant.Application.Services.Member
{
    public class CouponApplicationService : RealizeApplicationService<CouponEntity>, ICouponApplicationService
    {
        /// <summary>
        /// 生成优惠券
        /// </summary>
        public ICouponDomainService CouponDomainService { get; set; }
        static protected readonly object Locker = new object();

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public override bool Save(IList<CouponEntity> infos)
        {
            lock (Locker)
            {
                return base.Save(infos);

            }
        }

        /// <summary>
        /// 生成优惠券
        /// </summary>
        /// <param name="couponerId"></param>
        /// <returns></returns>
        public virtual bool CreateCoupon(long couponerId)
        {
            var infos = CouponDomainService.CreateCoupon(couponerId);
            if (infos ==null || infos.Count == 0) return false;
            return Save(infos);
        }
    }
}
