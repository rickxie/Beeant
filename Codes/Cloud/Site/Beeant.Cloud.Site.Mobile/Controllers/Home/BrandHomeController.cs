using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Mobile.Models.Home;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Mobile.Controllers.Home
{
    public class BrandHomeController : MobileSiteBaseController
    {
        
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Tag()
        {
            var query = new QueryInfo();
            query.Query<TagEntity>()
                .Where(it => it.Site.Id == SiteId).OrderBy(it => it.Sequence)
                .Select(it => new object[] { it.Id, it.Name,it.CommodityTags.Take(1).
                    Where(s=>s.Commodity.Status==CommodityStatusType.Normal).
                    OrderBy(s=>s.Commodity.Sequence).Select(s=>new object[] { s.Commodity.Id,s.Commodity.Password,s.Commodity.FileName } ) });
            var model = new HomeModel();
            model.Tags = this.GetEntities<TagEntity>(query);
            return View(GetViewPath("~/Views/Home/_Tag.cshtml"), model);
        }

    }
}
