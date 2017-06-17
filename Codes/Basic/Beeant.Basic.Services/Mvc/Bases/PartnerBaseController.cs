using System;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Merchant;
using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.Mvc.Extension;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Basic.Services.Mvc.Bases
{
    public class PartnerBaseController : BaseController
    {
        public virtual string PartnerIdName
        {
            get { return "pid"; }
        }

        private PartnerEntity _partner;
        /// <summary>
        /// 加盟商信息
        /// </summary>
        public virtual PartnerEntity Partner
        {
            get { return _partner ?? (_partner = GetPartner()); }
        }
        /// <summary>
        /// 账户编号
        /// </summary>
        public virtual long PartnerAccountId
        {
            get { return Partner == null || Partner.Account == null ? 0 : Partner.Account.Id; }
        }
        /// <summary>
        /// 得到加盟商信息
        /// </summary>
        /// <returns></returns>
        protected virtual PartnerEntity GetPartner()
        {
            var domain = Request.Url.Host.Replace("www.", "");
            var value = RouteData.Values[PartnerIdName] ??
                      HttpContext.Request.QueryString[PartnerIdName];
            var query = new QueryInfo();
            query.SetCacheTime(DateTime.MaxValue).Query<PartnerEntity>()
                .Where(it => it.Domain == domain || it.Id == value.Convert<long>());
            var infos = this.GetEntities<PartnerEntity>(query);
            if (infos == null)
                return null;
            var info= infos.FirstOrDefault();
            if (info != null)
            {
                ViewBag.PartnerTitle = info.Name;
                ViewBag.PartnerLogo = info.FullFileName;
                SetPartnerStyle(info);
            }
            return info;
        }
        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (Partner == null)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Error/Http404");
            }
        }
        /// <summary>
        /// 设置风格
        /// </summary>
        /// <param name="partner"></param>
        protected virtual void SetPartnerStyle(PartnerEntity partner)
        {
            if(partner==null)
                return;
            var query = new QueryInfo();
            query.SetCacheTime(DateTime.MaxValue).From<StyleEntity>();
            var infos = this.GetEntities<StyleEntity>(query);
            if (infos != null)
            {
                if (partner.MobileStyle != null)
                    partner.MobileStyle = infos.FirstOrDefault(it => it.Id == partner.MobileStyle.Id);
                if (partner.WebsiteStyle != null)
                    partner.WebsiteStyle = infos.FirstOrDefault(it => it.Id == partner.WebsiteStyle.Id);
            }
        }
    
    }
}
