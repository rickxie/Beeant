using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Component.Extension;
using Dependent;
using Beeant.Application.Services.Finance;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Distributed.Outside.Pay.Models;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Order;
using Configuration;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Distributed.Outside.Pay.Controllers
{
    public class PayController : BaseController
    {

        /// <summary>
        /// 得到App
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual IPaylineApplicationService GetPaylineApp(PaylineType type)
        {
            return Ioc.Resolve<IPaylineApplicationService>(string.Format("Beeant.Application.Services.Finance.I{0}PaylineApplicationService", type));
        }


        #region 创建

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="model"></param>
        /// <param name="paylineType"></param>
        /// <returns></returns>
        protected virtual ActionResult Create(PaylineModel model,PaylineType paylineType)
        {
            var info = CreatePayline(model, paylineType);
            if (string.IsNullOrEmpty(info.Request))
                return Process(info);
            if (info.Errors != null && info.Errors.Count > 0)
                return PayError(info);
            var request = info.Request.ToLower();
            if (request.StartsWith("http://") || request.StartsWith("https://"))
                return Redirect(info.Request);
            if (!string.IsNullOrEmpty(Request["createhandle"]))
            {
                var builder = new StringBuilder();
                builder.AppendFormat("<script  type=\"text/javascript\" >document.domain='{0}';{1}('{2}');</script>",
                    ConfigurationManager.GetSetting<string>("Domain"), Request["createhandle"], info.Id);
                builder.Append(info.Request);
                return Content(builder.ToString());
            }
            return Content(info.Request);
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="model"></param>
        /// <param name="paylineType"></param>
        /// <returns></returns>

        protected virtual PaylineEntity CreatePayline(PaylineModel model, PaylineType paylineType)
        {
            var info = new PaylineEntity
            {
                Account = new AccountEntity { Id = Identity.Id },
                ChannelType = model.ChannelType,
                Status = PaylineStatusType.Create,
                Remark = "",
                Type = paylineType,
                SaveType = SaveType.Add,
                PaylineItems = new List<PaylineItemEntity>(),
                Forms = model.Forms??new Dictionary<string, string>()
            };
            var orders = GetOrders(model);
            FillPaylineItems(info, orders);
            if (model.Amount.HasValue)
            {
                info.Amount = model.Amount.Value;
            }
            GetPaylineApp(paylineType).Create(info);
            return info;
        }
        /// <summary>
        /// 填充明细
        /// </summary>
        /// <param name="payline"></param>
        /// <param name="orders"></param>
        protected virtual void FillPaylineItems(PaylineEntity payline, IList<OrderEntity> orders)
        {
            if (orders == null) return;
            foreach (var order in orders)
            {
                var paylineItem = new PaylineItemEntity
                {
                    Order = order,
                    Payline = payline,
                    Amount= order.PayAmount == 0 && order.Deposit > 0 ? order.Deposit: order.TotalPayAmount - order.PayAmount,
                    SaveType = SaveType.Add
                };
                payline.PaylineItems.Add(paylineItem);
            }
        }

        /// <summary>
        /// 得到金额
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual IList<OrderEntity> GetOrders(PaylineModel model)
        {
            if (string.IsNullOrEmpty(model.OrderIds))
                return null;
            var orderIds= model.OrderIds.Split(',').Select(s => s.Convert<long>()).ToList();
            var query=new QueryInfo();
            query.Query<OrderEntity>()
                .Where(it => orderIds.Contains(it.Id) &&　it.Account.Id== Identity.Id)
                .Select(it => new object[] {it.Id,it.TotalAmount, it.TotalInvoiceAmount,it.TotalPayAmount, it.PayAmount,it.Deposit});
            var orders = this.GetEntities<OrderEntity>(query);
            return orders;
        }
        #endregion

        #region 回调

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="paylineType"></param>
        /// <returns></returns>
        protected virtual ActionResult Process(PaylineType paylineType)
        {
            var info = GetPaylineApp(paylineType).Process();
            return Process(info);
        }
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual ActionResult Process(PaylineEntity info)
        {
            if (info == null || info.Status!= PaylineStatusType.Success)
            {
                return PayError(info);
            }
            var url = string.Format("{0}/Home/Index", info.ChannelType == ChannelType.Website ? this.GetUrl("PresentationWebsiteOrderUrl") : this.GetUrl("PresentationMobileOrderUrl"));
            if (!string.IsNullOrEmpty(Request["successhandle"]))
            {
                return Content(string.Format("<script  type=\"text/javascript\" >document.domain='{0}';{1}('{2}');</script>", ConfigurationManager.GetSetting<string>("Domain"), Request["success"],info.Id));
            }
            return new RedirectResult(url);
        }

        /// <summary>
        /// 支付失败
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual ActionResult PayError(PaylineEntity info)
        {
            var message = info?.Errors?.FirstOrDefault()?.Message;
            message = string.IsNullOrWhiteSpace(message) ? "支付失败" : message;
            if (!string.IsNullOrEmpty(Request["failhandle"]))
            {
           
                return Content(string.Format("<script  type=\"text/javascript\" >document.domain='{0}';{1}('{2}');</script>", ConfigurationManager.GetSetting<string>("Domain"), Request["failhandle"], message));
            }
            var url = string.Format("{0}/Home/Index",HttpContext.Request.Browser.IsMobileDevice ? this.GetUrl("PresentationWebsiteOrderUrl") : this.GetUrl("PresentationMobileOrderUrl"));
            return Content(string.Format("<script  type=\"text/javascript\" >alert('{0}');window.location.href='{1}'</script>",message, url));
        }

        #endregion

        #region 退款

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="model"></param>
        /// <param name="paylineType"></param>
        /// <returns></returns>
        protected virtual ActionResult Refund(PaylineModel model, PaylineType paylineType)
        {
            var info = CreateRefund(model, paylineType);
            var request = info.Request.ToLower();
            if (request.StartsWith("http://") || request.StartsWith("https://"))
                return Redirect(info.Request);
            if (!string.IsNullOrEmpty(Request["createhandle"]))
            {
                var builder = new StringBuilder();
                builder.AppendFormat("<script  type=\"text/javascript\" >document.domain='{0}';{1}('{2}');</script>",
                    ConfigurationManager.GetSetting<string>("Domain"), Request["createhandle"], info.Id);
                builder.Append(info.Request);
                return Content(builder.ToString());
            }
            var url = string.Format("{0}/Home/Index", info.ChannelType == ChannelType.Website ? this.GetUrl("PresentationWebsiteOrderUrl") : this.GetUrl("PresentationMobileOrderUrl"));
            return new RedirectResult(url);
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="model"></param>
        /// <param name="paylineType"></param>
        /// <returns></returns>

        protected virtual PaylineEntity CreateRefund(PaylineModel model, PaylineType paylineType)
        {
            var info = new PaylineEntity
            {
                Account = new AccountEntity { Id = Identity.Id },
                ChannelType = model.ChannelType,
                Status = PaylineStatusType.Create,
                Remark = "",
                Type = paylineType,
                SaveType = SaveType.Add,
                PaylineItems = new List<PaylineItemEntity>(),
                Forms = model.Forms ?? new Dictionary<string, string>()
            };
            var orderPays = GetOrderPays(model);
            FillRefundPaylineItems(info, orderPays);
            info.Amount = info.PaylineItems.Sum(it => it.Amount);
            GetPaylineApp(paylineType).Refund(info);
            return info;
        }
        /// <summary>
        /// 填充明细
        /// </summary>
        /// <param name="payline"></param>
        /// <param name="orderPays"></param>
        protected virtual void FillRefundPaylineItems(PaylineEntity payline, IList<OrderPayEntity> orderPays)
        {
            if (orderPays == null) return;
            foreach (var orderPay in orderPays)
            {
                if(orderPay.Order==null || orderPay.Amount<=0 || orderPay.Order.TotalPayAmount - orderPay.Order.PayAmount>=0)
                    continue;
                var paylineItem = new PaylineItemEntity
                {
                    Order = orderPay.Order,
                    Payline = payline,
                    Amount = orderPay.Order.TotalPayAmount- orderPay.Order.PayAmount,
                    SaveType = SaveType.Add
                };
                payline.PaylineItems.Add(paylineItem);
                payline.OutNumber = orderPay.Number;
            }
        }
        /// <summary>
        /// 得到金额
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual IList<OrderPayEntity> GetOrderPays(PaylineModel model)
        {
            if (string.IsNullOrEmpty(model.OrderPayId))
                return null;
            var query = new QueryInfo();
            query.Query<OrderPayEntity>()
                .Where(it => it.Id== model.OrderPayId.Convert<long>() && it.Order.Account.Id == Identity.Id)
                .Select(it => new object[] {it.Amount, it.Order.Id,it.Order.TotalPayAmount,it.Order.PayAmount,it.Number});
            var orders = this.GetEntities<OrderPayEntity>(query);
            return orders;
        }
        #endregion

    }
}
