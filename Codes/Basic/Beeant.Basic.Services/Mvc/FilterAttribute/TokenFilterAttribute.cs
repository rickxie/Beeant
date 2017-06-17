using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Component.Extension;
using Dependent;
using Beeant.Application.Services.Api;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Domain.Entities.Api;

namespace Beeant.Basic.Services.Mvc.FilterAttribute
{
    public class TokenFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 方法
        /// </summary>
        public virtual string Method { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Verify(filterContext);
            base.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual bool Verify(ActionExecutingContext filterContext)
        {
            var value = GetValue(filterContext);
            var sign = filterContext.RequestContext.RouteData.Values["sign"] ??
                   filterContext.RequestContext.HttpContext.Request["sign"];
            var token = filterContext.RequestContext.RouteData.Values["token"] ??
               filterContext.RequestContext.HttpContext.Request["token"];
            var args = new ApiArgsEntity
            {
                Ip = HttpContextHelper.GetClientIp(),
                Method = Method,
                Sign = sign.Convert<string>(),
                Token = token.Convert<string>(),
                Value = value
            };
            var info = Ioc.Resolve<IApiEngineApplicationService>().Verify(args);
            filterContext.Controller.ViewBag.Verification = info;
            filterContext.Controller.ViewBag.Method = Method;
            if (info != null && info.Error != null)
            {
                filterContext.Result = new ContentResult {Content = GetErrorResult(info.Error.Key, info.Error.Message)};
            }
            return info != null && info.IsPass;
        }
        /// <summary>
        /// 得到值
        /// </summary>
        /// <returns></returns>
        protected virtual string GetValue(ActionExecutingContext filterContext)
        {
            var builder = new StringBuilder();
            foreach (var key in filterContext.RequestContext.HttpContext.Request.QueryString.AllKeys)
            {
                if (key.ToLower() == "sign" || key.ToLower() == "token")
                    continue;
                if (filterContext.RequestContext.HttpContext.Request[key] == null) continue;
                builder.AppendFormat("{0}={1}", key, filterContext.RequestContext.HttpContext.Request[key]);
                if (builder.Length > 0)
                    builder.Append("&");
            }
            foreach (var key in filterContext.RequestContext.HttpContext.Request.Form.AllKeys)
            {
                if (key.ToLower() == "sign" || key.ToLower() == "token")
                    continue;
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
        /// 返回错误结构
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual string GetErrorResult(string code, string message)
        {
            var result = new Dictionary<string, string> { { "Code", code }, { "Message", message } };
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
    }
}
