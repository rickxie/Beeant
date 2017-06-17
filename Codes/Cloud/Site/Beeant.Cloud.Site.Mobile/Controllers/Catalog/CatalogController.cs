using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Mobile.Models.Catalog;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Mobile.Controllers.Catalog
{
    public class CatalogController : MobileSiteBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
         
            return View(GetViewPath("~/Views/Catalog/index.cshtml"));
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int? page)
        {
            var model = new CatalogListModel
            {
                PageIndex = page.HasValue ? page.Value : 0
            };

            model.Catalogs = GetCatalogs(model);
            if (model.Catalogs == null || model.Catalogs.Count == 0)
                return Content("");
            return View(GetViewPath("~/Views/Catalog/_Catalog.cshtml"), model);
        }

        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CatalogEntity> GetCatalogs(CatalogListModel model)
        {
            var query = new QueryInfo();
            query.Query<CatalogEntity>().Where(it => it.Site.Id == SiteId)
                .OrderBy(it => it.Sequence).Select(it => new object[] { it.Id, it.Name,it.FileName,
                    it.Commodities.Take(1).
                    Where(s=>s.Status==CommodityStatusType.Normal).
                    OrderBy(s=>s.Sequence).Select(s=>new object[] {s.Id,s.Password,s.FileName}) });
            return this.GetEntities<CatalogEntity>(query);
        }

    }
}
