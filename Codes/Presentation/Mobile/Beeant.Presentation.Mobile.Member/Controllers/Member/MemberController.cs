using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Member;
using Beeant.Presentation.Mobile.Member.Models.Member;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Mobile.Member.Controllers.Member
{
    [AuthorizeFilter]
    public class MemberController : MobileBaseController
    {

        #region 首页
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            var memberId = GetMemberId();
            var model=new MemberModel();
            model.Member = this.GetEntity<MemberEntity>(memberId);
            return View("~/Views/Member/Member/Index.cshtml", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Save(MemberModel model)
        {
            var memberId = GetMemberId();
            var member = model.CreateMember(memberId);
            model.Member = member;
            this.SaveEntity(member);
            return View("~/Views/Member/Member/Index.cshtml", model);
        }

        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="headUrl"></param>
        /// <param name="headUrlValue"></param>
        /// <returns></returns>
        public virtual ActionResult UpdateHeadUrl(string headUrl, string headUrlValue)
        {

            var bs = Convert.FromBase64String(headUrlValue);
            var memberId = GetMemberId();
            var member = memberId > 0
                ? new MemberEntity
                {
                    Id = memberId,
                    HeadUrl = headUrl,
                    HeadUrlByte = bs,
                    SaveType = SaveType.Modify
                }
                : new MemberEntity
                {
                    Nickname = "",
                    Gender = "",
                    Birthday = "1990-01-01".Convert<DateTime>(),
                    Telephone = "",
                    IdCardNumber = "",
                    Postal = "",
                    Address = "",
                    Remark = "",
                    HeadUrl = headUrl,
                    HeadUrlByte = bs,
                    SaveType = SaveType.Add
                };
            member.HeadUrl = string.IsNullOrEmpty(headUrl)
                ? ""
                : string.Format("Files/Images/Member/c{0}", Path.GetExtension(headUrl));
            if (member.SaveType == SaveType.Modify)
                member.SetProperty(it => it.HeadUrl);
            var rev = this.SaveEntity(member);
            var mess = rev ? member.FullHeadUrl : member.Errors?.FirstOrDefault()?.Message;
            var result = new Dictionary<string, object>
            {
                {"Status", rev},
                {"Message", mess}
            };
            return this.Jsonp(result);
        }
        /// <summary>
        /// 得到会员编号
        /// </summary>
        /// <returns></returns>
        protected virtual long GetMemberId()
        {
            var query=new QueryInfo();
            query.Query<MemberEntity>().Where(it => it.Account.Id == Identity.Id).Select(it => it.Id);
            var entities = this.GetEntities<MemberEntity>(query);
            var entity = entities?.FirstOrDefault();
            return entity==null?0: entity.Id;
        }
        #endregion

    }
}
