using System;

namespace Beeant.Domain.Entities.Purchase
{
    [Serializable]
    public enum PurchaseStatusType
    {
        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 1,
        /// <summary>
        /// 完成
        /// </summary>
        Finish = 2,
        /// <summary>
        /// 已经提交
        /// </summary>
        WaitHandle = 3,
        /// <summary>
        /// 等待经理审核
        /// </summary>
        WaitManagerAudit = 4,
        /// <summary>
        /// 等待总监审核
        /// </summary>
        WaitDirectorAudit = 5,
        /// <summary>
        /// 等待收货
        /// </summary>
        WaitReceive = 6,
        /// <summary>
        /// 等待验收
        /// </summary>
        WaitSign = 7,
        /// <summary>
        /// 等待结算
        /// </summary>
        WaitSettle = 8
    }

}
