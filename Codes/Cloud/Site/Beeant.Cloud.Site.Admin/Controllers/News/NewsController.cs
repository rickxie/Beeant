using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Admin.Models.News;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Admin.Controllers.News
{
    [SiteAuthorizeFilter]
    public class NewsController : SiteAuthorizeBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model=new NewsListModel();
       
            return View("~/Views/News/index.cshtml", model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int? page)
        {
            var model = new NewsListModel
            {
                PageIndex = page.HasValue ? page.Value : 0
            };

            model.Newses = GetNewses(model);
            if (model.Newses == null || model.Newses.Count == 0)
                return Content("");
            return View("~/Views/News/_News.cshtml", model);
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [SiteDataFilter(EntityType = typeof(NewsEntity))]
        public virtual ActionResult Get(long id)
        {
            var entity = this.GetEntity<NewsEntity>(id);
            if (entity == null)
                return null;
            var model = new NewsModel
            {
                Id = entity.Id.ToString(),
                Title = entity.Title,
                IsShow = entity.IsShow,
                Content = entity.Content
            };
            return this.Jsonp(model);
        }

        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<NewsEntity> GetNewses(NewsListModel model)
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageSize(model.PageSize).SetPageIndex(model.PageIndex)
                .Query<NewsEntity>().Where(it=>it.Site.Id==SiteId).OrderBy(it=>it.Sequence)
                .Select(it => new object[] {it.Id, it.Title, it.Sequence});
            return this.GetEntities<NewsEntity>(query);
        }

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Add(NewsModel model)
        {
            if (model == null)
                return null;
            var entity = model.CreateEntity(SaveType.Add);
            entity.Sequence = GetSequence();
            var result = new Dictionary<string, object>();
            entity.Site = new SiteEntity { Id = SiteId };
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status",rev);
            result.Add("Id", entity.Id);
            result.Add("Message", mess);
            result.Add("Sequence", entity.Sequence);
            return this.Jsonp(result);
        }
        /// <summary>
        /// D
        /// </summary>
        /// <returns></returns>
        protected virtual int GetSequence()
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageSize(1).Query<NewsEntity>().Where(it=>it.Site.Id==SiteId)
                .OrderBy(it => it.Sequence).Select(it => it.Sequence);
            var entities = this.GetEntities<NewsEntity>(query);
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

        [SiteDataFilter(EntityType = typeof(NewsEntity))]
        [HttpPost]
        public virtual ActionResult Modify(NewsModel model)
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
        [SiteDataFilter(EntityType = typeof(NewsEntity))]
        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<NewsEntity>();
                foreach (var i in id)
                {
                    var info = new NewsEntity
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
