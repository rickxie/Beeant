using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dependent;
using Beeant.Application.Services;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Presentation.Website.Register.Models.Home;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Website.Register.Controllers.Home
{
    public class HomeController : BaseController
    {
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
            if (model == null)
                return View("Index");
            if (!model.IsReadProtocol)
            {
                model.Errors = model.Errors ??
                               new List<ErrorInfo> { new ErrorInfo { Key = "IsReadProtocolError", Message = "请先阅读并同意用户注册协议" } };
           
            }
            else if (!CodeHelper.ValidateCode(model.Code, "RegisterCode"))
            {
                model.Errors = model.Errors ??
                           new List<ErrorInfo> { new ErrorInfo { Key = "RegisterCodeError", Message = "验证码错误" } };
            }
            else
            {
                var info = new AccountEntity
                {
                    Name = model.Name,
                    Password = model.Password,
                    Email = "电子邮箱".Equals(model.Email) ? null : model.Email,
                    Mobile = "手机号码".Equals(model.Mobile) ? null : model.Mobile,
                    RealName = "真实姓名".Equals(model.RealName) ? null : model.RealName,
                    SaveType = SaveType.Add,
                    IsUsed = true,
                    IsActiveEmail = false,
                    IsActiveMobile = false
                };
                info.AccountIdentites=new List<AccountIdentityEntity>();
                info.AccountIdentites.Add(new AccountIdentityEntity
                {
                    Account = info,
                    Name = "Name",
                    Number = info.Name,
                    SaveType = SaveType.Add
                });
                var rev = Ioc.Resolve<IApplicationService, AccountEntity>().Save(info);
                if (rev)
                    return RedirectToAction("RegisterFul", model);
                model.Errors = info.Errors;
            }
            return View("Index", model);
           
        }
        /// <summary>
        /// 注册成功
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult RegisterFul(RegisterModel model)
        {
            return View("RegisterFul",model);
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult RegisterCode()
        {
            return null;// this.CookieCodeActionResult("RegisterCode");
        }

        #region 验证用户名


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

        #endregion


    }
}
