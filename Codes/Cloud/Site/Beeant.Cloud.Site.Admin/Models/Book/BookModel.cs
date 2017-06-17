using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Admin.Models.Book
{

    public class BookModel:PagerModel
    {
      
        /// <summary>
        /// 公司信息
        /// </summary>
        public CompanyEntity Company { get; set; }
        /// <summary>
        /// 站点信息
        /// </summary>
        public SiteEntity Site { get; set; }
        /// <summary>
        /// 选择的目录本
        /// </summary>
        public AlbumEntity Album { get; set; }
        /// <summary>
        /// 图册
        /// </summary>
        public IList<AlbumEntity> Albums { get; set; } 
  
        /// <summary>
        /// 类目
        /// </summary>
        public IList<CatalogEntity> Catalogs { get; set; }
        /// <summary>
        /// 类目
        /// </summary>
        public CatalogEntity Catalog { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public IList<CommodityEntity> Commodities { get; set; }
        /// <summary>
        /// 得到产品图片
        /// </summary>
        /// <param name="commodity"></param>
        /// <returns></returns>
        public virtual string GetCommodityFileName(CommodityEntity commodity)
        {
            if (commodity == null)
                return "";
            return string.IsNullOrEmpty(commodity.AlbumFileName) ? commodity.FullFileName : commodity.AlbumFullFileName;
        }
 
    }
}