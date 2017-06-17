using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dependent;
using Beeant.Application.Services;
using Beeant.Application.Services.Security;
using Beeant.Application.Services.Utility;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Agent;
using Beeant.Domain.Entities.Security;
using Beeant.Presentation.Mobile.Register.Models.Home;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Mobile.Register.Controllers.Home
{
    public class HomeController : MobileBaseController
    {
        private const string CodeName = "RegisterCode";
        private const string CodeTag = "AccountRegister";
        #region 注册

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Index(string url)
        {
            var model = new RegisterModel
            {
                Url = url
            };
            return View("Index", model);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Register(RegisterModel model)
        {
            if (model == null || !CheckRegister(model))
                return View("Index", model);
            string ip = Request.UserHostAddress;
            var ipEntity = Ioc.Resolve<IIpApplicationService>().Get(ip);
            string city = ipEntity != null ? ipEntity.City : "";
            var info = new AccountEntity
            {
                Name = string.Format("m{0}", model.Mobile),
                Password = model.Password,
                Email = "",
                Mobile = model.Mobile,
                RealName = "",

                Payword="",
                SaveType = SaveType.Add,
                IsUsed = true,
                IsActiveEmail = false,
                IsActiveMobile = true
            };
            info.AccountIdentites=new List<AccountIdentityEntity>();
            info.AccountIdentites.Add(new AccountIdentityEntity
            {
                Account= info,
                Name="Mobile",
                Number = model.Mobile,
                SaveType = SaveType.Add
            });
            info.AccountIdentites.Add(new AccountIdentityEntity
            {
                Account = info,
                Name = "Name",
                Number = info.Name,
                SaveType = SaveType.Add
            });
            var rev = Ioc.Resolve<IApplicationService, AccountEntity>().Save(info);
            if (rev && model.IsLogin==true)
            {
                Login(model);
                var url = !string.IsNullOrEmpty(model.Url) ? model.Url : this.GetUrl("PresentationMobileHomeUrl");
                return new RedirectResult(url);
            }
            model.Errors = info.Errors;
            return View("Index", model);

        }
 

        /// <summary>
        /// 登入
        /// </summary>
        protected virtual void Login(RegisterModel model)
        {
            if (Identity != null)
                Ioc.Resolve<IIdentityApplicationService>().Remove();
            var info = new Domain.Entities.Utility.LoginEntity
            {
                Name = model.Mobile,
                Password = model.Password
            };
            Ioc.Resolve<ILoginApplicationService>().Login(info);
            var rev = model.Errors == null || model.Errors.Count == 0;
            if (rev)
            {
                var identity = info.Identity;
                var token = Ioc.Resolve<IIdentityApplicationService>().Set(identity);
                this.AddLoginLog(identity, "Account", "");
            }
        }

        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual bool CheckRegister(RegisterModel model)
        {
            if (model.SurePassword != model.Password)
            {
                model.Errors = model.Errors ??
                          new List<ErrorInfo>();
                model.Errors.Add(new ErrorInfo { Key = "SurePasswordError", Message = "两次输入密码不一致" });
            }
            var rev= Ioc.Resolve<ICodeApplicationService>()
                .ValidateCode(CodeTag, model.Mobile,CodeType.Mobile, model.MobileCode);
            if (!rev)
            {
                model.Errors = model.Errors ??
                            new List<ErrorInfo> ();
                model.Errors.Add(new ErrorInfo { Key = "MobileCodeError", Message = "手机验证码不正确" });
            }
            return rev;
        }
  
        #region 验证用户名

        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual ActionResult SendMobileCode(string mobile,string code)
        {
            var dis=new Dictionary<string,object>();
            if (!CodeHelper.ValidateCode(code,CodeName))
            {
                dis.Add("status", false);
                dis.Add("message","验证码错误");
                return Json(dis, JsonRequestBehavior.AllowGet);
            }
            var info = new CodeEntity
            {
                Tag=CodeTag,
                Name = mobile,
                Type=CodeType.Mobile,
                Subject = "注册验证码",
                Body = "{0}",
                ToAddress= mobile,
                SaveType=SaveType.Add
            };
            var rev=this.SaveEntity(info);
            if (!rev)
            {
                var error = info.Errors?.FirstOrDefault();
                var message = error == null ? "" : error.Message;
                dis.Add("status", false);
                dis.Add("message", message);
            }
            else
            {
                dis.Add("status", true);
                dis.Add("message", info.SendStep);
            }
            return Json(dis,JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 检查用户名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public virtual bool CheckAccountIdentity(string name, string number)
        {
            var query = new QueryInfo();
            query.Query<AccountIdentityEntity>().Where(it => it.Name == name && it.Number == number).Select(it => it.Id);
            var infos = Ioc.Resolve<IApplicationService, AccountIdentityEntity>().GetEntities<AccountIdentityEntity>(query);
            if (infos != null && infos.Count == 0)
                return true;
            return false;
        }
        #endregion

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
