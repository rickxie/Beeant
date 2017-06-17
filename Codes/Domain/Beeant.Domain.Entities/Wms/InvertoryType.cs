using System;

namespace Beeant.Domain.Entities.Wms
{
    /// <summary>
    /// 出入库类型
    /// </summary>
    [Serializable]
    public enum InvertoryType
    {
        /// <summary>
        /// 不同步
        /// </summary>
        None = 1,
        /// <summary>
        /// 即时
        /// </summary>
        Immediacy = 2,
        /// <summary>
        ///  周期重置
        /// </summary>
        Reset = 3

    }
}
