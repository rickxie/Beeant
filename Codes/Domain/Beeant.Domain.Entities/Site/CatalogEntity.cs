using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Site
{
    [Serializable]
    public class CatalogEntity : BaseEntity<CatalogEntity>
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
        ///文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] FileByte { get; set; }
        /// <summary>
        /// 全部文件路径
        /// </summary>
        public string FullFileName
        {
            get { return this.GetFullFileName(FileName); }
        }

        /// <summary>
        /// 商品
        /// </summary>
        public IList<CommodityEntity> Commodities { get; set; } 

    }
}
