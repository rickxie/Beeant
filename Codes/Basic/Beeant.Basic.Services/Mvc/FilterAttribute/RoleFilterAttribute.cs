using System.Web.Mvc;
using Beeant.Application.Services.Authority;
using Dependent;
using Beeant.Domain.Entities.Authority;


namespace Beeant.Basic.Services.Mvc.FilterAttribute
{
    public class RoleFilterAttribute : AuthorizeFilterAttribute
    {
        /// <summary>
        /// 是否验证角色
        /// </summary>
        public bool IsVerifyRole { get; set; }
        public override bool CheckFilter(ActionExecutingContext filterContext)
        {
            var rev= base.CheckFilter(filterContext);
            if (rev && IsVerifyRole)
            {
                rev = VerifyResource(filterContext, Identity.Id);
            }
            return rev;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="accountId"></param>
        public static bool VerifyResource(ActionExecutingContext filterContext,long accountId)
        {
            var info = GetVerification(filterContext,accountId);
            if (info == null) return false;
            filterContext.Controller.ViewBag.Verification = info;
            return info.IsPass;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="accountId"></param>
        public static VerificationEntity GetVerification(ActionExecutingContext filterContext,long accountId)
        {
           
            var info = Ioc.Resolve<IAuthorityEngineApplicationService>()
                          .GeVerificationEntity(accountId,  filterContext.RequestContext.HttpContext.Request.RawUrl);
            return info;

        }
    }
}












