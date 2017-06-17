using System;

namespace Beeant.Domain.Entities.Site
{
    [Serializable]
    public class NewsEntity : BaseEntity<NewsEntity>
    {
        /// <summary>
        /// 站点
        /// </summary>
        public SiteEntity Site { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        ///邮箱必填
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
    }
}
