using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Site
{
    [Serializable]
    public class CommodityTagEntity : BaseEntity<CommodityTagEntity>
    {
        /// <summary>
        /// 标签 
        /// </summary>
        public TagEntity Tag { get; set; }

        /// <summary>
        /// 产品 
        /// </summary>
        public CommodityEntity Commodity { get; set; }
    }
}
