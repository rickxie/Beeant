using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Cloud.Site.Admin.Models.Message;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Admin.Controllers.Message
{
    [SiteAuthorizeFilter]
    public class MessageController : SiteAuthorizeBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var site = this.GetEntity<SiteEntity>(SiteId);
            if (site == null || string.IsNullOrEmpty(site.WechatAppId) || !site.IsOpenMessage)
            {
                return Content("您尚未开通客服消息机器功能");
            }
            var model = new MessageListModel();
            return View("~/Views/Message/index.cshtml", model);
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int? page)
        {
            var model = new MessageListModel
            {
                PageIndex = page.HasValue ? page.Value : 0
            };

            model.Messages = GetMessagees(model);
            if (model.Messages == null || model.Messages.Count == 0)
                return Content("");
            return View("~/Views/Message/_Message.cshtml", model);
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [SiteDataFilter(EntityType = typeof (MessageEntity))]
        public virtual ActionResult Get(long id)
        {
            var entity = this.GetEntity<MessageEntity>(id);
            if (entity == null)
                return null;
            var model = new MessageModel
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
                Content = entity.Content
            };
            return this.Jsonp(model);
        }

        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<MessageEntity> GetMessagees(MessageListModel model)
        {
            var query = new QueryInfo {IsReturnCount = false};
            query.SetPageSize(model.PageSize).SetPageIndex(model.PageIndex)
                .Query<MessageEntity>().Where(it => it.Site.Id == SiteId && it.Type=="text")
                .Select(it => new object[] {it.Id, it.Name});
            return this.GetEntities<MessageEntity>(query);
        }

        #region 添加

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Add(MessageModel model)
        {
            if (model == null)
                return null;
            var entity = model.CreateEntity(SaveType.Add);
            entity.Type = "text";
            var result = new Dictionary<string, object>();
            entity.Site = new SiteEntity {Id = SiteId};
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status", rev);
            result.Add("Id", entity.Id);
            result.Add("Message", mess);
            return this.Jsonp(result);
        }

        #endregion

        #region 修改

        [SiteDataFilter(EntityType = typeof (MessageEntity))]
        [HttpPost]
        public virtual ActionResult Modify(MessageModel model)
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

        [SiteDataFilter(EntityType = typeof (MessageEntity))]
        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<MessageEntity>();
                foreach (var i in id)
                {
                    var info = new MessageEntity
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

        #region 关注消息
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Subscribe()
        {
            var site = this.GetEntity<SiteEntity>(SiteId);
            if (site == null || string.IsNullOrEmpty(site.WechatAppId) || !site.IsOpenMessage)
            {
                return Content("您尚未开通客服消息机器功能");
            }
            var model = new MessageModel();
            model.Message = GetMessage();
            return View("~/Views/Message/Subscribe.cshtml", model);
        }
        /// <summary>
        /// 得到公司信息
        /// </summary>
        /// <returns></returns>
        protected virtual MessageEntity GetMessage()
        {
            var query = new QueryInfo();
            query.Query<MessageEntity>().Where(it => it.Site.Id == SiteId);
            var entities = this.GetEntities<MessageEntity>(query);
            return entities?.FirstOrDefault();
        }


        #region 保存
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult SaveSubscribe(MessageModel model)
        {
            if (model == null)
                return null;
            var message = GetMessage();
            MessageEntity entity = null;
            if (message == null)
            {
                entity = model.CreateEntity(SaveType.Add);
                entity.Type = "subscribe";
                entity.Name = "欢迎词";
                entity.Site = new SiteEntity { Id = SiteId };
            }
            else
            {
                entity = model.CreateEntity(SaveType.Modify);
                entity.Id = message.Id;
            }
            var result = new Dictionary<string, object>();
            entity.Site = new SiteEntity { Id = SiteId };
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status", rev);
            result.Add("Message", mess);
            return this.Jsonp(result);
        }
        #endregion
        #endregion
    }
}
