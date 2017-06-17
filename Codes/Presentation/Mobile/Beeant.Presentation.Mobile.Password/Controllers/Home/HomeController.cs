using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Beeant.Application.Services.Account;
using Dependent;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Security;
using Beeant.Presentation.Mobile.Password.Models.Home;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Mobile.Password.Controllers.Home
{
    public class HomeController : MobileBaseController
    {
        /// <summary>
        /// 得到支付示例
        /// </summary>
        /// <returns></returns>
        protected virtual IPasswordApplicationService GetPasswordApplicationService()
        {
            if (Request["type"] == "pay")
                return Ioc.Resolve<IPaywordApplicationService>();
            return Ioc.Resolve<IPasswordApplicationService>();
        }

        #region 找回密码

        /// <summary>
        /// 找回密码
        /// </summary>
        [HttpGet]
        public virtual ActionResult Index()
        {
            var model = new PasswordModel
            {
                Step = 1
            };
            return View("Index", model);
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        [HttpPost]
        public virtual ActionResult Check([Bind(Include = "Name,Code")] PasswordModel model)
        {
            if(model==null)
                return RedirectToAction("Index");
            if (!CodeHelper.ValidateCode(model.Code, "PasswordCode"))
            {
                model.Errors = model.Errors ??
                               new List<ErrorInfo> { new ErrorInfo { Key = "CodeError", Message = "验证码错误" } };
            }
            else
            {

                var account =GetPasswordApplicationService().CheckAccount(model.Name);
                if (account == null)
                {
                    model.Errors = model.Errors ??
                                   new List<ErrorInfo> { new ErrorInfo { Key = "AccountNoExist", Message = "账户不存在" } };
                }
                else
                {
                    HidePartEntity(model, account);
                }
            }
            if (model.Errors != null && model.Errors.Count > 0)
                return View("Index", model);
            model.Step = 2;
            return View("Check", model);
        }
        /// <summary>
        /// 隐藏部分信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="account"></param>
        protected virtual void HidePartEntity(PasswordModel model, AccountEntity account)
        {
            if (!string.IsNullOrEmpty(account.Mobile) && account.IsActiveMobile)
            {
                var builder = new StringBuilder();
                if (account.Mobile.Length == 11)
                {
                    builder.Append(account.Mobile.Substring(0, 3));
                    builder.Append("****");
                    builder.Append(account.Mobile.Substring(7, 4));
                    model.AccountMobile = builder.ToString();
                }
            }
            if (!string.IsNullOrEmpty(account.Email) && account.IsActiveEmail)
            {
                var builder = new StringBuilder();
                var values = account.Email.Split('@');
                if (values.Length > 1)
                {
                    if (values[0].Length > 2)
                        builder.Append(values[0].Substring(0, 2));
                    for (int i = 2; i < values[0].Length; i++)
                    {
                        builder.Append("*");
                    }
                    builder.Append("@");
                    builder.Append(values[1]);
                    model.AcountEmail = builder.ToString();
                }
            }
        }

        /// <summary>
        /// 确认验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult SendCode([Bind(Include = "Name,ValidateValue,ValidateType")] PasswordModel model)
        {
            var result = new Dictionary<string, object>();
            if (model == null)
            {
                result.Add("Status", false);
                result.Add("Message","操作超时,请刷新后重新操作");
                return this.Jsonp(result);
            }
            var code = new CodeEntity
            {
                Name = model.Name,
                Type = (CodeType) model.ValidateType
            };
            var dto = GetPasswordApplicationService().SendCode(model.Name, code);
            var rev = !dto.IsTimeout && (code.Errors == null || code.Errors.Count == 0);
            var mess = code.Errors?.FirstOrDefault()?.Message;
            mess = mess ?? "发送失败";
            result.Add("Status", rev);
            result.Add("Message", rev ? code.SendStep.ToString() : dto.IsTimeout? "操作超时,请刷新后重新操作" : mess);
            return this.Jsonp(result);
        }

        /// <summary>
        /// 确认验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult CheckCode([Bind(Include = "Name,ValidateValue,ValidateType")] PasswordModel model)
        {
            if (model == null)
                return RedirectToAction("Index");
            var dto = GetPasswordApplicationService()
                .CheckCode(model.Name, model.ValidateValue, (CodeType) model.ValidateType);
            if (dto.IsTimeout)
            {
                FillTimeoutError(model);
                return View("Index", model);
            }
            if (dto.Result)
            {
                model.Step = 3;
                return View("Reset", model);
            }
            model.Errors = model.Errors ??
                     new List<ErrorInfo> { new ErrorInfo { Key = "ValidateValueError", Message = "验证码错误" } };
            return View("Check", model);
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Reset([Bind(Include = "Name,Password,SurePassword")] PasswordModel model)
        {
            if (model == null)
                return RedirectToAction("Index");
            if (model.Password != model.SurePassword)
            {
                model.Errors = model.Errors ??
                          new List<ErrorInfo> { new ErrorInfo { Key = "SurePasswordError", Message = "两次输入的密码不一致" } };
            }
            else
            {
 
                var dto = GetPasswordApplicationService().Reset(model.Name,model.Password);
                if (dto.IsTimeout)
                    return RedirectToAction("Index");
                if (dto.Result)
                {
                    model.Step = 4;
                    return RedirectToAction("Finish");
                }
                  
                model.Errors = dto.Errors;

            }
            return View("Reset", model);
        }
        /// <summary>
        /// 完成密码
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Finish()
        {

            return View("Finish");
        }


        /// <summary>
        /// 检查Seession
        /// </summary>
        /// <returns></returns>
        protected virtual bool FillTimeoutError(PasswordModel model)
        {
          
            model.Errors = model.Errors ??
                           new List<ErrorInfo>
                               {
                                   new ErrorInfo {Key = "ValidateValueError", Message = "您操作超时，请重新输入用户名"}
                               };
            return false;
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public virtual void Code()
        {
            CodeHelper.CreateCode("PasswordCode");
        }
        /// <summary>
        /// 得到账户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual AccountEntity GetAccount(string name)
        {
            var query = new QueryInfo();
            query.Query<AccountEntity>().Where(it => it.Name == name).Select(it => new object[] { it.Id, it.Name, it.Email, it.Mobile });
            var info = this.GetEntities<AccountEntity>(query).FirstOrDefault();
            return info;
        }

        #endregion

    }
}
