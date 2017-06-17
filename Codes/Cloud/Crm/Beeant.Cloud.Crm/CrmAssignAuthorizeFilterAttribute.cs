using System.Web.Mvc;
using Beeant.Domain.Entities.Crm;

namespace Beeant.Cloud.Crm
{
    public class CrmAssignAuthorizeFilterAttribute : CrmAuthorizeFilterAttribute
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
                rev = filterContext.Controller.ViewBag.IsMainAccount == true
                    || (filterContext.Controller.ViewBag.Staff!=null && 
                    filterContext.Controller.ViewBag.Staff.ReadCustomerType!= ReadCustomerType.Self);
            }
            return rev;
        }
      
    }
}
