namespace Beeant.Application.Services.Member
{
    public interface ICouponApplicationService
    {
        /// <summary>
        /// 生成优惠券
        /// </summary>
        /// <param name="couponerId"></param>
        /// <returns></returns>
        bool CreateCoupon(long couponerId);
    }
}
