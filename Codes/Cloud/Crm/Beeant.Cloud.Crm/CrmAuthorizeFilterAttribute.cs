using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using System.Web.Mvc;
using Component.Extension;
using Component.Sdk;
using Dependent;
using Beeant.Application.Services;
using Beeant.Application.Services.Utility;
using Beeant.Basic.Services.Mvc.Extension.Mobile;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Crm;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Crm
{
    public class CrmAuthorizeFilterAttribute : AuthorizeFilterAttribute
    {
        private const string AuthorizeKeyName = "CrmAuthorizeKeyName";
        /// <summary>
        /// 
        /// </summary>
        public override WechatSdk WechatSdk
        {
            get { return ThirtyPartyExtension.CrmWechat(null, null); }
            set { base.WechatSdk = value; }
        }

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
        /// 得到缓存值
        /// </summary>
        /// <returns></returns>
        protected static string GetCacheKey(AccountIdentityEntity identity)
        {
            return  string.Format("{0}{1}", AuthorizeKeyName, identity.AccountId);
        }
        /// <summary>
        /// 退出
        /// </summary>
        public static void RemoveCache(AccountIdentityEntity identity)
        {
            var key = GetCacheKey(identity);
            Ioc.Resolve<ICacheApplicationService>().Remove(key);
        }
        /// <summary>
        /// 检查权限
        /// </summary>
        /// <returns></returns>
        public override bool CheckFilter(ActionExecutingContext filterContext)
        {
            if (Identity == null)
                return false;
            var key = GetCacheKey(Identity);
            var value = Ioc.Resolve<ICacheApplicationService>().Get<Dictionary<string, object>>(key);
            if (value == null)
            {
                var token = Ioc.Resolve<IIdentityApplicationService>().GetToken();
                var info = GetCrm();
                if (info != null)
                {
                    value = new Dictionary<string, object>();
                    value.Add("Id", info.Id);
                    value.Add("AccountId", info.Account == null ? 0 : info.Account.Id);
                    var curStaff = info.Staffs?.FirstOrDefault(it => it.Account!=null && it.Account.Id==Identity.Id);
                    value.Add("StaffId", curStaff == null?0: curStaff.Id);
                    Ioc.Resolve<ICacheApplicationService>().Set(key, value, token.TimeOut * 60);
                }
            }
            filterContext.Controller.ViewBag.CrmId = value == null || !value.ContainsKey("Id") ? 0 : value["Id"].Convert<long>();
            filterContext.Controller.ViewBag.CrmAccountId = value == null || !value.ContainsKey("AccountId") ? 0 : value["AccountId"].Convert<long>();
            filterContext.Controller.ViewBag.IsMainAccount = filterContext.Controller.ViewBag.CrmAccountId == Identity.AccountId;
            var staffId = value == null || !value.ContainsKey("StaffId") ? 0 : value["StaffId"].Convert<long>();
            filterContext.Controller.ViewBag.Staff = Ioc.Resolve<IApplicationService>().GetEntity<StaffEntity>(staffId);
            filterContext.Controller.ViewBag.IsAssignCustomer = filterContext.Controller.ViewBag.IsMainAccount==true
                    || (filterContext.Controller.ViewBag.Staff != null &&
                    filterContext.Controller.ViewBag.Staff.ReadCustomerType != ReadCustomerType.Self);
            return value != null;
        }
 
 
        /// <summary>
        /// 得到网站实体
        /// </summary>
        /// <returns></returns>
        protected virtual CrmEntity GetCrm()
        {
            var query = new QueryInfo();
            query.Query<CrmEntity>().Where(it => it.ExpireDate >= DateTime.Now.Date && (it.Account.Id == Identity.Id || it.Staffs.Any(s=>s.Account.Id== Identity.Id)))
                .Select(it => new object[] {it.Id,it.Account.Id,it.Staffs.Select(s=>new object[] {s.Id,s.Account.Id})});
            var infos = Ioc.Resolve<IApplicationService, CrmEntity>().GetEntities<CrmEntity>(query);
            return infos?.FirstOrDefault();
        }
   
    }
}
