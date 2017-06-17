using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Mobile.Models.Home;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Mobile.Controllers.Home
{
    public class HomeController : MobileSiteBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
         
            return View(GetViewPath("~/Views/Home/index.cshtml"));
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Banner()
        {
            var query=new QueryInfo();
            query.Query<BannerEntity>()
                .Where(it => it.Site.Id == SiteId && it.IsShow)
                .Select(it => new object[] {it.Id, it.Url, it.FileName});
            var model = new HomeModel();
            model.Banners = this.GetEntities<BannerEntity>(query);
            return View(GetViewPath("~/Views/Home/_Banner.cshtml"), model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Tag()
        {
            var query = new QueryInfo();
            query.Query<TagEntity>()
                .Where(it => it.Site.Id == SiteId).OrderBy(it=>it.Sequence)
                .Select(it => new object[] { it.Id, it.Name,it.CommodityTags.Take(4).
                    Where(s=>s.Commodity.Status==CommodityStatusType.Normal).
                    OrderBy(s=>s.Commodity.Sequence).Select(s=>new object[] { s.Commodity.Id,s.Commodity.Name,s.Commodity.Password,s.Commodity.FileName } ) });
            var model = new HomeModel();
            model.Tags = this.GetEntities<TagEntity>(query);
            return View(GetViewPath("~/Views/Home/_Tag.cshtml"), model);
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        /// <returns></returns>
        public ActionResult Mobile()
        {
            var query = new QueryInfo();
            query.Query<CompanyEntity>().Where(it => it.Site.Id == SiteId).Select(it=>it.Mobile);
            var entities = this.GetEntities<CompanyEntity>(query);
            var entity= entities?.FirstOrDefault();
            if (entity == null)
            {
                return Content("");
            }
            return Content(entity.Mobile);
        }
        /// <summary>
        /// 得到二维码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetQrCode()
        {
            var url = string.Format("{0}/Home/Index/{1}", this.GetUrl("CloudSiteMobileUrl"), SiteId);
            var bs = QrEncodHelper.Create(url);
            return File(bs, "image/jpeg");
        }
    }
}
