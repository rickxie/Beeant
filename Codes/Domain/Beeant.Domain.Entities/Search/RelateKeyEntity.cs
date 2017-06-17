using System;

namespace Beeant.Domain.Entities.Search
{
    [Serializable]
    public class RelateKeyEntity : BaseEntity<RelateKeyEntity>
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
        /// 相关词
        /// </summary>
        public string KeyName { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }
    }
}
