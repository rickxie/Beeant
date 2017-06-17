using System;
using System.Linq;
using System.Web.Mvc;
using Configuration;
using Dependent;
using Beeant.Application.Dtos.Order;
using Beeant.Application.Services.Order;
using Beeant.Domain.Entities.Merchant;
using Beeant.Domain.Entities.Member;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Presentation.Website.Buy.Models.Home;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Website.Buy.Controllers.Home
{
    [AuthorizeFilter(IsRedirect = true)]
    public class HomeController : BaseController
    {

        public long IdentityUserId
        {
            get { return ConfigurationManager.GetSetting<long>("SystemUserId"); }
        }
        #region 结算

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual ActionResult Settlement(SettlementDto dto)
        {
            dto.IsGenerate = false;
            var model = new SettlementModel();
            SetAddresses(model);
            SetCoupons(model);
            SetInvoices(model);
            model.SettlementDto = Ioc.Resolve<IOrderApplicationService>().Create(dto);
            SetPartners(model);
            return View("Settlement", model);
        }
      
        /// <summary>
        /// 得到会员地址信息
        /// </summary>
        /// <returns></returns>
        protected virtual void SetAddresses(SettlementModel model)
        {
            var query = new QueryInfo();
            query.Query<AddressEntity>();
            model.Addresses = this.GetEntitiesByIdentity<AddressEntity>(query);
          
        }

        /// <summary>
        /// 得到会员优惠券信息
        /// </summary>
        /// <returns></returns>
        protected virtual void SetCoupons(SettlementModel model)
        {
            var query = new QueryInfo();
            query.Query<CouponEntity>()
                 .Where(it => it.EndDate >= DateTime.Now.Date && !it.IsUsed)
                 .Select(it => new object[] {it.Id,it.Name, it.Amount});
            model.Coupons = this.GetEntitiesByIdentity<CouponEntity>(query);

        }
        /// <summary>
        /// 得到会员发票信息
        /// </summary>
        /// <returns></returns>
        protected virtual void SetInvoices(SettlementModel model)
        {
            var query = new QueryInfo();
            query.Query<InvoiceEntity>().Select(it => new object[] { it, it.Title,it.Type,it.GeneralType,it.Content});
            model.Invoices = this.GetEntitiesByIdentity<InvoiceEntity>(query);
        }
        /// <summary>
        /// 得到会员发票信息
        /// </summary>
        /// <returns></returns>
        protected virtual void SetPartners(SettlementModel model)
        {
            if(model.SettlementDto==null ||  model.SettlementDto.Products==null)
                return;
            var accountIds =
                model.SettlementDto.Products.Where(
                    it => it.Product != null && it.Product.Goods != null && it.Product.Goods.Account != null)
                     .Select(it => it.Product.Goods.Account.Id)
                     .ToArray();
            var query = new QueryInfo();
            query.Query<PartnerEntity>().Where(it => accountIds.Contains(it.Account.Id)).Select(it => new object[] { it.Account.Id, it.Name });
            model.Partners = this.GetEntities<PartnerEntity>(query);
            model.Partners.Add(new PartnerEntity{Id=0});
        }
        #endregion

        #region 下单

        /// <summary>
        /// 下单成功
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Sucessful()
        {
            return View("Sucessful");
        }

        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual ActionResult Order(SettlementDto dto)
        {
            dto.IsGenerate = true;
            Ioc.Resolve<IOrderApplicationService>().Create(dto);
            if (dto.IsSaveSuccess)
                return RedirectToAction("Sucessful");
            var model = new SettlementModel {SettlementDto = dto};
            SetAddresses(model);
            SetCoupons(model);
            SetPartners(model);
            return View("Settlement", model);
        }

        #endregion



    }
}
