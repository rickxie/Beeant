using System.Web.Mvc;
using System.Web.Routing;
using Dependent;
using Beeant.Application.Services.Utility;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;

namespace Beeant.Basic.Services.Mvc.Bases
{
    public class BaseController : Controller
    {
   

        protected override void Initialize(RequestContext requestContext)
        {
            if (ViewBag.Identity==null)
            ViewBag.Identity = Identity;
            base.Initialize(requestContext);
        }

        /// <summary>
        /// 没有找到视图
        /// </summary>
        /// <param name="message"></param>
        /// <param name="viewPath"></param>
        /// <returns></returns>
        public virtual ActionResult None(string message, string viewPath = "/Views/Shared/none.cshtml")
        {
            ViewBag.Message = message;
            return View(viewPath);            
        }

        private bool _isGetIdentity;
        private IdentityEntity _identity;
        /// <summary>
        /// 身份验证
        /// </summary>
        public virtual IdentityEntity Identity
        {
            get
            {
                if (!_isGetIdentity)
                {
                    _identity = Ioc.Resolve<IIdentityApplicationService>().Get<IdentityEntity>();
                    _isGetIdentity = true;
                }
                return _identity;
            }
        }

      

        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        protected virtual string GetViewPath(string path, string style)
        {
            if (string.IsNullOrEmpty(style))
                return path;
            var index = path.LastIndexOf("/");
            if (index == -1)
                return path;
            var newpath = path.Insert(index, string.Format("/{0}", style));
            return System.IO.File.Exists(Server.MapPath(newpath)) ? newpath : path;
        }
    }
}
