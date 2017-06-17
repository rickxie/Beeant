using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Admin.Models.Home;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Admin.Controllers.Home
{
 
    public class HomeController : SiteAuthorizeBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new HomeModel();
            if (Identity!=null)
            {
                var query=new QueryInfo();
                query.Query<SiteEntity>()
                    .Where(it => it.Account.Id == Identity.Id)
                    .Select(it => new object[] {it.Id, it.ExpireDate});
                var entities = this.GetEntities<SiteEntity>(query);
                model.Site = entities?.FirstOrDefault();
            }
            return View("~/Views/Home/index.cshtml", model);
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View("~/Views/Home/About.cshtml");
        }
    
    }
}
