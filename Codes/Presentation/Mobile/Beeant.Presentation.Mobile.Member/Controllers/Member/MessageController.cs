using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Member;
using Beeant.Presentation.Mobile.Member.Models.Member;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Mobile.Member.Controllers.Member
{
    [AuthorizeFilter]
    public class MessageController : MobileBaseController
    {

        #region 首页
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            var model = new MessageModel();
            return View("~/Views/Member/Message/Index.cshtml", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public virtual ActionResult List(int? page)
        {
            var model = new MessageModel
            {
                PageIndex = page.HasValue ? page.Value : 0
            };
            model.Messages = GetMessages(model);
            if (model.Messages == null || model.Messages.Count == 0)
                return Content("");
            return View("~/Views/Member/Message/_List.cshtml", model);
        }
        /// <summary>
        /// 得到会员地址信息
        /// </summary>
        /// <returns></returns>
        protected virtual IList<MessageEntity> GetMessages(MessageModel model)
        {
            var query = new QueryInfo();
            query.SetPageSize(model.PageSize).SetPageIndex(model.PageIndex).Query<MessageEntity>().OrderByDescending(it=>it.Id)
                 .Select(it => new object[] { it.Id,it.Title,it.IsRead});
            return this.GetEntitiesByIdentity<MessageEntity>(query);

        }

        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DataFilter(EntityType = typeof(MessageEntity))]
        public virtual ActionResult Remove(long id)
        {
            var entity = new MessageEntity
            {
                Id = id,
                SaveType=SaveType.Remove
            };
           var rev= this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            var result=new Dictionary<string,object>
            {
                {"Status",rev },
                {"Message",mess }
            };
            return this.Jsonp(result);
        }
        #endregion

        #region 详情

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult Detail(long id)
        {
            var model = new MessageModel
            {
                Message = this.GetEntityByIdentity<MessageEntity>(id)
            };
            if (model.Message != null)
            {
                Action<long> action = UpdateRead;
                action.BeginInvoke(id, null, null);
            }
            return View("~/Views/Member/Message/Detail.cshtml", model);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="id"></param>
        protected virtual void UpdateRead(long id)
        {
            var entity = new MessageEntity
            {
                Id = id,
                IsRead = true,
                SaveType = SaveType.Modify
            };
            entity.SetProperty(it => it.IsRead);
            var rev = this.SaveEntity(entity);
        }
        #endregion

    }
}
