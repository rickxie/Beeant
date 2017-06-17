using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Cloud.Site.Admin.Models.Banner;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Admin.Controllers.Banner
{
    [SiteAuthorizeFilter]
    public class BannerController : SiteAuthorizeBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new BannerListModel();

            return View("~/Views/Banner/index.cshtml", model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int? page)
        {
            var model = new BannerListModel
            {
                PageIndex = page.HasValue ? page.Value : 0
            };
            model.Banners = GetBanners(model);
            if (model.Banners == null || model.Banners.Count == 0)
                return Content("");
            return View("~/Views/Banner/_Banner.cshtml", model);
        }
        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<BannerEntity> GetBanners(BannerListModel model)
        {
            var query = new QueryInfo();
            query.SetPageSize(model.PageSize).SetPageIndex(model.PageIndex).Query<BannerEntity>().Where(it => it.Site.Id == SiteId)
                .OrderBy(it => it.Sequence).Select(it => new object[] { it.Id, it.Url,it.FileName, it.Sequence });
            return this.GetEntities<BannerEntity>(query);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [SiteDataFilter(EntityType = typeof(BannerEntity))]
        public virtual ActionResult Get(long id)
        {
            var entity = this.GetEntity<BannerEntity>(id);
            if (entity == null)
                return null;
            var model = new BannerModel
            {
                Id = entity.Id.ToString(),
                Url = entity.Url,
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
        public virtual ActionResult Add(BannerModel model)
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
            query.SetPageSize(1).Query<BannerEntity>().Where(it => it.Site.Id == SiteId)
                .OrderBy(it => it.Sequence).Select(it => it.Sequence);
            var entities = this.GetEntities<BannerEntity>(query);
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

        [SiteDataFilter(EntityType = typeof(BannerEntity))]
        [HttpPost]
        public virtual ActionResult Modify(BannerModel model)
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
        [SiteDataFilter(EntityType = typeof(BannerEntity))]
        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<BannerEntity>();
                foreach (var i in id)
                {
                    var info = new BannerEntity
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
