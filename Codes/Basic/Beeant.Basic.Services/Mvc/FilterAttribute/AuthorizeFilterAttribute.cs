using System.Web;
using System.Web.Mvc;
using Component.Extension;
using Component.Sdk;
using Configuration;
using Dependent;
using Beeant.Application.Services.Utility;
using Beeant.Basic.Services.Mvc.Extension.Mobile;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;


namespace Beeant.Basic.Services.Mvc.FilterAttribute
{
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        private const string TicketKey = "ticket";
        private  IdentityEntity _identity;
        private string _ticketKeyValue;
        /// <summary>
        /// 是否跳转
        /// </summary>
        public bool IsRedirect { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual WechatSdk WechatSdk { get; set; } = ThirtyPartyExtension.Wechat(null);
        /// <summary>
        /// 身份验证
        /// </summary>
        public virtual IdentityEntity Identity
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[TicketKey];
                if (cookie != null)
                {
                    if (_ticketKeyValue == cookie.Value)
                    {
                        return _identity ?? (_identity = Ioc.Resolve<IIdentityApplicationService>().Get<IdentityEntity>());
                    }
                    _ticketKeyValue = cookie.Value;
                    _identity = Ioc.Resolve<IIdentityApplicationService>().Get<IdentityEntity>();
                    return _identity;
                }
                return null;
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!CheckFilter(filterContext))
                RedirectPage(filterContext);
            base.OnActionExecuting(filterContext);
        }
        public virtual bool CheckFilter(ActionExecutingContext filterContext)
        {
            return Identity != null;
        }
        /// <summary>
        /// 验证失败
        /// </summary>
        public virtual void RedirectPage(ActionExecutingContext filterContext)
        {
            if (WechatSdk.CheckWechatBrower())
            {
                RedirectWechatLoginPage(filterContext); 
            }
            else if (filterContext.HttpContext.Request.Url != null && (filterContext.HttpContext.Request.Browser.IsMobileDevice || filterContext.HttpContext.Request.Url.AbsoluteUri.Contains("m.")))
            {
                RedirectMobileLoginPage(filterContext);
            }
            else
            {
                RedirectWebsiteLoginPage(filterContext);
            }
        }
        /// <summary>
        /// 手机端登陆
        /// </summary>
        /// <param name="filterContext"></param>
        public virtual void RedirectWechatLoginPage(ActionExecutingContext filterContext)
        {
            var url = string.Format("{0}/Home/WechatLogin?url={1}", ConfigurationManager.GetSetting<string>("PresentationMobileLoginUrl"),
                         filterContext.HttpContext.Server.UrlEncode(
                             filterContext.HttpContext.Request.Url.AbsoluteUri));
            var json = ConfigurationManager.GetSetting<string>("AutoLogin").DeserializeJson<dynamic>();
            if (WechatSdk!=null && json != null && json.Weixin == true)
            {
                url = string.Format("{0}/Wechat/Oauth?url={1}",
                    ConfigurationManager.GetSetting<string>("DistributedOutsideReceptionUrl"),
                    filterContext.HttpContext.Server.UrlEncode(url));
                url = WechatSdk.CreateAuthorityUrl(url, true);
            }
            filterContext.Result =
                     new RedirectResult(url);

        }
        /// <summary>
        /// 手机端登陆
        /// </summary>
        /// <param name="filterContext"></param>
        public virtual void RedirectMobileLoginPage(ActionExecutingContext filterContext)
        {
            filterContext.Result =
                     new RedirectResult(string.Format("{0}?url={1}", ConfigurationManager.GetSetting<string>("PresentationMobileLoginUrl"),
                         filterContext.HttpContext.Server.UrlEncode(
                             filterContext.HttpContext.Request.Url.AbsoluteUri)));

        }
        /// <summary>
        /// 网站登陆
        /// </summary>
        /// <param name="filterContext"></param>
        public virtual void RedirectWebsiteLoginPage(ActionExecutingContext filterContext)
        {
            filterContext.Result =
                 new RedirectResult(string.Format("{0}?url={1}", ConfigurationManager.GetSetting<string>("PresentationWebsiteLoginUrl"),
                     filterContext.HttpContext.Server.UrlEncode(
                         filterContext.HttpContext.Request.Url.AbsoluteUri)));

        }
    }
}












