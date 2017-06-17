using System;

namespace Beeant.Domain.Entities.Site
{
    [Serializable]
    public class InqueryEntity : BaseEntity<InqueryEntity>
    {
        /// <summary>
        /// 账户
        /// </summary>
        public SiteEntity Site { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Linkman { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 资讯内容
        /// </summary>
        public string Content { get; set; }

    }
}
