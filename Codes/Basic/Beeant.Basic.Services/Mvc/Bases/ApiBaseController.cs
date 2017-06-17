using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Dependent;
using Beeant.Application.Services;
using Beeant.Application.Services.Utility;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Api;
using Beeant.Domain.Entities.Log;
using Component.Extension;
using Winner;
using Winner.Filter;
using Winner.Persistence;

namespace Beeant.Basic.Services.Mvc.Bases
{
    public class ApiBaseController : Controller
    {
        /// <summary>
        /// 票据
        /// </summary>
        public string TicketName { get; set; } = "ticket";
        private IdentityEntity _identity;
        /// <summary>
        /// 身份验证
        /// </summary>
        public virtual IdentityEntity Identity
        {
            get
            {
                var ticket = RouteData.Values[TicketName] ?? HttpContext.Request[TicketName];
                if (ticket == null)
                    return null;
                if(_identity==null)
                    _identity= Ioc.Resolve<IIdentityApplicationService>().Get<IdentityEntity>(ticket.ToString());
                return _identity;
            }
        }

 
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            if (IsAddlog)
            {
              AddLog(filterContext);
            }
        }

        /// <summary>
        /// 得到输出结果
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected virtual string GetResponse(ResultExecutedContext filterContext)
        {
            if (filterContext.Result is ContentResult)
            {
                return ((ContentResult) filterContext.Result).Content;
            }
            if (filterContext.Result is JsonResult)
            {
                return ((JsonResult) filterContext.Result).Data.SerializeJson();
            }
            return "";
        }
        /// <summary>
        /// 得到值
        /// </summary>
        /// <returns></returns>
        protected virtual string GetRequest(ResultExecutedContext filterContext)
        {
            var builder = new StringBuilder();
            foreach (var key in filterContext.RequestContext.HttpContext.Request.QueryString.AllKeys)
            {
                if (filterContext.RequestContext.HttpContext.Request[key] == null) continue;
                builder.AppendFormat("{0}={1}", key, filterContext.RequestContext.HttpContext.Request[key]);
                if (builder.Length > 0)
                    builder.Append("&");
            }
            foreach (var key in filterContext.RequestContext.HttpContext.Request.Form.AllKeys)
            {
                if (filterContext.RequestContext.HttpContext.Request[key] == null)
                    continue;
                builder.AppendFormat("{0}={1}", key, filterContext.RequestContext.HttpContext.Request[key]);
                if (builder.Length > 0)
                    builder.Append("&");
            }
            if (builder.Length > 0)
                builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        /// <summary>
        /// 是否添加日志
        /// </summary>
        protected virtual bool IsAddlog
        {
            get
            {
                return Verification != null && Verification.IsLog;
            }
        }
        /// <summary>
        /// 是否添加日志
        /// </summary>
        protected virtual VerificationEntity Verification
        {
            get
            {
               return ViewBag.Verification;
            }
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        public virtual void AddLog(ResultExecutedContext filterContext)
        {
            VerificationEntity verification = filterContext.Controller.ViewBag.Verification;
            string method = filterContext.Controller.ViewBag.Method;
            var accountId = verification == null || verification.Voucher == null || verification.Voucher.Account == null
                ? 0
                : verification.Voucher.Account.Id;
            string key = filterContext.Controller.ViewBag.TraceKey;
            var info = new ApiTraceEntity
            {
                Method = method,
                Key= key,
                Request = GetRequest(filterContext),
                Response = GetResponse(filterContext),
                Ip = HttpContextHelper.GetClientIp(),
                Account = new AccountEntity {Id = accountId},
                SaveType = SaveType.Add
            };
            Ioc.Resolve<IApplicationService, ApiTraceEntity>().Save(info);
        }

        /// <summary>
        /// 返回错误结构
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual ActionResult ReturnResult(string code, string message)
        {
            var result = new Dictionary<string, string> { { "Code", code }, { "Message", message } };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 返回错误结构
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual ActionResult ReturnSuccessResult(string message)
        {
            var result = new Dictionary<string, string> { { "Code", "00000" }, { "Message", message } };
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 返回错误结构
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual ActionResult ReturnFailureResult(string message)
        {
            var result = new Dictionary<string, string> { { "Code", "00001" }, { "Message", message } };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 返回错误结构
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected virtual ActionResult ReturnExceptionResult(Exception ex)
        {
            var error = Creator.Get<IValidation>().GetErrorInfo(typeof (VoucherProtocolEntity).FullName, "99999");
            var result = new Dictionary<string, string> {{"Code", "99999" }, {"Message", error==null? "99999" : error.Message}};
            AddErrorEntity(ex);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加错误信息
        /// </summary>
        protected virtual void AddErrorEntity(Exception ex)
        {
            var info = new Domain.Entities.Log.ErrorEntity
            {
                Address = Request.Url.ToString(),
                Device = Request.UserAgent,
                Ip = Request.UserHostAddress,
                SaveType = SaveType.Add
            };
            info.SetEntity(ex);
            Ioc.Resolve<IApplicationService, ErrorInfo>().Save(info); 
        }
       
    }
}
