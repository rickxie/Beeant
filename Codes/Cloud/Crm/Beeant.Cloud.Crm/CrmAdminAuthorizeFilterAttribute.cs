using System.Web.Mvc;

namespace Beeant.Cloud.Crm
{
    public class CrmAdminAuthorizeFilterAttribute : CrmAuthorizeFilterAttribute
    { 
        /// <summary>
        /// 检查权限
        /// </summary>
        /// <returns></returns>
        public override bool CheckFilter(ActionExecutingContext filterContext)
        {
            var rev = base.CheckFilter(filterContext);
            if (rev)
            {
                rev = filterContext.Controller.ViewBag.IsMainAccount == true;
            }
            return rev;
        }
      
    }
}
