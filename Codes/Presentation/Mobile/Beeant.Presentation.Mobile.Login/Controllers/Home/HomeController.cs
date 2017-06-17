using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Dependent;
using Beeant.Application.Services.Utility;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.Extension.Mobile;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Utility;
using Beeant.Presentation.Mobile.Login.Models.Home;
using Winner.Filter;

namespace Beeant.Presentation.Mobile.Login.Controllers.Home
{
    public class HomeController : MobileBaseController
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
                        ? Configuration.ConfigurationManager.GetSetting<string>("PresentationMobileHomeUrl")
                        : url);
            }
            CodeHelper.InitilzeCodeErrorCount(CodeErrorName);
            var model = new LoginModel { Url = url,IsShowCode = IsShowCode};
            model.InitUserName(this);
            return View(model);
        }




        /// <summary>
        /// 登陆框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Dialog(string url)
        {
            CodeHelper.InitilzeCodeErrorCount(CodeErrorName);
            var model = new LoginModel { Url = url, IsShowCode = IsShowCode };
            model.InitUserName(this);
            return View("Dialog", model);
        }


        /// <summary>
        /// 返回正确
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult FormLogin(LoginModel model)
        {
            var rev = CheckLogin(model);
            if (!rev)
                return View("Index", model);
            CodeHelper.RemoveCodeErrorCount(CodeErrorName);
            return
              new RedirectResult(string.IsNullOrEmpty(model.Url)
                                     ? Configuration.ConfigurationManager.GetSetting<string>("PresentationMobileMemberUrl")
                                     : model.Url);
        }
 
        /// <summary>
        /// 返回正确
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual ActionResult DialogLogin(LoginModel model)
        {
            var rev = CheckLogin(model);
            var result=new Dictionary<string,object>
            {
                {"Status", rev}
            };
            if (rev)
            {
                result.Add("Message",model.Url);
                CodeHelper.RemoveCodeErrorCount(CodeErrorName);
            }
            else
            {
                var message = model.Errors?.FirstOrDefault()?.Message;
                result.Add("Message", message);
            }
            result.Add("IsShowCode",model.IsShowCode);
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
            model.IsShowCode = IsShowCode;
            var info = new LoginEntity
            {
                Name = model.Name,
                Password = model.Password,
                Type = model.Type,
                LockerTag = IdentityEntity.LockTag,
            };
            Ioc.Resolve<ILoginApplicationService>().Login(info);
            model.Errors = info.Errors;
            var rev = model.Errors == null || model.Errors.Count == 0 && info.Identity != null;
            if (rev)
            {
                var identity = info.Identity;
                var token = Ioc.Resolve<IIdentityApplicationService>().Set(identity);
                rev = token != null;
                if (rev)
                {
                    model.SaveUserName(this,token,identity);
                    this.AddLoginLog(identity, "Account", "");
                }
            }
            else
            {
                model.IsShowCode = IsShowCode;
            }
            return rev;
        }

        #endregion

        #region 微信登陆
        //
        // GET: /Index/

        public ActionResult WechatLogin(string url)
        {
            if (Identity != null)
                Ioc.Resolve<IIdentityApplicationService>().Remove();
            var openid = this.Wechat().GetAuthorityOpenId();
            var info = new LoginEntity
            {
                Name = "Wechat",
                Password = openid,
                Type = "ThirdParty",
                LockerTag = IdentityEntity.LockTag,
            };
            Ioc.Resolve<ILoginApplicationService>().Login(info);
            var rev = info.Errors == null || info.Errors.Count == 0 && info.Identity!=null;
            if (rev)
            {
                var identity = info.Identity;
                var token = Ioc.Resolve<IIdentityApplicationService>().Set(identity);
                rev = token != null;
                if (rev)
                {
                    this.AddLoginLog(identity, "Account", "");
                }
                return
                    new RedirectResult(string.IsNullOrEmpty(Request["url"])
                                           ? Configuration.ConfigurationManager.GetSetting<string>("PresentationMobileMemberUrl")
                                           : Request["url"]);
            }
            return RedirectToAction("Index", new RouteValueDictionary { { "url", Request["url"] } });
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
