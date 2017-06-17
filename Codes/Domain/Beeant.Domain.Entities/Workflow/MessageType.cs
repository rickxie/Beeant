using System;

namespace Beeant.Domain.Entities.Workflow
{
    [Serializable]
    public enum MessageType
    {

        /// <summary>
        /// 默认
        /// </summary>
        Default = 1,
        /// <summary>
        /// 邮箱
        /// </summary>
        Email = 2,
        /// <summary>
        /// 短信
        /// </summary>
        Mobile = 4
    }
}
