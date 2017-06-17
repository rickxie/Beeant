using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Website.Models.News;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Website.Controllers.News
{
    public class NewsController : WebSiteBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new NewsListModel();
            return View(GetViewPath("~/Views/News/index.cshtml"), model);
        }
 
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int? page)
        {
            var model = new NewsListModel
            {
                PageIndex = page.HasValue ? page.Value : 0
            };
           
            model.Newses = GetNewses(model);
            if (model.Newses == null || model.Newses.Count == 0)
                return Content("");
            return View(GetViewPath("~/Views/News/_News.cshtml"), model);
        }
 
        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<NewsEntity> GetNewses(NewsListModel model)
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageIndex(model.PageIndex).SetPageSize(model.PageSize).Query<NewsEntity>()
                .Where(it => it.Site.Id == SiteId).
                OrderBy(it => it.Sequence).Select(it => new object[] { it.Id, it.Title,it.Content});
            return this.GetEntities<NewsEntity>(query);
        }
        
    }
}
