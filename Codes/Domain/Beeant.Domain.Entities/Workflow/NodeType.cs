using System;

namespace Beeant.Domain.Entities.Workflow
{
    [Serializable]
    public enum NodeType
    {
        /// <summary>
        /// 开始
        /// </summary>
        Start=1,
        /// <summary>
        /// 单节点
        /// </summary>
        Single = 2,
        /// <summary>
        /// 所以通过
        /// </summary>
        All = 3,
        /// <summary>
        /// 任意
        /// </summary>
        Any=4,
        /// <summary>
        /// 结束
        /// </summary>
        End=5
    }
}
