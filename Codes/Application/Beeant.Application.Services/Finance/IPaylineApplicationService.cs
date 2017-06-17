using Beeant.Domain.Entities.Finance;

namespace Beeant.Application.Services.Finance
{
    public interface IPaylineApplicationService
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        bool Create(PaylineEntity info);
        /// <summary>
        /// 处理
        /// </summary>
        /// <returns></returns>
        PaylineEntity Process();
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        bool Refund(PaylineEntity info);

    }
}
