using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Application.Services;
using Beeant.Application.Services.Utility;
using Beeant.Domain.Entities.Member;
using Beeant.Domain.Entities.Utility;
using Component.Extension;
using Dependent;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;
using Beeant.Domain.Entities.Agent;

namespace Beeant.Distributed.Outside.Api.Controllers.Account
{
    [TokenFilter]
    public class AccountController : ApiBaseController
    {
        #region 注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public virtual ActionResult Register()
        {
            try
            {
                var info = new AccountEntity();
                var mess = Register(info);
                return mess
                    ? ReturnSuccessResult("注册成功")
                    : ReturnFailureResult(info.Errors != null && info.Errors.Count > 0
                        ? string.Join(",", info.Errors.Select(it => it.Message).ToArray())
                        : "注册失败");

            }
            catch (Exception ex)
            {
                return ReturnExceptionResult(ex);
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="info"></param>
        protected virtual bool Register(AccountEntity info)
        {
            string mobile = Request["Mobile"];
            string name = mobile;
            if (!string.IsNullOrEmpty(name))
            {
                name = "m" + name;
            }
            else
            {
                name = string.Format("{0}{1}", Request["PlatformType"], Request["OpenID"]);
            }
            string city = Request["City"];
            string ip = Request["ip"];
            if (string.IsNullOrEmpty(city))
            {
                var ipInfo = Ioc.Resolve<IIpApplicationService>().Get(ip);
                if (ipInfo != null)
                {
                    city = ipInfo.City;
                }
            }
            info.Name = name;
            info.Password = Request["Password"];
            info.Email = Request["Email"];
            info.Mobile = Request["Mobile"];
            info.RealName = Request["RealName"];

            info.IsUsed = true;
            info.IsActiveMobile = !string.IsNullOrEmpty(mobile);
            info.SaveType = SaveType.Add;
            string thirdParty = Request["ThirdParty"];
            string openId = Request["OpenID"];
            if (!string.IsNullOrEmpty(thirdParty) && string.IsNullOrEmpty(openId))
            {
                info.Thirdparties = new List<ThirdpartyEntity>();
                info.Thirdparties.Add(new ThirdpartyEntity
                {
                    Type = (ThirdpartyType)Enum.Parse(typeof(ThirdpartyType), thirdParty),
                    OpenId = openId,
                    SaveType = SaveType.Add
                });
            }
            var rev = Ioc.Resolve<IApplicationService, AccountEntity>().Save(info);
            if (rev)
            {
                try
                {

                    var member = new MemberEntity
                    {
                        Account = new AccountEntity { Id = info.Id },
                        Gender = Request["Gender"],
                        Agent = new AgentEntity { Id = 0 },
                        HeadUrl = Request["HeadUrl"],
                        Nickname = Request["RealName"],
                        Address = string.Empty,
                        IdCardNumber = string.Empty,
                        Birthday = "1990-01-01".Convert<DateTime>(),
                        IsUsed = true,
                        Telephone = string.Empty,
                        Postal = string.Empty,
                        Remark = string.Empty,
                        SaveType = SaveType.Add
                    };

                    Ioc.Resolve<IApplicationService, MemberEntity>().Save(member);

                }
                catch (Exception ex)
                {
                    this.AddExceptionLog(ex);
                }
                return true;
            }
            return false;
        }
        #endregion

        #region 登录

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Login()
        {
            try
            {
                var model = new LoginEntity
                {
                    Name = Request["Name"],
                    Password = Request["Password"],
                    Type = Request["Type"]
                };
                model = Ioc.Resolve<ILoginApplicationService>().Login(model);
                var rev = model.Identity != null;
                string address = Request["Address"];
                string device = Request["Device"];
                string ip = Request["Ip"];
                string city = Request["City"];
                string type = Request["Type"];
                if (rev)
                {
                    var identity = (AccountIdentityEntity)model.Identity;
                    var token = Ioc.Resolve<IIdentityApplicationService>().Set(identity);
                    if (token != null)
                    {
                        this.AddLoginLog(identity, type, ip, city, address, device, "");
                        return Json(token, JsonRequestBehavior.AllowGet);
                    }
                }
                var error = model.Errors?.FirstOrDefault();
                this.AddLoginLog(new AccountIdentityEntity { AccountName = model.Name }, type, ip, city, address, device, error == null ? "" : error.Message);
                return Content("");
            }
            catch (Exception ex)
            {
                return ReturnExceptionResult(ex);
            }
        }



        #endregion

        #region 得到登录信息

        /// <summary>
        /// 得到登录信息
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public virtual ActionResult GetIdentity(string ticket)
        {
            try
            {
                var info = Ioc.Resolve<IIdentityApplicationService>().Get<AccountIdentityEntity>(ticket);
                return Json(info, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ReturnExceptionResult(ex);
            }
        }
        #endregion

        #region 退出
        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Quit(string ticket)
        {
            try
            {
                Ioc.Resolve<IIdentityApplicationService>().Remove(ticket);
                return ReturnSuccessResult("退出成功");
            }
            catch (Exception ex)
            {
                return ReturnExceptionResult(ex);
            }
        }
        #endregion

     
    }
}
