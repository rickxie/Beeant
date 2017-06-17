using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Admin.Models.Company;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Admin.Controllers.Company
{
    [SiteAuthorizeFilter]
    public class CompanyController : SiteAuthorizeBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model=new CompanyModel();
            model.Company = GetCompany();
            return View("~/Views/Company/index.cshtml", model);
        }
        /// <summary>
        /// 得到公司信息
        /// </summary>
        /// <returns></returns>
        protected virtual CompanyEntity GetCompany()
        {
            var query=new QueryInfo();
            query.Query<CompanyEntity>().Where(it => it.Site.Id == SiteId);
            var entities = this.GetEntities<CompanyEntity>(query);
            return entities?.FirstOrDefault();
        }
      

        #region 保存
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Save(CompanyModel model)
        {
            if (model == null)
                return null;
            var company = GetCompany();
            CompanyEntity entity = null;
            if (company == null)
            {
                entity = model.CreateEntity(SaveType.Add);
                entity.Site = new SiteEntity { Id = SiteId };
            }
            else
            {
                entity = model.CreateEntity(SaveType.Modify);
                entity.Id = company.Id;
            }
            var result = new Dictionary<string, object>();
            entity.Site = new SiteEntity { Id = SiteId };
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status",rev);
            result.Add("Message", mess);
            return this.Jsonp(result);
        }
        #endregion
 
    }
}
