using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dependent;
using Beeant.Application.Services.Order;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Order;
using Beeant.Presentation.Mobile.Buy.Models.Home;

namespace Beeant.Presentation.Mobile.Buy.Controllers.Home
{
    [AuthorizeFilter]
    public class HomeController : MobileBaseController
    {
      
        #region 首页
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index(BuyModel model)
        {
            model.IsGenerate = false;
            FillBuyModel(model);
            Ioc.Resolve<IOrderApplicationService>().Create(model);
            if (model.PayTypes != null)
            {
                model.PayTypes = model.PayTypes.Where(it => it.Tag == "All" || it.Tag == "Mobile").ToList();
            }
            return View("Index", model);
        }


        #endregion

        #region 创建
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual ActionResult Buy(BuyModel model)
        {
            if (model == null)
                return Content("false");
            model.IsGenerate = true;
            FillBuyModel(model);
            Ioc.Resolve<IOrderApplicationService>().Create(model);
            var result = new Dictionary<string, object>
            {
                {"Status", model.IsSaveSuccess.ToString().ToLower()}
            };
            if (model.IsSaveSuccess)
            {
                result.Add("Message",string.Join(",",model.Orders.Select(it=>it.Order.Id).ToArray()));
            }
            else
            {
                result.Add("Message", model.Errors?.FirstOrDefault()?.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

  

        #endregion

        /// <summary>
        /// 填充
        /// </summary>
        /// <param name="model"></param>
        protected virtual void FillBuyModel(BuyModel model)
        {
            model.OrderType=OrderType.Standard;
            model.ChannelType = ChannelType.Mobile;
            model.AccountId = Identity.Id;
        }


    }
}
