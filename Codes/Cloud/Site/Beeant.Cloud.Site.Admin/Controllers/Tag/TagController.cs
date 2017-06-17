using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Cloud.Site.Admin.Models.Tag;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Admin.Controllers.Tag
{
    [SiteAuthorizeFilter]
    public class TagController : SiteAuthorizeBaseController 
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new TagListModel();

            return View("~/Views/Tag/index.cshtml", model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int? page)
        {
            var model = new TagListModel
            {
                PageIndex = page.HasValue ? page.Value : 0
            };
            model.Tags = GetTags(model);
            if (model.Tags == null || model.Tags.Count == 0)
                return Content("");
            return View("~/Views/Tag/_Tag.cshtml", model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult All()
        {
 
            var result = new List<Dictionary<string, object>> ();
            var tags = GetTags(new TagListModel {PageIndex=0});
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    var value = new Dictionary<string, object> {{"Id", tag.Id}, {"Name", tag.Name}};
                    result.Add(value);
                }
            }
            return this.Jsonp(result);
        }

        /// <summary>
        /// 得到标签
        /// </summary>
        /// <returns></returns>
        protected virtual IList<TagEntity> GetTags(TagListModel model)
        {
            var query = new QueryInfo();
            query.SetPageSize(model.PageSize).SetPageIndex(model.PageIndex).Query<TagEntity>().Where(it => it.Site.Id == SiteId)
                .OrderBy(it => it.Sequence).Select(it => new object[] { it.Id, it.Name, it.Sequence });
            return this.GetEntities<TagEntity>(query);
        }

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Add(TagModel model)
        {
            if (model == null)
                return null;
            var entity = model.CreateEntity(SaveType.Add);
            var result = new Dictionary<string, object>();
            entity.Site = new SiteEntity { Id =SiteId };
            entity.Sequence = GetSequence();
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status",rev);
            result.Add("Id", entity.Id);
            result.Add("Message", mess);
            return this.Jsonp(result);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Adds(string[] names)
        {
            if (names == null || names.Length==0)
                return null;
            var sequence = GetSequence();
            var result=new Dictionary<string,object>();
            IList<TagEntity> entites=new List<TagEntity>();
            foreach (var name in names)
            {
                var entity=new TagEntity();
                entity.Name = name;
                entity.SaveType=SaveType.Add;
                entity.Sequence = sequence;
                entity.Site = new SiteEntity { Id = SiteId };
                if (entity.Sequence + 5000 > 100000000)
                    sequence= 100000000;
                else
                    sequence = sequence+ 5000;
                entites.Add(entity);
            }
            var rev = this.SaveEntities(entites);
            var mess = rev ? "" : "保存失败";
            result.Add("Status", rev);
            result.Add("Ids", entites.Select(it=>it.Id).ToArray());
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
            query.SetPageSize(1).Query<TagEntity>().Where(it => it.Site.Id == SiteId)
                .OrderBy(it => it.Sequence).Select(it => it.Sequence);
            var entities = this.GetEntities<TagEntity>(query);
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

        [SiteDataFilter(EntityType = typeof(TagEntity))]
        [HttpPost]
        public virtual ActionResult Modify(TagModel model)
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
        [SiteDataFilter(EntityType = typeof(TagEntity))]
        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<TagEntity>();
                foreach (var i in id)
                {
                    var info = new TagEntity
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
