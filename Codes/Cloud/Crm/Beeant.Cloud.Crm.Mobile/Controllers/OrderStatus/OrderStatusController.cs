﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Crm.Mobile.Models.OrderStatus;
using Beeant.Domain.Entities.Crm;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Crm.Mobile.Controllers.OrderStatus
{
    [CrmAdminAuthorizeFilterAttribute]
    public class OrderStatusController : CrmAuthorizeBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new OrderStatusListModel();

            return View("~/Views/OrderStatus/index.cshtml", model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int? page)
        {
            var model = new OrderStatusListModel
            {
                PageIndex = page.HasValue ? page.Value : 0
            };

            model.OrderStatuses = GetOrderStatuses(model);
            if (model.OrderStatuses == null || model.OrderStatuses.Count == 0)
                return Content("");
            return View("~/Views/OrderStatus/_OrderStatus.cshtml", model);
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [CrmDataFilter(EntityType = typeof(OrderStatusEntity))]
        public virtual ActionResult Get(long id)
        {
            var entity = this.GetEntity<OrderStatusEntity>(id);
            if (entity == null)
                return null;
            var model = new OrderStatusModel
            {
                Id = entity.Id.ToString(),
                Name = entity.Name
            };
            return this.Jsonp(model);
        }

        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<OrderStatusEntity> GetOrderStatuses(OrderStatusListModel model)
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageSize(model.PageSize).SetPageIndex(model.PageIndex)
                .Query<OrderStatusEntity>().Where(it => it.Crm.Id == CrmId).OrderBy(it => it.Sequence)
                .Select(it => new object[] { it.Id, it.Name, it.Sequence });
            return this.GetEntities<OrderStatusEntity>(query);
        }

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Add(OrderStatusModel model)
        {
            if (model == null)
                return null;
            var entity = model.CreateEntity(SaveType.Add);
            entity.Sequence = GetSequence();
            var result = new Dictionary<string, object>();
            entity.Crm = new CrmEntity { Id = CrmId };
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status", rev);
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
            query.SetPageSize(1).Query<OrderStatusEntity>().Where(it => it.Crm.Id == CrmId)
                .OrderBy(it => it.Sequence).Select(it => it.Sequence);
            var entities = this.GetEntities<OrderStatusEntity>(query);
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

        [CrmDataFilter(EntityType = typeof(OrderStatusEntity))]
        [HttpPost]
        public virtual ActionResult Modify(OrderStatusModel model)
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
        [CrmDataFilter(EntityType = typeof(OrderStatusEntity))]
        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<OrderStatusEntity>();
                foreach (var i in id)
                {
                    var info = new OrderStatusEntity
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
