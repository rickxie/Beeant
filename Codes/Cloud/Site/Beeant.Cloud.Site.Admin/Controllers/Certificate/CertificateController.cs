using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Cloud.Site.Admin.Models.Certificate;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Admin.Controllers.Certificate
{
    [SiteAuthorizeFilter]
    public class CertificateController : SiteAuthorizeBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new CertificateListModel();

            return View("~/Views/Certificate/index.cshtml", model);
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
            model.Certificates = GetCertificates(model);
            if (model.Certificates == null || model.Certificates.Count == 0)
                return Content("");
            return View("~/Views/Certificate/_Certificate.cshtml", model);
        }
        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CertificateEntity> GetCertificates(CertificateListModel model)
        {
            var query = new QueryInfo();
            query.SetPageSize(model.PageSize).SetPageIndex(model.PageIndex).Query<CertificateEntity>().Where(it => it.Site.Id == SiteId)
                .OrderBy(it => it.Sequence).Select(it => new object[] { it.Id,it.FileName, it.Sequence });
            return this.GetEntities<CertificateEntity>(query);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [SiteDataFilter(EntityType = typeof(CertificateEntity))]
        public virtual ActionResult Get(long id)
        {
            var entity = this.GetEntity<CertificateEntity>(id);
            if (entity == null)
                return null;
            var model = new CertificateModel
            {
                Id = entity.Id.ToString(),
                Sequence = entity.Sequence,
                IsShow=entity.IsShow,
                FileName = entity.FullFileName
            };
            return this.Jsonp(model);
        }
        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Add(CertificateModel model)
        {
            if (model == null)
                return null;
            var entity = model.CreateEntity(SaveType.Add);
            var result = new Dictionary<string, object>();
            entity.Site = new SiteEntity { Id = SiteId };
            entity.Sequence = GetSequence();
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status", rev);
            result.Add("Id", entity.Id);
            result.Add("Message", mess);
            return this.Jsonp(result);
        }

        /// <summary>
        /// D
        /// </summary>
        /// <returns></returns>
        protected virtual int GetSequence()
        {
            var query = new QueryInfo();
            query.SetPageSize(1).Query<CertificateEntity>().Where(it => it.Site.Id == SiteId)
                .OrderBy(it => it.Sequence).Select(it => it.Sequence);
            var entities = this.GetEntities<CertificateEntity>(query);
            var entity = entities?.FirstOrDefault();
            if (entity != null)
            {
                if (entity.Sequence + 5000 > 100000000)
                    return 100000000;
                return entity.Sequence + 5000;
            }
            return -100000000;
        }
        #endregion

        #region 修改

        [SiteDataFilter(EntityType = typeof(CertificateEntity))]
        [HttpPost]
        public virtual ActionResult Modify(CertificateModel model)
        {
            if (model == null)
                return null;
            var entity = model.CreateEntity(SaveType.Modify);
            var result = new Dictionary<string, object>();
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status", rev);
            result.Add("Message", mess);
            return this.Jsonp(result);
        }
        #endregion

        #region 删除
        [SiteDataFilter(EntityType = typeof(CertificateEntity))]
        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<CertificateEntity>();
                foreach (var i in id)
                {
                    var info = new CertificateEntity
                    {
                        Id = i.Convert<long>(),
                        SaveType = SaveType.Remove
                    };
                    infos.Add(info);
                }
                rev = this.SaveEntities(infos);
            }
            result.Add("Status", rev);
            return this.Jsonp(result);
        }
        #endregion
    }
}
