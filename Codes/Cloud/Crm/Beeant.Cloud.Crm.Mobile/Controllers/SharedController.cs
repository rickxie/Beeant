using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;

namespace Beeant.Cloud.Crm.Mobile.Controllers
{

    public class SharedController : SharedBaseController
    {

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult NoAuthorize()
        {
            return View("~/Views/Shared/NoAuthorize.cshtml");
        }
  

    }
}
