using System;

namespace Beeant.Domain.Entities.Site
{
    [Serializable]
    public class MessageEntity : BaseEntity<MessageEntity>
    {
        /// <summary>
        /// 站点
        /// </summary>
        public SiteEntity Site { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///内容
        /// </summary>
        public string Content { get; set; }

    }
}
