using System;

namespace Beeant.Domain.Entities.Sys
{
    [Serializable]
    public class QueueEntity : BaseEntity<QueueEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}
