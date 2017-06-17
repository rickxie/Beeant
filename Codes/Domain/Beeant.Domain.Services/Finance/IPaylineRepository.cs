using Beeant.Domain.Entities.Finance;

namespace Beeant.Domain.Services.Finance
{

    public interface IPaylineRepository
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
        /// <summary>
        /// 检查状态
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        bool Check(PaylineEntity info);

    }
}
