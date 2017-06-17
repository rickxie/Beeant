using System.Collections.Generic;
using System.Web.Mvc;
using Component.Extension;

namespace Beeant.Basic.Services.Mvc.FilterAttribute
{
    public class SignFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Verify(filterContext))
            {
                filterContext.Result = null;
            }
            base.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual bool Verify(ActionExecutingContext filterContext)
        {
 
            var signValue = filterContext.RequestContext.RouteData.Values["sign"] ??
                   filterContext.RequestContext.HttpContext.Request["sign"];
            var markValue = filterContext.RequestContext.RouteData.Values["mark"] ??
               filterContext.RequestContext.HttpContext.Request["mark"];
            var mark = markValue.Convert<string>();
            var sign= signValue.Convert<string>();
         
            if (string.IsNullOrEmpty(sign) || string.IsNullOrEmpty(mark) || sign.Length<10 ||
                Winner.Creator.Get<Winner.Base.ISecurity>().EncryptSign(sign) != mark)
            {
                return false;
            }
            return true;
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
