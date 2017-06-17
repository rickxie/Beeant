using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Website.Models.Company;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Website.Controllers.Company
{
    public class CompanyController : WebSiteBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            var model = new CompanyModel();
            model.Company = GetCompany();
            return View(GetViewPath("~/Views/Company/About.cshtml"), model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            var model = new CompanyModel();
            model.Company = GetCompany();
            return View(GetViewPath("~/Views/Company/Contact.cshtml"), model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Get(bool? isDetail)
        {
            var company = GetCompany();
            var result = new Dictionary<string, object>();
            if (company != null)
            {
                result.Add("Name",company.Name);
                result.Add("Address", company.Address);
                result.Add("Mobile", company.Mobile);
                result.Add("Email", company.Email);
                result.Add("Fax", company.Fax);
                result.Add("Qq", company.Qq);
                result.Add("Linkman", company.Linkman);
                result.Add("RecordNumber", company.RecordNumber);
                result.Add("WeixinQrCodeUrl",string.IsNullOrEmpty(company.WeixinQrCodeFileName)?"": company.WeixinQrCodeFullFileName);
                if(isDetail==true)
                    result.Add("Detail", company.Detail);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
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
