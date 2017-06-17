using System;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public enum PropertySearchType
    {
        /// <summary>
        /// 无
        /// </summary>
        None=1,
        /// <summary>
        /// 单选框
        /// </summary>
        Select=2,
        /// <summary>
        /// 多选框
        /// </summary>
        Check=3
    }
}
