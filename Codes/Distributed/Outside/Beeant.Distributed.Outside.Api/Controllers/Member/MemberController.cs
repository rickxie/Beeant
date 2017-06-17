using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Beeant.Application.Services;
using Component.Extension;
using Dependent;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;
using Winner.Persistence.Linq;
using MemberEntity = Beeant.Domain.Entities.Member.MemberEntity;

namespace Beeant.Distributed.Outside.Api.Controllers.Member
{
    //[AuthorityFilter]
    public class MemberController : ApiBaseController
    {
        public virtual ActionResult Update()
        {
            try
            {
                var rev = UpdateMember();
                return rev;
            }
            catch (Exception ex)
            {
                return ReturnExceptionResult(ex);
            }
        }

        /// <summary>
        /// 更新会员表
        /// </summary>
        /// <returns></returns>
        protected virtual ActionResult UpdateMember()
        {
            if (string.IsNullOrEmpty(Request["AccountId"]))
                return ReturnFailureResult("AccountId不能为空");
            long accountId = Request["AccountId"].Convert<long>();
            if (accountId <= 0)
                return ReturnFailureResult("AccountId" + accountId + "不正确");
            var menberInfo = GetMemberByAccount(new AccountEntity { Id = accountId });
            if (menberInfo != null)
            {

                SetMember(menberInfo, Request.QueryString.AllKeys);
                SetMember(menberInfo, Request.Form.AllKeys);
                menberInfo.SaveType = SaveType.Modify;
                var rev = Ioc.Resolve<IApplicationService, MemberEntity>().Save(menberInfo);
                if (rev) return ReturnSuccessResult("保存成功");
            }
            return ReturnFailureResult("保存失败");
        }

        /// <summary>
        /// 设置会员
        /// </summary>
        /// <param name="member"></param>
        /// <param name="keys"></param>
        protected virtual void SetMember(MemberEntity member, string[] keys)
        {
            foreach (var key in keys)
            {
                if (key == "Password") continue;
                if (IsAccountProperty(key))
                {
                    member.Account?.SetProperty(key);
                    if (member.Account != null)
                    {
                        member.Account.SaveType = SaveType.Modify;
                        Winner.Creator.Get<Winner.Base.IProperty>().SetValue(member.Account, key, Request[key]);
                    }

                }
                else
                {
                    Winner.Creator.Get<Winner.Base.IProperty>().SetValue(member, key, Request[key]);
                    member.SetProperty(key);
                }

            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual bool IsAccountProperty(string key)
        {
            var property = typeof(AccountEntity)
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(it => it.Name.Equals(key));
            if (property != null) return true;
            return false;
        }

 

        /// <summary>
        /// 根据Account获取Member
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual MemberEntity GetMemberByAccount(AccountEntity info)
        {
            var query = new QueryInfo();
            query.Query<MemberEntity>().Where(it => it.Account.Id == info.Id);
            return Ioc.Resolve<IApplicationService>().GetEntities<MemberEntity>(query)?.FirstOrDefault();
        }
    }
}