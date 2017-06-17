using System;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public enum CommentType
    {
        /// <summary>
        /// 好评
        /// </summary>
        Good=1,
        /// <summary>
        /// 一般
        /// </summary>
        General=2,
        /// <summary>
        /// 差评
        /// </summary>
        Bad=3
    }
}
