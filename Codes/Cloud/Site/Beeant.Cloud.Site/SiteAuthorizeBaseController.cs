using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Domain.Entities;
using Component.Extension;

namespace Beeant.Cloud.Site
{
    public class SiteAuthorizeBaseController : BaseController
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public virtual long SiteId
        {
            get
            {
                if (ViewBag.SiteId == null)
                {
                    ViewBag.SiteId = Identity==null?0:Identity.GetNumber<long>("SiteId");
                }
                return ViewBag.SiteId;
            }
        }

    }
}
