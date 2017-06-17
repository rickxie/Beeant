using System;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public enum PropertyType
    {
        /// <summary>
        /// 无
        /// </summary>
        None=1,
        /// <summary>
        /// 输入框
        /// </summary>
        Input=2,
        /// <summary>
        /// 单选框
        /// </summary>
        Select=3,
        /// <summary>
        /// 多选框
        /// </summary>
        Check=4,
        /// <summary>
        /// 组合框
        /// </summary>
        Combo=5
    }
}
