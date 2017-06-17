using System;

namespace Beeant.Domain.Entities.Finance
{
    [Serializable]
    public enum PayinStatusType
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
        /// 等待审核
        /// </summary>
        WaitAudit = 4
    }

}
