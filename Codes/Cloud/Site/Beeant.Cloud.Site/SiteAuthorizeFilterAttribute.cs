using System;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using System.Web.Mvc;
using Component.Sdk;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Site;
using Component.Extension;

namespace Beeant.Cloud.Site
{
    public class SiteAuthorizeFilterAttribute : AuthorizeFilterAttribute
    {
       
        /// <summary>
        /// 
        /// </summary>
        public override WechatSdk WechatSdk
        {
            get { return WechatExtension.Wechat(null, null); }
            set { base.WechatSdk = value; }
        }


        public virtual long SiteId { get; set; }
        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="filterContext"></param>
        public override void RedirectPage(ActionExecutingContext filterContext)
        {
            if (Identity == null)
            {
                base.RedirectPage(filterContext);
            }
            else
            {
                filterContext.Result =
                    new RedirectResult("/Shared/NoAuthorize");
            }
        }


   
        /// <summary>
        /// 检查权限
        /// </summary>
        /// <returns></returns>
        public override bool CheckFilter(ActionExecutingContext filterContext)
        {
            if (Identity == null)
                return false;
            long siteId = Identity.Numbers!=null && Identity.Numbers.ContainsKey("SiteId")? Identity.Numbers["SiteId"].Convert<long>():0;
            var site =
                Ioc.Resolve<IApplicationService, SiteEntity>().GetEntity<SiteEntity>(siteId);
            filterContext.Controller.ViewBag.SiteId = siteId;
            filterContext.Controller.ViewBag.Site = site;
            filterContext.Controller.ViewBag.IsMainAccount = site != null && Identity != null && site.Account != null && site.Account.Id == Identity.Id;
            return site != null && site.ExpireDate > DateTime.Now.Date;
        }
    }
}
