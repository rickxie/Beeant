using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Cloud.Site
{
    public class SiteBaseController : BaseController
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public virtual long SiteId
        {
            get { return Site == null ? 0 : Site.Id; }
        }
        private SiteEntity _site;


        /// <summary>
        /// 身份验证
        /// </summary>
        public virtual SiteEntity Site
        {
            get
            {
                return _site ?? (_site = GetSite());
            }
        }
        /// <summary>
        /// 返回账户编号
        /// </summary>
        public virtual long AccountId
        {
            get { return Site == null || Site.Account == null ? 0 : Site.Account.Id; }
        }
        /// <summary>
        /// 重写
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!CheckAuthorize(filterContext))
            {
                var url = Site == null || !Site.IsOpenMobile ? "/Shared/NoSite" : string.Format("{0}/Home/Index/{1}", this.GetUrl("CloudSiteMobileUrl"), Site.Id);
                filterContext.Result = new RedirectResult(url);
            }
            base.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// 得到站点
        /// </summary>
        /// <returns></returns>
        protected virtual SiteEntity GetSite()
        {
            var siteId = RouteData.Values["SiteId"] ??
                            HttpContext.Request["SiteId"];
            var id = siteId.Convert<long>();
            if (id != 0)
            {
                return this.GetEntity<SiteEntity>(id);
            }
            var domain = Request.Url.Host.ToLower().Replace("www.", "");
            var query = new QueryInfo();
            query.Query<SiteEntity>().Where(it => it.Domain == domain);
            var infos = this.GetEntities<SiteEntity>(query);
            return infos?.FirstOrDefault();
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected virtual bool CheckAuthorize(ActionExecutingContext filterContext)
        {
            if (Site == null || AccountId == 0)
            {
                return false;
            }
            ViewBag.Site = Site;
            ViewBag.SiteId = Site.Domain.ToLower() == Request.Url.Host.ToLower().Replace("www.", "") ? "" : Site.Id.ToString();
            return true;
        }

    }
}
