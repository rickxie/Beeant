using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Admin.Models.Commodity
{
    public class CommodityListModel:PagerModel
    {
        public override int PageSize
        {
            get { return 12; }
        }

        /// <summary>
        /// 类目编号
        /// </summary>
        public long? CatalogId { get; set; }
        /// <summary>
        /// 类目编号
        /// </summary>
        public long? TagId { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 类目
        /// </summary>
        public IList<CommodityEntity> Commodities { get; set; } 
        /// <summary>
        /// 类目
        /// </summary>
        public IList<CatalogEntity> Catalogs { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public IList<TagEntity> Tags { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public SiteEntity Site { get; set; }
       

    }
}