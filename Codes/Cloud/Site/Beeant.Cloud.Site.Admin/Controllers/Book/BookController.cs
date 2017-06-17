using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Dependent;
using Beeant.Application.Services.Site;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Admin.Models.Book;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Admin.Controllers.Book
{
    [SitePrinterAuthorizeFilter]
    public class BookController : SiteAuthorizeBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
    
        public ActionResult Index()
        {
            var model=new BookModel();
            model.Company = GetCompany();
            return View("~/Views/Book/index.cshtml", model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>

        public ActionResult List(int? page)
        {
            var model = new BookModel {PageIndex=page.HasValue?page.Value:0};
            model.Albums = GetAlbums(model);
            return View("~/Views/Book/_Album.cshtml", model);
        }
        /// <summary>
        /// 封面
        /// </summary>
        /// <returns></returns>

        public ActionResult CreateFrontCover(long albumId)
        {
            var album = this.GetEntity<AlbumEntity>(albumId);
            var model = new BookModel
            {
                Album = album,
                Site = this.GetEntity<SiteEntity>(SiteId),
                Company = GetCompany()
            };
            return View(GetViewPath("~/Views/Book/FrontCover.cshtml", album==null?"": album.Path), model);
        }
        /// <summary>
        /// 关于我们
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateAbout(long albumId)
        {
            var album = this.GetEntity<AlbumEntity>(albumId);
            var model = new BookModel
            {
                Album = album,
                Site = this.GetEntity<SiteEntity>(SiteId),
                Company = GetCompany()
            };
            return View(GetViewPath("~/Views/Book/About.cshtml", album == null ? "" : album.Path), model);
        }
  
        /// <summary>
        /// 背面
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateBackCover(long albumId)
        {
            var album = this.GetEntity<AlbumEntity>(albumId);
            var model = new BookModel
            {
                Album = album,
                Site = this.GetEntity<SiteEntity>(SiteId),
                Company = GetCompany()
            };
            return View(GetViewPath("~/Views/Book/BackCover.cshtml", album == null ? "" : album.Path), model);
        }
 
        /// <summary>
        /// 类目
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateCommodity(long albumId, long catalogId,string ids)
        {
            var album = this.GetEntity<AlbumEntity>(albumId);
            var model = new BookModel ();
            model.Catalog = catalogId == 0 ? null: GetCatalog(catalogId);
            model.Commodities = GetCommodities(ids.Split(',').Select(it => it.Convert<long>()).ToArray());
            return View(GetViewPath("~/Views/Book/Commodity.cshtml", album == null ? "" : album.Path), model);
        }
        /// <summary>
        /// 得到公司信息
        /// </summary>
        /// <returns></returns>
        protected virtual CompanyEntity GetCompany()
        {
            var query = new QueryInfo();
            query.Query<CompanyEntity>().Where(it => it.Site.Id == SiteId);
            return this.GetEntities<CompanyEntity>(query)?.FirstOrDefault();
        }
        /// <summary>
        /// 得到公司信息
        /// </summary>
        /// <returns></returns>
        protected virtual CatalogEntity GetCatalog(long id)
        {
            var query = new QueryInfo();
            query.Query<CatalogEntity>().Where(it => it.Site.Id == SiteId && it.Id==id);
            return this.GetEntities<CatalogEntity>(query)?.FirstOrDefault();
        }

        /// <summary>
        /// 得到商品
        /// </summary>
        /// <returns></returns>
        protected virtual IList<AlbumEntity> GetAlbums(BookModel model)
        {
            var query = new QueryInfo {IsReturnCount=false};
            query.SetPageSize(model.PageSize).SetPageIndex(model.PageIndex).Query<AlbumEntity>()
                .Select(it => new object[] { it.Id, it.Name,it.Detail});
            var infos= this.GetEntities<AlbumEntity>(query);
            return infos;
        }
  
        /// <summary>
        /// 得到商品
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CommodityEntity> GetCommodities(long[] ids)
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.Query<CommodityEntity>()
                .OrderBy(it => it.Sequence)
                .Where(it =>it.Site.Id==SiteId && ids.Contains(it.Id))
                .Select(it => new object[] { it.Id, it.Name, it.AlbumFileName,it.FileName });
            return this.GetEntities<CommodityEntity>(query);
        }
        /// <summary>
        /// 目录
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CatalogEntity> GetCatalogs()
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.Query<CatalogEntity>()
                .Where(it => it.Site.Id == SiteId)
                .Select(it => new object[] { it.Id,it.Name});
            return this.GetEntities<CatalogEntity>(query);
        }
        /// <summary>
        /// 得到mull
        /// </summary>
        /// <returns></returns>
        protected virtual int GetCommodityCount(long catalogId)
        {
            var query = new QueryInfo { };
            query.SetPageSize(1).Query<CommodityEntity>()
                .Where(it => it.Site.Id == SiteId && it.Catalog.Id== catalogId && it.IsCreateAlbum)
                .Select(it => new object[] { it.Id });
             this.GetEntities<CommodityEntity>(query);
            return query.DataCount;
        }
        #region 生成
        /// <summary>
        /// 创建 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Check(long albumId)
        {
            if (!ViewBag.Site.IsPrint)
                return null;
            var album = this.GetEntity<AlbumEntity>(albumId);
            if (album == null || album.PageSize<=0)
                return null;
            var catalogs = GetCatalogs();
            if (catalogs == null)
                return null;
            var mess = new List<string>();
            foreach (var catalog in catalogs)
            {
                var count = GetCommodityCount(catalog.Id);
                if (count%album.PageSize != 0)
                    mess.Add(string.Format("{0}生成产品数量为{1}", catalog.Name, count));
            }
            return Content(string.Join(",", mess.ToArray()));
        }

        /// <summary>
        /// 创建 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Create(long albumId)
        {
            if (!ViewBag.Site.IsPrint)
                return null;
            var rev = Ioc.Resolve<ICreateAlbumApplicationService>().Add(SiteId, albumId);
            var reuslt = new Dictionary<string, object>
            {
                {"Status",rev }
            };
            return this.Jsonp(reuslt);
        }
      
        #endregion
    }
}
