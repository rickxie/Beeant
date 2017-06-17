using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dependent;
using Beeant.Application.Services.Account;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Presentation.Mobile.Password.Models.Account;
using Winner.Filter;

namespace Beeant.Presentation.Mobile.Password.Controllers.Account
{
    [AuthorizeFilter]
    public class MobileController : MobileBaseController
    {

        #region 首页

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            var model = new MobileModel
            {
                AccountId=Identity.Id
            };
            Ioc.Resolve<IMobileApplicationService>().Load(model);
            return GetActionResult(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Action(MobileModel model)
        {
            model.AccountId = Identity.Id;
            Ioc.Resolve<IMobileApplicationService>().Action(model);
            return GetActionResult(model);
        }
        /// <summary>
        /// 得到结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual ActionResult GetActionResult(MobileModel model)
        {
            switch (model.Action)
            {
                case "Valid":
                    model.Step = 2;
                    return View("~/Views/Account/Mobile/Index.cshtml", model);
                case "Bind":
                    model.Step = 3;
                    return View("~/Views/Account/Mobile/Bind.cshtml", model);
                case "Finish":
                    model.Step = 4;
                    return View("~/Views/Account/Mobile/Finish.cshtml", model);
                default:
                    return null;
            }
        }

        private const string CodeName = "ResetMobileCode";
        /// <summary>
        /// 确认验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult SendCode(string value, string code,string action)
        {
            var model=new MobileModel {AccountId = Identity.Id, Action = action,Mobile= value };
            if (!CodeHelper.ValidateCode(code, CodeName))
            {
                model.Errors = model.Errors ??
                               new List<ErrorInfo> { new ErrorInfo { Key = "CodeError", Message = "验证码错误" } };
            }
            else
            {
                var dto = Ioc.Resolve<IMobileApplicationService>().SendCode(model);
                model.Result = dto.Result;
                model.Errors = dto.Errors;
            }

            var result=new Dictionary<string,object>();
            var rev = model.Result;
            var mess = model.Errors?.FirstOrDefault()?.Message;
            mess = mess ?? "发送失败";
            result.Add("Status", rev);
            result.Add("Message", rev ? model.CodeEntity.SendStep.ToString() : mess);
            return this.Jsonp(result);
        }
        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public virtual void Code()
        {
            CodeHelper.CreateCode(CodeName);
        }
        #endregion

    }
}
