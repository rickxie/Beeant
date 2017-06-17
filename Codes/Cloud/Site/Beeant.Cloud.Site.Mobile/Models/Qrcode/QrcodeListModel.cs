using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Mobile.Models.Qrcode
{
    public class QrcodeListModel : PagerModel
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        public override int PageSize
        {
            get { return 24; }
        }
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public IList<CommodityEntity> Commodities { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public IList<CatalogEntity> Catalogs { get; set; }
    }

  
}
