using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Member;
using Beeant.Presentation.Mobile.Member.Models.Home;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Mobile.Member.Controllers.Home
{
    [AuthorizeFilter]
    public class HomeController : MobileBaseController
    {
      
        #region 首页
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            var model=new HomeModel
            {
                MessageCount = GetMessageCount(),
                Member=GetMember()
            };
            return View("~/Views/Home/Index.cshtml", model);
        }
        /// <summary>
        /// 得到会员
        /// </summary>
        /// <returns></returns>
        protected virtual MemberEntity GetMember()
        {
            var query=new QueryInfo();
            query.Query<MemberEntity>().Where(it => it.Account.Id == Identity.Id).Select(it=>new object[] {it.Id,it.HeadUrl });
            var infos = this.GetEntities<MemberEntity>(query);
            return infos?.FirstOrDefault();
        }
        /// <summary>
        /// 得到消息
        /// </summary>
        /// <returns></returns>
        protected virtual int GetMessageCount()
        {
            var query=new QueryInfo();
            query.SetPageSize(1).Query<MessageEntity>().Where(it=>!it.IsRead).Select(it => it.Id);
            this.GetEntitiesByIdentity<MessageEntity>(query);
            return query.DataCount;
        }


        #endregion




    }
}
