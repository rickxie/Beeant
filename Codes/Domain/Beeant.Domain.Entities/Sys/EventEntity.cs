using System;

namespace Beeant.Domain.Entities.Sys
{
    [Serializable]
    public class EventEntity : BaseEntity<EventEntity>
    {
        /// <summary>
        /// 执行事件的类
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
