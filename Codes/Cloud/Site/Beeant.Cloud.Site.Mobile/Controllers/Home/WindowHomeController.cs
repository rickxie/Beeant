using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Mobile.Models.Home;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Mobile.Controllers.Home
{
    public class WindowHomeController : MobileSiteBaseController
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
                .Select(it => new object[] { it.Id, it.Name});
            var model = new HomeModel();
            model.Tags = this.GetEntities<TagEntity>(query);
            return View(GetViewPath("~/Views/Home/_Tag.cshtml"), model);
        }

    }
}
