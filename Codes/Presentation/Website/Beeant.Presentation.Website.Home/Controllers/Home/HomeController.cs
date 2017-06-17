using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Dependent;
using Beeant.Application.Services.Account;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Security;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Website.Home.Controllers.Home
{
    public class HomeController : BaseController
    {
    

        #region 首页

        /// <summary>
        /// 找回密码
        /// </summary>

        public virtual ActionResult Index()
        {
           
            return View("Index");
        }

    

        #endregion

    }
}
