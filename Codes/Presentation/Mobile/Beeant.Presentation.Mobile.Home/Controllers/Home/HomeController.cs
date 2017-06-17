using System;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Domain.Entities.Agent;
using Beeant.Domain.Entities.Cms;
using Beeant.Domain.Entities.Product;
using Beeant.Presentation.Mobile.Home.Models.Home;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Mobile.Home.Controllers.Home
{
    public class HomeController : MobileBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("~/Views/Home/index.cshtml");
        }
        /// <summary>
        /// 广告栏
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Banner()
        {
            var model=new HomeModel();
            var query=new QueryInfo();
            query.SetCacheName("首页横幅").SetCacheTime(DateTime.Now.AddHours(2))
                .Query<ContentEntity>().Where(it => it.Class.Tag == "Mobile_Home_Banner" && it.IsShow);
            model.Contents = this.GetEntities<ContentEntity>(query);
            return View("~/Views/Home/_Banner.cshtml", model);
        }
        /// <summary>
        /// 得到产品
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Goods(int page)
        {
            var quey=new QueryInfo();
            quey.SetPageSize(8).SetPageIndex(page).Query<GoodsEntity>()
                .Where(it => it.IsSales).OrderByDescending(it=>it.Sequence)
                .Select(it => new object[] {it.Id, it.FileName, it.Name,it.Price});
            var infos = this.GetEntities<GoodsEntity>(quey);
            if (infos == null || infos.Count == 0)
                return Content("");
            var model = new HomeModel
            {
                Goodses= infos,
                Agent=GetAgent()
            };
            return View("~/Views/Home/_Goods.cshtml", model);
        }
        /// <summary>
        /// 得到代理
        /// </summary>
        /// <returns></returns>
        public virtual AgentEntity GetAgent()
        {
            if (Identity == null)
                return null;
            var query=new QueryInfo();
            query.Query<AgentEntity>().Where(it => it.Account.Id == Identity.Id && it.IsUsed)
                .Select(it => it.Setting);
            return this.GetEntities<AgentEntity>(query)?.FirstOrDefault();
        }
    }
}
