using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;

namespace Beeant.Cloud.Crm.Mobile.Controllers.Home
{
    [CrmAuthorizeFilter]
    public class HomeController : BaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
         
            return View("~/Views/Home/index.cshtml");
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Quit(string url)
        {
             CrmAuthorizeFilterAttribute.RemoveCache(Identity);
             url = string.Format("{0}/home/quit?url={1}", this.GetUrl("PresentationMobileLoginUrl"), url);
            return Redirect(url);
        }
    }
}
