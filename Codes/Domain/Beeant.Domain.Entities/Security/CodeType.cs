using System;

namespace Beeant.Domain.Entities.Security
{
    [Serializable]
    public enum CodeType
    {
        /// <summary>
        /// 邮件
        /// </summary>
        Email = 1,
        /// <summary>
        /// 手机号码
        /// </summary>
        Mobile = 2
    }
}
