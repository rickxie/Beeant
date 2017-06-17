using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Mobile.Models.Qrcode;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Mobile.Controllers.Qrcode
{
    public class QrcodeController : WebSiteBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            
            return View(GetViewPath("~/Views/Qrcode/index.cshtml"));
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string type, string key, int? page)
        {
            if (string.IsNullOrEmpty(type) || type.ToLower().Equals("commodity"))
                return Commodity(key, page);
            return Catalog(key, page);
          
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Commodity(string key,int? page)
        {
            var model = new QrcodeListModel
            {
                Key = key,
                PageIndex = page.HasValue ? page.Value : 0
            };
            model.Commodities = GetCommodities(model);
            return View(GetViewPath("~/Views/Qrcode/_Commodity.cshtml"), model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Catalog(string key, int? page)
        {
            var model = new QrcodeListModel
            {
                Key = key,
                PageIndex = page.HasValue ? page.Value : 0
            };
            model.Catalogs = GetCatalogs(model);
            return View(GetViewPath("~/Views/Qrcode/_Catalog.cshtml"), model);
        }
        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CatalogEntity> GetCatalogs(QrcodeListModel model)
        {
            var query = new QueryInfo();
            query.SetPageIndex(model.PageIndex).SetPageSize(model.PageSize).Query<CatalogEntity>().Where(it=>it.Site.Id==SiteId).Select(it => new object[] { it.Id, it.Name,it.FileName });
            if (!string.IsNullOrEmpty(model.Key))
            {
                query.Where(string.Format("{0} && Name.Contains(@Name)", query.WhereExp))
                    .SetParameter("Name", model.Key);
            }
            return this.GetEntities<CatalogEntity>(query);
        }
        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CommodityEntity> GetCommodities(QrcodeListModel model)
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageIndex(model.PageIndex).SetPageSize(model.PageSize).Query<CommodityEntity>()
                .Where(it => it.Site.Id == SiteId && it.Status == CommodityStatusType.Normal)
                .Select(it => new object[] {it.Id,it.Name, it.FileName});
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
        public virtual ActionResult CatalogQrCode(long id)
        {
            var url = string.Format("{0}/Commodity/Index/{1}?catalogId={2}", this.GetUrl("CloudSiteMobileUrl"), SiteId, id);
            var bs = QrEncodHelper.Create(url);
            return File(bs, "image/png");
        }
        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult CommodityQrCode(long id)
        {
            var url = string.Format("{0}/Commodity/Detail/{1}?ps[0].Id={2}", this.GetUrl("CloudSiteMobileUrl"), SiteId, id);
            var bs = QrEncodHelper.Create(url);
            return File(bs, "image/png");
        }
    }
}
