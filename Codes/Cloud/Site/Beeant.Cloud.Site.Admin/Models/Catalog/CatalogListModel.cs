using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Admin.Models.Catalog
{
    public class CatalogListModel:PagerModel
    {
        public override int PageSize { get; set; } = 24;

        /// <summary>
        /// 类目
        /// </summary>
        public IList<CatalogEntity> Catalogs { get; set; } 
    }
}