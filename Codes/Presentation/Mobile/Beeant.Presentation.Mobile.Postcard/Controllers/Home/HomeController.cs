using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Domain.Entities.Cms;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Mobile.Postcard.Controllers.Home
{
    public class HomeController : MobileBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(long id)
        {
            var query=new QueryInfo();
            query.Query<PostcardEntity>()
                .Where(it => it.Id == id && it.IsShow)
                .Select(it => new object[] {it.Name, it.Detail});
            var infos = this.GetEntities<PostcardEntity>(query);
            var info = infos?.FirstOrDefault();
            return View("~/Views/Home/index.cshtml",info);
        }
    }
}
