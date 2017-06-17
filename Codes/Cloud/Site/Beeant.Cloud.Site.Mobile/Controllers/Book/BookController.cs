using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Mobile.Models.Book;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Mobile.Controllers.Book
{
    public class BookController : MobileSiteBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new BookListModel();
            model.Company = GetCompany();
            return View(GetViewPath("~/Views/Book/index.cshtml"),model);
        }

        protected virtual CompanyEntity GetCompany()
        {
            var query = new QueryInfo();
            query.Query<CompanyEntity>().Where(it => it.Site.Id == SiteId).Select(it =>new object[] { it.Album.MusicUrl ,it.Album.Width,it.Album.Height} );
            return this.GetEntities<CompanyEntity>(query)?.FirstOrDefault();
             
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var model = new BookListModel();
            model.Books = GetBooks(model);
            model.Company = GetCompany();
            model.SetSize();
            if (model.Books == null || model.Books.Count == 0)
                return Content("");
            return View(GetViewPath("~/Views/Book/_Book.cshtml"), model);
        }

        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<BookEntity> GetBooks(BookListModel model)
        {
            var query = new QueryInfo();
            query.Query<BookEntity>().Where(it => it.Site.Id == SiteId && it.IsUsed)
                .OrderBy(it => it.Id).Select(it => new object[] { it.Id,it.FileName });
            return this.GetEntities<BookEntity>(query);
        }

    }
}
