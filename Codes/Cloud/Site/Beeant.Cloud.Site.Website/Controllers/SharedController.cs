using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;

namespace Beeant.Cloud.Site.Website.Controllers
{

    public class SharedController : SharedBaseController
    {
        /// <summary>
        /// 没有开通
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult NoSite()
        {
            return View("NoSite");
        }
    }
}
