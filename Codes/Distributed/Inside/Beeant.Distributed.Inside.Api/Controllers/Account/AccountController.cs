using System.Web.Mvc;
using Dependent;
using Beeant.Application.Services.Utility;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;

namespace Beeant.Distributed.Inside.Api.Controllers.Account
{

    public class AccountController :Controller
    {
        //
        // GET: /Test/
 
        public virtual ActionResult GetLoginInfo(string ticket)
        {
            var info = Ioc.Resolve<IIdentityApplicationService>().Get<IdentityEntity>(ticket);
            return Json(info, JsonRequestBehavior.AllowGet);
        }

    }
}
