using System;

namespace Beeant.Domain.Entities.Workflow
{
    [Serializable]
    public enum NodeAssignType
    {

        /// <summary>
        /// 随机
        /// </summary>
        Random = 1,
        /// <summary>
        /// 平均
        /// </summary>
        Average = 2
    }
}
