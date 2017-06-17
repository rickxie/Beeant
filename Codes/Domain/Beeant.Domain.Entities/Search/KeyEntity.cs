using System;

namespace Beeant.Domain.Entities.Search
{
    [Serializable]
    public class KeyEntity : BaseEntity<KeyEntity>
    {
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ip地址
        /// </summary>
        public string Ip { get; set; }

    }
}
