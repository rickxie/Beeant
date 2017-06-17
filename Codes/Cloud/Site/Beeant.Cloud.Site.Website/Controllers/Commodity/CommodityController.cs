using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Website.Models.Commodity;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Website.Controllers.Commodity
{
    public class CommodityController : WebSiteBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string key, string catalogId, string tagId)
        {
            ViewBag.SearchKey = key;
            var model = new CommodityListModel
            {
                CatalogId = string.IsNullOrWhiteSpace(catalogId) ? null : (long?)catalogId.Convert<long>(),
                TagId = string.IsNullOrWhiteSpace(tagId) ? null : (long?)tagId.Convert<long>(),
                Key = key
            };
            return View(GetViewPath("~/Views/Commodity/index.cshtml"), model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Catalog()
        {
            var model = new CommodityListModel();
            model.Catalogs = GetCatalogs();
            model.Logo = Site.LogoFullFileName;
            if (model.Catalogs == null || model.Catalogs.Count == 0)
                return Content("");
            return View(GetViewPath("~/Views/Commodity/_Catalog.cshtml"), model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string catalogId,string tagId,string key, int? page)
        {
            var model = new CommodityListModel
            {
                Key=key,
                PageIndex = page.HasValue ? page.Value : 0
            };

            if (!string.IsNullOrEmpty(catalogId))
                model.CatalogId = catalogId.Convert<long>();
            if (!string.IsNullOrEmpty(tagId))
                model.TagId = tagId.Convert<long>();
            model.Commodities = GetCommodities(model);
            if (model.Commodities == null || model.Commodities.Count == 0)
                return Content("");
            return View(GetViewPath("~/Views/Commodity/_Commodity.cshtml") , model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Top()
        {
            var model = new CommodityListModel
            {
                PageSize = 8
            };
            model.Commodities = GetCommodities(model);
            if (model.Commodities == null || model.Commodities.Count == 0)
                return Content("");
            return View(GetViewPath("~/Views/Commodity/_Commodity.cshtml"), model);
        }
        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CatalogEntity> GetCatalogs()
        {
            var query = new QueryInfo();
            query.Query<CatalogEntity>().Where(it=>it.Site.Id==SiteId)
                .OrderBy(it => it.Sequence).Select(it => new object[] { it.Id, it.Name,it.FileName,
                    it.Commodities.Take(1).
                    Where(s=>s.Status==CommodityStatusType.Normal).
                    OrderBy(s=>s.Sequence).Select(s=>new object[] {s.Password, s.FileName}) });
            return this.GetEntities<CatalogEntity>(query);
        }
        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CommodityEntity> GetCommodities(CommodityListModel model)
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageIndex(model.PageIndex).SetPageSize(model.PageSize).Query<CommodityEntity>()
                .Where(it => it.Site.Id == SiteId && it.Status==CommodityStatusType.Normal).
                OrderBy(it => it.Sequence).Select(it => new object[] { it.Id, it.Name,it.Description,it.IsShowPrice,it.Price, it.FileName,it.Password});
            if (model.CatalogId.HasValue)
            {
                query.Where(string.Format("{0} && Catalog.Id==@CatalogId", query.WhereExp))
                    .SetParameter("CatalogId", model.CatalogId);
            }
            if (model.TagId.HasValue)
            {
                query.Where(string.Format("{0} && CommodityTags.Count(Tag.Id==@TagId)>0", query.WhereExp))
                    .SetParameter("TagId", model.TagId);
            }
            if (!string.IsNullOrEmpty(model.Key))
            {
                query.Where(string.Format("{0} && (Name.Contains(@Key) || Description.Contains(@Key))", query.WhereExp))
                    .SetParameter("Key", model.Key);
            }
            return this.GetEntities<CommodityEntity>(query);
        }

        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetFileName(long id,string password)
        {
            var query=new QueryInfo();
            query.Query<CommodityEntity>().Where(it => it.Id == id && it.Site.Id == SiteId)
                .Select(it => new object[] { it.FileName ,it.Password} );
            var entities = this.GetEntities<CommodityEntity>(query);
            var enitty = entities?.FirstOrDefault();
            var dis=new Dictionary<string,object>();
            if (enitty != null && enitty.Password== password)
            {
                dis.Add("FileName", enitty.GetFullFileName(enitty.FileName,"l"));
            }
            return this.Jsonp(dis);
        }
        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Detail(long id, string password)
        {
            var model=new CommodityModel();
            var entity = GetCommodity(id);
            if (entity != null && (string.IsNullOrEmpty(entity.Password) || entity.Password== password))
            {
                model.FileNames=new List<string>
                {
                    entity.FullFileName
                };
                var query = new QueryInfo();
                query.Query<CommodityImageEntity>().Where(it => it.Commodity.Id == id && it.Site.Id == SiteId)
                    .Select(it => new object[] { it.FileName });
                var entities = this.GetEntities<CommodityImageEntity>(query);
                if (entities != null)
                {
                    model.FileNames.AddList(entities.Select(it => it.FullFileName).ToArray());
                }
            }
            return View(GetViewPath("~/Views/Commodity/Detail.cshtml"), model);
        }
        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual CommodityEntity GetCommodity(long id)
        {
            var query = new QueryInfo();
            query.Query<CommodityEntity>().Where(it => it.Id == id && it.Site.Id == SiteId)
                .Select(it => new object[] { it.Id,it.FileName,it.Password });
            var entities = this.GetEntities<CommodityEntity>(query);
            return entities?.FirstOrDefault();
        }
    }
}
