using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Mobile.Models.Certificate;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Mobile.Controllers.Certificate
{
    public class CertificateController : MobileSiteBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
         
            return View(GetViewPath("~/Views/Certificate/index.cshtml"));
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int? page)
        {
            var model = new CertificateListModel
            {
                PageIndex = page.HasValue ? page.Value : 0
            };

            model.Certificaties = GetCertificates(model);
            if (model.Certificaties == null || model.Certificaties.Count == 0)
                return Content("");
            return View(GetViewPath("~/Views/Certificate/_Certificate.cshtml"), model);
        }

        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CertificateEntity> GetCertificates(CertificateListModel model)
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageIndex(model.PageIndex).SetPageSize(model.PageSize).Query<CertificateEntity>()
                .Where(it => it.Site.Id == SiteId && it.IsShow).
                OrderBy(it => it.Sequence).Select(it => new object[] { it.Id, it.FileName });
            return this.GetEntities<CertificateEntity>(query);
        }

    }
}
