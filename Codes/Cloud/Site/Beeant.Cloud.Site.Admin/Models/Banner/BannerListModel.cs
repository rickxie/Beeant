using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Admin.Models.Banner
{
    public class BannerListModel : PagerModel
    {
        public override int PageSize { get; set; } = 24;

        /// <summary>
        /// 类目
        /// </summary>
        public IList<BannerEntity> Banners { get; set; }
    }
}