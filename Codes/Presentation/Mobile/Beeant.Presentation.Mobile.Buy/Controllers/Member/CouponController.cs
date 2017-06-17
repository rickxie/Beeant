using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Member;
using Beeant.Presentation.Mobile.Buy.Models.Member;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Mobile.Buy.Controllers.Member
{

    [AuthorizeFilter]
    public class CouponController : MobileBaseController
    {

        #region 首页
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            var model = new CouponModel
            {
                Coupons = GetCoupons()
            };
            return View("~/Views/Member/Coupon/_Index.cshtml", model);
        }
        /// <summary>
        /// 得到会员地址信息
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CouponEntity> GetCoupons()
        {
            var query = new QueryInfo();
            query.Query<CouponEntity>()
                 .Where(it => it.EndDate >= DateTime.Now.Date && !it.IsUsed)
                 .Select(it => new object[] { it.Id, it.Name, it.Amount });
            return this.GetEntitiesByIdentity<CouponEntity>(query);

        }

        #endregion

    }
}
