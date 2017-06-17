using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Site
{
    [Serializable]
    public class TagEntity : BaseEntity<TagEntity>
    {
        /// <summary>
        /// 站点 
        /// </summary>
        public SiteEntity Site { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 商品
        /// </summary>
        public IList<CommodityTagEntity> CommodityTags { get; set; } 

    }
}
