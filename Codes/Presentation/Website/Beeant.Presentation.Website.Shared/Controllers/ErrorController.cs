using System.Web.Mvc;

namespace Beeant.Presentation.Website.Shared.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult General()
        {
            return View("~/Views/Error/Generic.cshtml");
        }

        public ActionResult Http404()
        {
            return View("~/Views/Error/404.cshtml");
        }

        public ActionResult Http403()
        {
            return View("~/Views/Error/403.cshtml");
        }
        public ActionResult Http413()
        {
            return View("~/Views/Error/413.cshtml");
        }
        public ActionResult VersionExpired()
        {
            return View("~/Views/Error/Generic.cshtml");
        }
    }
}
