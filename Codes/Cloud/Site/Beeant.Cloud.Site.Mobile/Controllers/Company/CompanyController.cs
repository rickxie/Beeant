using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Mobile.Models.Company;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Mobile.Controllers.Company
{
    public class CompanyController : MobileSiteBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new CompanyModel();

            return View(GetViewPath("~/Views/Company/index.cshtml"), model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            var model = new CompanyModel();
            model.Company = GetCompany();
            return View(GetViewPath("~/Views/Company/_detail.cshtml"), model);
        }
        /// <summary>
        /// 得到公司信息
        /// </summary>
        /// <returns></returns>
        protected virtual CompanyEntity GetCompany()
        {
            var query = new QueryInfo();
            query.Query<CompanyEntity>().Where(it => it.Site.Id == SiteId);
            var entities = this.GetEntities<CompanyEntity>(query);
            return entities?.FirstOrDefault();
        }

    }
}
