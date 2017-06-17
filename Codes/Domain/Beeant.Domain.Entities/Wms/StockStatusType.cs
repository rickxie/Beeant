using System;

namespace Beeant.Domain.Entities.Wms
{
    [Serializable]
    public enum StockStatusType
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
        /// 等待确认
        /// </summary>
        WaitAudit = 4
    }

}
