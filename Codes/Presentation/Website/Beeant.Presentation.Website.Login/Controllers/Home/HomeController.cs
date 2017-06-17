using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Dependent;
using Beeant.Application.Services.Utility;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Utility;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Presentation.Website.Login.Models.Home;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Domain.Entities;
using Winner.Filter;

namespace Beeant.Presentation.Website.Login.Controllers.Home
{
    public class HomeController : BaseController
    {
        private const string CodeErrorName = "LoginCodeError";
        private const string CodeName = "LoginCode";
        /// <summary>
        /// 是否显示验证码
        /// </summary>
        public virtual bool IsShowCode
        {
            get
            {
                var count = CodeHelper.GetErrorCount(CodeErrorName);
                if (count == null || count >= 5)
                    return true;
                return false;
            }
        }

        #region 登录对话框
        //
        // GET: /Index/
        [HttpGet]
        public ActionResult Index(string url)
        {
            if (Identity != null)
            {
                return
                    new RedirectResult(string.IsNullOrEmpty(url)
                        ? Configuration.ConfigurationManager.GetSetting<string>("PresentationWebsiteHomeUrl")
                        : url);
            }
            CodeHelper.InitilzeCodeErrorCount(CodeErrorName);
            var model = new LoginModel {Url = url,Name=GetLoginCookie()};
            return View(model);
        }

        [HttpPost]
        public ActionResult Index([Bind(Exclude = "Errors,Identity,Advertisements")] LoginModel model)
        {
            SetLoginModel(model);
            if (model == null || model.Errors != null && model.Errors.Count > 0)
            {
                return View(model);
            }
            return
                new RedirectResult(string.IsNullOrEmpty(model.Url)
                    ? this.GetUrl("PresentationWebsiteHomeUrl")
                    : model.Url);
        }

        /// <summary>
        /// 登陆框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoginDialog()
        {
            CodeHelper.InitilzeCodeErrorCount(CodeErrorName);
            return View();
        }
         
        [HttpPost]
        public ActionResult LoginDialog([Bind(Exclude = "Errors,Identity")]LoginModel model)
        {
            SetLoginModel(model);
            return View(model);
        }

        /// <summary>
        /// 设置登录
        /// </summary>
        /// <param name="model"></param>
        protected virtual void SetLoginModel(LoginModel model)
        {
            if (model == null) return;
            if (IsShowCode && !CodeHelper.ValidateCode(model.Code, CodeName))
            {
                model.Errors = new List<ErrorInfo>
                {
                    new ErrorInfo("CodeError", "验证码错误")
                };
            }
            else if (CheckLogin(model))
            {
                HttpCookie cookie = Request.Cookies["LoginCode"];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.AppendCookie(cookie);
                }
            }
            model.IsShowCode = IsShowCode;
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public virtual void LoginCode()
        {
            CodeHelper.CreateCode(CodeName);
        }
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="model"></param>
        protected virtual bool CheckLogin(LoginModel model)
        {
            if (model == null)
                return false;
            if (IsShowCode && !CodeHelper.ValidateCode(model.Code, CodeName))
            {
                model.Errors = new List<ErrorInfo>
                {
                    new ErrorInfo("CodeError", "验证码错误")
                };
            }
            var info = new LoginEntity
            {
                Name = model.Name,
                Password = model.Password,
                Type = model.Type,
                LockerTag = IdentityEntity.LockTag,
            };
            Ioc.Resolve<ILoginApplicationService>().Login(info);
            model.Errors = info.Errors;
            var rev = model.Errors == null || model.Errors.Count == 0;
            if (rev && info.Identity!=null)
            {
                var identity = info.Identity;
                var token = Ioc.Resolve<IIdentityApplicationService>().Set(identity);
                if (token!=null)
                {
                    SetLoginCookie(model,token, identity);
                }
                this.AddLoginLog(identity, "Account", "");
                CodeHelper.RemoveCodeErrorCount(CodeErrorName);
            }
            else
            {
                model.IsShowCode = IsShowCode;
            }
            return rev;
        }

        /// <summary>
        /// 设置登录信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <param name="identity"></param>
        protected virtual void SetLoginCookie(LoginModel model, TokenEntity token, IdentityEntity identity)
        {
            if(identity==null || !model.IsSaveCookie)
                return;
            var usernamecookie = new HttpCookie("username")
            {
                Expires = DateTime.Now.AddDays(7),
                Value = Server.UrlEncode(identity.Name)
            };
            Response.AppendCookie(usernamecookie);
        }
        /// <summary>
        /// 设置登录信息
        /// </summary>
        protected virtual string GetLoginCookie()
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["username"];
            if (cookie != null)
            {
                return System.Web.HttpContext.Current.Server.UrlDecode(cookie.Value);
            }
            return null;
        }
        #endregion

        #region 退出
        /// <summary>
        /// 退出对话框
        /// </summary>
        public virtual ActionResult Quit(string url)
        {
            Ioc.Resolve<IIdentityApplicationService>().Remove();
            if (string.IsNullOrWhiteSpace(url))
                return RedirectToAction("Index");
            return new RedirectResult(url);
        }


        #endregion



    }
}
