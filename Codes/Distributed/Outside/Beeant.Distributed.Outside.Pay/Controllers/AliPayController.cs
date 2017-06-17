using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Distributed.Outside.Pay.Models;
using Beeant.Domain.Entities.Finance;

namespace Beeant.Distributed.Outside.Pay.Controllers
{

    public class AliPayController : PayController
    {

        #region 创建
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AuthorizeFilter]
        public virtual ActionResult Create(PaylineModel model)
        {
            return Create(model, PaylineType.Ali);
        }
        /// <summary>
        /// 通知
        /// </summary>
        public virtual ActionResult Process()
        {
            return Process(PaylineType.Ali);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AuthorizeFilter]
        public virtual ActionResult Refund(PaylineModel model)
        {
            return Refund(model, PaylineType.Ali);
        }

        #endregion
    }
}
