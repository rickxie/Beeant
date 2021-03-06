﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Application.Services.Account;
using Dependent;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Account;
using Beeant.Presentation.Mobile.Password.Models.Account;
using Winner.Filter;
using Winner.Persistence;


namespace Beeant.Presentation.Mobile.Password.Controllers.Account
{
    [AuthorizeFilter]
    public class PaywordController : MobileBaseController
    {

        #region 首页

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            return View("~/Views/Account/Password/Index.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Modify(PaywordModel model)
        {
            if (model.NewPassword!=model.SurePassword)
            {
                model.Errors = model.Errors ?? new List<ErrorInfo>
                {
                    new ErrorInfo {Key = "SurePasswordError", Message = "两次输入密码不一致"}
                };
                return View("~/Views/Account/Payword/Index.cshtml", model);
            }
            if (string.IsNullOrEmpty(model.OldPassword))
            {
                model.Errors = model.Errors ?? new List<ErrorInfo>
                {
                    new ErrorInfo {Key = "OldPasswordEmpty", Message = "请输入原始密码"}
                };
                return View("~/Views/Account/Payword/Index.cshtml", model);
            }
            var rev = Ioc.Resolve<IPaywordApplicationService>().CheckPassword(Identity.Id, model.OldPassword);
            if (!rev)
            {
                model.Errors = model.Errors ?? new List<ErrorInfo>
                {
                    new ErrorInfo {Key = "OldPasswordError", Message = "原始密码错误"}
                };
                return View("~/Views/Account/Payword/Index.cshtml", model);
            }
            var account = new AccountEntity
            {
                Id = Identity.Id,
                Password = model.NewPassword,
                SaveType=SaveType.Modify
            };
            account.SetProperty(it => it.Password);
            this.SaveEntity(account);
            model.Errors = account.Errors;
            return View("~/Views/Account/Payword/Index.cshtml", model);
        }
        /// <summary>
        /// 得到结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual ActionResult GetActionResult(EmailModel model)
        {
            switch (model.Action)
            {
                case "Valid":
                    model.Step = 2;
                    return View("~/Views/Account/Email/Index.cshtml", model);
                case "Bind":
                    model.Step = 3;
                    return View("~/Views/Account/Email/Bind.cshtml", model);
                case "Finish":
                    model.Step = 4;
                    return View("~/Views/Account/Email/Finish.cshtml", model);
                default:
                    return null;
            }
        }

        private const string CodeName = "ResetEmailCode";
        /// <summary>
        /// 确认验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult SendCode(string email,string code,string action)
        {
            var model=new EmailModel {AccountId = Identity.Id, Action = action };
            if (!CodeHelper.ValidateCode(code, CodeName))
            {
                model.Errors = model.Errors ??
                               new List<ErrorInfo> { new ErrorInfo { Key = "CodeError", Message = "验证码错误" } };
            }
            var result=new Dictionary<string,object>();

            var dto = Ioc.Resolve<IEmailApplicationService>().SendCode(model);
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
