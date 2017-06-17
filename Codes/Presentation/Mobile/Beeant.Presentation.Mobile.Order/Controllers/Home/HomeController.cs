using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Order;
using Beeant.Presentation.Mobile.Order.Models.Home;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Mobile.Order.Controllers.Home
{
    [AuthorizeFilter]
    public class HomeController : MobileBaseController
    {
      
        #region 首页
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            var model = new HomeModel
            {
                PayTypes = GetPayTypes()
            };
            return View("Index",model);
        }
        /// <summary>
        /// 付款类型
        /// </summary>
        /// <returns></returns>
        protected virtual IList<PayTypeEntity> GetPayTypes()
        {
            var query = new QueryInfo();
            query.Query<PayTypeEntity>()
                .Where(it => !string.IsNullOrEmpty(it.Url) &&　(it.Tag=="All" || it.Tag=="Mobile"))
                .Select(it => new object[] { it.Name, it.Url, it.Tag });
            var infos= this.GetEntities<PayTypeEntity>(query);
            if (infos != null)
            {
                foreach (var info in infos)
                {
                    info.SetUrl();
                }
            }
            return infos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isAppraisement"></param>
        /// <param name="status"></param>
        /// <param name="endInsertTime"></param>
        /// <param name="page"></param>
        /// <param name="beginInsertTime"></param>
        /// <returns></returns>
        public virtual ActionResult List(bool? isAppraisement, int? status, DateTime? beginInsertTime, DateTime? endInsertTime, int? page)
        {
            var model = new HomeModel
            {
                PageIndex = page.HasValue ? page.Value : 0,
                Status = status,
                BeginInsertTime=beginInsertTime,
                EndInsertTime = endInsertTime,
                IsAppraisement = isAppraisement
            };
            model.Orders = GetOrders(model);
            if (model.Orders == null || model.Orders.Count == 0)
                return Content("");
            return View("_List",model);
        }
  

        #endregion

        #region 确认收货

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Sure(long id)
        {
            var result=new Dictionary<string,object>();
            var order = this.GetEntity<OrderEntity>(id);
            var rev = true;
            var message = "";
            if (order == null || order.Account.Id != Identity.Id || order.Status != OrderStatusType.WaitSign)
            {
                message= "订单不存在";
                rev = false;
            }
            if (rev)
            {
                order.Status = OrderStatusType.Finish;
                order.SaveType = SaveType.Modify;
                order.SetProperty(it => it.Status);
                rev = this.SaveEntity(order);
                message = order.Errors?.FirstOrDefault()?.Message;
            }
            result.Add("Status",rev);
            result.Add("Message",message);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 取消

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Cancel(long id)
        {
            var result = new Dictionary<string, object>();
            var order = this.GetEntity<OrderEntity>(id);
            var rev = true;
            var message = "";
            if (order == null || order.Account.Id != Identity.Id || order.Status != OrderStatusType.WaitPay && order.Status != OrderStatusType.WaitHandle)
            {
                message = "订单不存在";
                rev = false;
            }
            if (rev)
            {
                order.Status = OrderStatusType.Cancel;
                order.SaveType = SaveType.Modify;
                order.SetProperty(it => it.Status);
                rev = this.SaveEntity(order);
                message = order.Errors?.FirstOrDefault()?.Message;
            }
            result.Add("Status", rev);
            result.Add("Message", message);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 等待处理

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Wait(long id)
        {
            var result = new Dictionary<string, object>();
            var order = this.GetEntity<OrderEntity>(id);
            var rev = true;
            var message = "";
            if (order == null || order.Account.Id != Identity.Id || order.Status != OrderStatusType.WaitDelivery || order.PayAmount == 0)
            {
                message = "订单不存在";
                rev = false;
            }
            if (rev)
            {
                order.Status=OrderStatusType.WaitHandle;
                order.SaveType=SaveType.Modify;
                order.SetProperty(it => it.Status);
                rev = this.SaveEntity(order);
                message = order.Errors?.FirstOrDefault()?.Message;
            }
            result.Add("Status", rev);
            result.Add("Message", message);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 详情页
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Detail(long id)
        {
            var model = new HomeModel
            {
               Order=GetOrder(id)
            };
            return View("Detail", model);
        }
        #endregion

        /// <summary>
        /// 得到订单
        /// </summary>
        /// <returns></returns>
        protected virtual IList<OrderEntity> GetOrders(HomeModel model)
        {
            var query = new QueryInfo();
            query.SetPageIndex(model.PageIndex).SetPageSize(model.PageSize).Query<OrderEntity>().OrderByDescending(it=>it.Id)
                .Where(it => it.Account.Id == Identity.Id).Select(it => new object[]
                {
                it.TotalAmount, it.TotalInvoiceAmount, it.Id, it.InsertTime, it.PayAmount,it.Status,
                it.OrderProducts.Select(s => new object[] {s.Product.Id, s.Name, s.Amount, s.Count,s.Price, s.FileName,s.IsAppraisement}),
                it.OrderExpresses.Select(s=>s.Amount)
                });
            if (model.BeginInsertTime.HasValue)
            {
                query.Where(string.Format("{0} && InsertTime>=@BeginInsertTime", query.WhereExp))
                    .SetParameter("BeginInsertTime", model.BeginInsertTime.Value.Date);
            }
            if (model.EndInsertTime.HasValue)
            {
                query.Where(string.Format("{0} && InsertTime>=@EndInsertTime", query.WhereExp))
                    .SetParameter("EndInsertTime", model.EndInsertTime.Value.Date);
            }
            if (model.Status.HasValue)
            {
                query.Where(string.Format("{0} && Status==@Status", query.WhereExp))
                    .SetParameter("Status", model.Status.Value);
            }
            if (model.IsAppraisement.HasValue)
            {
                query.Where(string.Format("{0} && OrderItems.Count(Product.Id!=@ProductId && IsAppraisement==@IsAppraisement)>0", query.WhereExp))
                       .SetParameter("ProductId", 0).SetParameter("IsAppraisement", model.IsAppraisement.Value);
            }
            return this.GetEntities<OrderEntity>(query);
        }

        /// <summary>
        /// 得到订单
        /// </summary>
        /// <returns></returns>
        protected virtual OrderEntity GetOrder(long id)
        {
            var query = new QueryInfo();
            query.Query<OrderEntity>()
                .Where(it =>it.Id==id && it.Account.Id == Identity.Id).Select(it => new object[]
                {
                it.TotalAmount, it.TotalInvoiceAmount, it.Id, it.InsertTime, it.PayAmount,it.PayTypes,it.Status,
                it.OrderProducts.Select(s => new object[] {s.Product.Id, s.Name,s.Price, s.Amount, s.Count, s.FileName}),
                it.OrderExpresses.Select(s=>new object[] {s.Number,s.Name,s.Address,s.Email,s.Mobile,s.Postcode,s.Recipient}),
                it.OrderComplaints.Select(s=>new object[] {s.Question,s.Question,s.InsertTime,s.AnswerTime,s.IsReply,s.Type})
                });
            return this.GetEntities<OrderEntity>(query)?.FirstOrDefault();
        }

    }
}
