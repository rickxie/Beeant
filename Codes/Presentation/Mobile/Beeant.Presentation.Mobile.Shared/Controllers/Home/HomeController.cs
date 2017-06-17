using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;

namespace Beeant.Presentation.Mobile.Shared.Controllers.Home
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// 底部
        /// </summary>
        /// <returns></returns>
        public ActionResult Top()
        {
            return View("~/Views/Home/_Top.cshtml");
        }
        /// <summary>
        /// 底部
        /// </summary>
        /// <returns></returns>
        public ActionResult Bottom()
        {
            return View("~/Views/Home/_Bottom.cshtml");
        }
        /// <summary>
        /// 顶部
        /// </summary>
        /// <returns></returns>
        public ActionResult Header()
        {
            return View("~/Views/Home/_Header.cshtml");
        }
        /// <summary>
        /// 底部
        /// </summary>
        /// <returns></returns>
        public ActionResult Footer()
        {
            return View("~/Views/Home/_Footer.cshtml");
        }

    }
}
