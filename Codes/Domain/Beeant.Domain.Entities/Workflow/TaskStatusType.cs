using System;

namespace Beeant.Domain.Entities.Workflow
{
    [Serializable]
    public enum TaskStatusType
    {
        /// <summary>
        /// 创建
        /// </summary>
        Created=1,
        /// <summary>
        /// 可以操作
        /// </summary>
        Waiting = 2,
        /// <summary>
        /// 通过
        /// </summary>
        Passed = 3,
        /// <summary>
        /// 拒绝
        /// </summary>
        Rejected = 4
    }
}
