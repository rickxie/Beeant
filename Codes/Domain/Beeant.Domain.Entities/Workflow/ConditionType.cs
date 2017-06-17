using System;

namespace Beeant.Domain.Entities.Workflow
{
    [Serializable]
    public enum ConditionType
    {

        /// <summary>
        /// 满足条件生成
        /// </summary>
        Create = 1,
        /// <summary>
        /// 满足条件不生成
        /// </summary>
        UnCreate = 2,
        /// <summary>
        /// 满足条件跳过
        /// </summary>
        Skip = 3
    }
}
