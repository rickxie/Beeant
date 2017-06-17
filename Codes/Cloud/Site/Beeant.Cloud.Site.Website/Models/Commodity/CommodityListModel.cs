using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Website.Models.Commodity
{
    public class CommodityListModel : PagerModel
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        public override int PageSize { get; set; } = 24;

        /// <summary>
        /// 类目
        /// </summary>
        public long? CatalogId { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public long? TagId { get; set; }
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// 类目
        /// </summary>
        public IList<CatalogEntity> Catalogs { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public IList<CommodityEntity> Commodities { get; set; }
        /// <summary>
        /// 得到图片
        /// </summary>
        /// <returns></returns>
        public virtual string CatalogFullFileName(CatalogEntity catalog)
        {
            if (!string.IsNullOrEmpty(catalog.FileName))
                return catalog.FullFileName;
            if (catalog.Commodities == null || catalog.Commodities.Count == 0)
                return "";
            var commodity = catalog.Commodities.First();
            if (string.IsNullOrEmpty(commodity.FileName))
                return "";
            if (!string.IsNullOrEmpty(commodity.Password))
                return "/Images/lock.png";
            return commodity.GetFullFileName(commodity.FileName, "i");
        }
        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <returns></returns>
        public virtual string GetFullFileName(CommodityEntity commodity)
        {
            if (!string.IsNullOrEmpty(commodity.Password))
                return "/Images/lock.png";
            return string.Format("{0}?v={1}", commodity.GetFullFileName(commodity.FileName, "l"), DateTime.Now.Ticks);
        }
    }
}
