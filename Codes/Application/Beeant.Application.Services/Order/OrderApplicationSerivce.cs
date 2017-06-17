using System;
using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Agent;
using Beeant.Domain.Entities.Cart;
using Beeant.Domain.Entities.Member;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Promotion;
using Beeant.Domain.Services;
using Beeant.Application.Dtos.Order;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Services.Gis;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Order
{
    public class OrderApplicationSerivce : RealizeApplicationService<OrderEntity>,IOrderApplicationService
    {
        #region 属性

   
        /// <summary>
        /// 订单服务
        /// </summary>
        public virtual IDomainService OrderDomainService { get; set; }
        /// <summary>
        /// 福利订单
        /// </summary>
        public virtual IDomainService WelfareOrderDomainService { get; set; }
        /// <summary>
        /// 商家订单
        /// </summary>
        public virtual IDomainService MerchantOrderDomainService { get; set; }
        /// <summary>
        /// 优惠券
        /// </summary>
        public virtual IDomainService CouponDomainService { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public virtual IDomainService IntegralItemDomainService { get; set; }

        /// <summary>
        /// 区域获取
        /// </summary>
        public virtual IDomainService ShopcartDomainService { get; set; }
        /// <summary>
        /// 区域获取
        /// </summary>
        public virtual IAreaRepository AreaRepository { get; set; }
        #endregion

        #region 生成

        /// <summary>
        /// 得到结算
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual SettlementDto Create(SettlementDto dto)
        {
            SetSettlementDto(dto);
            dto.Create();
            if(dto.IsGenerate)
                Generate(dto);
            return dto;
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="dto"></param>
        protected virtual void SetSettlementDto(SettlementDto dto)
        {
            if (dto.AccountId > 0)
                dto.Account = Repository.Get<AccountEntity>(dto.AccountId);
            SetCoupon(dto);
            SetAddress(dto);
            SetInvoice(dto);
            SetAgent(dto);
            SetSettlementProductDtos(dto);
            SetArea(dto);
            SetPayTypes(dto);
        }
        /// <summary>
        /// 设置支付方式
        /// </summary>
        /// <param name="dto"></param>
        protected virtual void SetPayTypes(SettlementDto dto)
        {
            var query=new QueryInfo();
            query.Query<PayTypeEntity>()
                .Where(it => !string.IsNullOrEmpty(it.Url))
                .Select(it => new object[] {it.Name, it.Url, it.Tag});
            dto.PayTypes = Repository.GetEntities<PayTypeEntity>(query);
        }
        /// <summary>
        /// 设置区域
        /// </summary>
        /// <param name="dto"></param>
        protected virtual void SetArea(SettlementDto dto)
        {
            if(!dto.IsArea)
                return;
            if(dto.Address==null || dto.Products==null)
                return;
            if(dto.Products.Count(it=>it.Product!=null && it.Product.Goods!=null && it.Product.Goods.Freight!=null && it.Product.Goods.Freight.IsDelivery)==0)
                return;
           var match= AreaRepository.Match(dto.Address.City, string.Format("{0}{1}", dto.Address.County, dto.Address.Address),"Freight");
            dto.Area = match == null || match.Areas == null ? null : match.Areas.FirstOrDefault();
        }
        /// <summary>
        /// 得到信息
        /// </summary>
        /// <returns></returns>
        protected virtual void SetAddress(SettlementDto dto)
        {
            if(dto.AddressId==0)
                return;
            var query = new QueryInfo();
            query.Query<AddressEntity>().Where(it => it.Account.Id == dto.AccountId && it.Id == dto.AddressId)
                .Select(it => new object[] {it.Id, it.Province, it.City, it.County, it.Address, it.Email, it.Mobile, it.Recipient, it.Company, it.Postcode, it.Telephone, it.Tag });
            var infos = Repository.GetEntities<AddressEntity>(query);
            if (infos == null)
                return;
            dto.Address = infos.FirstOrDefault();
        }
        /// <summary>
        /// 发票
        /// </summary>
        /// <returns></returns>
        protected virtual void SetInvoice(SettlementDto dto)
        {
            if(dto.InvoiceId<=0)
                return;
            var query = new QueryInfo();
            query.Query<InvoiceEntity>().Where(it => it.Account.Id == dto.AccountId && it.Id == dto.InvoiceId)
                .Select(it => new object[] {it });
            var infos = Repository.GetEntities<InvoiceEntity>(query);
            if (infos == null)
                return;
            dto.Invoice = infos.FirstOrDefault();
        }
        /// <summary>
        /// 得到信息
        /// </summary>
        /// <returns></returns>
        protected virtual void SetCoupon(SettlementDto dto)
        {
            if (dto.CouponId <= 0)
                return;
            var query = new QueryInfo();
            query.Query<CouponEntity>()
                .Where(
                    it =>
                        it.Account.Id == dto.AccountId && it.Id == dto.CouponId && it.IsUsed &&
                        it.EndDate >= DateTime.Now.Date)
                .Select(it => new object[] {it.Id, it.Name, it.EndDate, it.Amount});
            var infos = Repository.GetEntities<CouponEntity>(query);
            if (infos == null)
                return;
            dto.Coupon = infos.FirstOrDefault();
        }

        /// <summary>
        /// 得到会员地址信息
        /// </summary>
        /// <returns></returns>
        protected virtual void SetAgent(SettlementDto dto)
        {
            if (dto.AccountId <= 0)
                return;
            var query = new QueryInfo();
            query.Query<AgentEntity>()
                 .Where(it => it.Account.Id == dto.AccountId && it.IsUsed )
                 .Select(it => new object[] { it});
            var infos = Repository.GetEntities<AgentEntity>(query);
            if (infos == null)
                return;
            dto.Agent = infos.FirstOrDefault();
        }
      
        /// <summary>
        /// 得到商品
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual void SetSettlementProductDtos(SettlementDto dto)
        {
            if (dto.Products == null || dto.Products.Count==0)
                return;
            var ids = dto.Products.Select(it => it.ProductId).ToArray();
            var query = new QueryInfo();
            query.Query<ProductEntity>()
                 .Where(it => ids.Contains(it.Id))
                 .Select(it => new object[]
                     {
                         it.Id, it.Name, it.Cost, it.Price,it.DepositRate,it.IsReturn,it.IsCustom,
                         it.Sku, it.IsSales, it.FileName, it.Count, it.OrderMinCount,it.OrderStepCount,it.Goods.PayTypes,
                         it.Goods.Account.Id,it.Goods.Account.RealName,it.Goods.Freight.Id,
                         it.Goods.Freight.FreeProfit,it.Goods.Freight.FreeRegion,
                         it.Goods.Freight.Carries.Select(s => new object[] {s.Id,s.Region,s.DefaultCount,s.DefaultPrice,s.ContinueCount,s.ContinuePrice})
                     });
            var infos = Repository.GetEntities<ProductEntity>(query);
            if (infos == null)
                return;
            var promotions = GetPromotions(infos.Select(it => it.Id).ToArray());
            if(promotions==null)
                return;
            foreach (var product in dto.Products)
            {
                product.Product = infos.FirstOrDefault(it => it.Id == product.ProductId);
                if (product.Product == null) continue;
                product.Product.Promotion = promotions.FirstOrDefault(it =>  it.Product != null && it.Product.Id == product.ProductId);
            }
        }


        /// <summary>
        /// 得到促销商品
        /// </summary>
        /// <param name="productIds"></param>
        protected virtual IList<PromotionEntity> GetPromotions(long[] productIds)
        {
            if (productIds == null)
                return null;
            var query = PromotionEntity.GetUsedQuery(productIds);
            query.Query<PromotionEntity>()
                 .Select(it => new object[]
                     {
                        it.Id,it.Product.Id,it.OrderLimitCount,it.Cities,it.PayTypes,it.Price
                     });
            return Repository.GetEntities<PromotionEntity>(query);
        }

 

        #endregion

        #region 创建订单

        private static readonly object CreateLock = new object();
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        public virtual void Generate(SettlementDto dto)
        {
            lock (CreateLock)
            {
                if (!CheckOrder(dto))
                    return;
                dto.Generate();
                if (dto.Orders == null || dto.Orders.Count == 0)
                    return ;
                var unitofworks = new List<IUnitofwork>();
                var rev = AddOrder( dto, unitofworks) &&
                          AddCoupon(dto.Coupon, dto, unitofworks);
                if (!rev) return ;
                rev = Winner.Creator.Get<IContext>().Commit(unitofworks);
                if (rev)
                {
                    Action<SettlementDto> action = RemoveShopcart;
                    action.BeginInvoke(dto, null, null);
                }
            }
        }
        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual void RemoveShopcart(SettlementDto dto)
        {
            var query = new QueryInfo();
            query.Query<ShopcartEntity>()
                .Where(
                    it =>
                        it.Account.Id == dto.AccountId &&
                        dto.Products.Select(s => s.ProductId).ToArray().Contains(it.Product.Id))
                .Select(it => it.Id);
            var infos = Repository.GetEntities<ShopcartEntity>(query);
            if (infos == null || infos.Count == 0)
                return;
            foreach (var info in infos)
            {
                info.SaveType = SaveType.Remove;
            }
            var unitofworks = ShopcartDomainService.Handle(infos);
            Winner.Creator.Get<IContext>().Commit(unitofworks);
        }

        /// <summary>
        /// 添加工作流
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="unitofworks"></param>
        /// <returns></returns>
        protected virtual bool AddOrder(SettlementDto dto, IList<IUnitofwork> unitofworks)
        {
            foreach (var order in dto.Orders)
            {
                var orderUnitofworks = OrderDomainService.Handle(order.Order);
                if (orderUnitofworks == null)
                {
                    dto.Errors = dto.Errors ?? new List<ErrorInfo>();
                    dto.Errors.AddList(order.Order.Errors);
                    return false;
                }
                unitofworks.AddList(orderUnitofworks);
                if (order.MerchantOrder != null)
                {
                    var merchantOrderUnitofworks = MerchantOrderDomainService.Handle(order.MerchantOrder);
                    if (merchantOrderUnitofworks == null)
                    {
                        dto.Errors = dto.Errors ?? new List<ErrorInfo>();
                        dto.Errors.AddList(order.Order.Errors);
                        return false;
                    }
                    unitofworks.AddList(merchantOrderUnitofworks);
                }
              
            }
            return true;
        }

        /// <summary>
        /// 添加优惠券
        /// </summary>
        /// <param name="coupon"></param>
        /// <param name="dto"></param>
        /// <param name="unitofworks"></param>
        /// <returns></returns>
        protected virtual bool AddCoupon(CouponEntity coupon, SettlementDto dto, IList<IUnitofwork> unitofworks)
        {
            if (coupon != null)
            {
                coupon.IsUsed = true;
                coupon.SaveType = SaveType.Modify;
                coupon.SetProperty(it => it.IsUsed);
                var units = CouponDomainService.Handle(coupon);
                if (units == null)
                {
                    dto.Errors = dto.Errors ?? new List<ErrorInfo>();
                    dto.Errors.AddList(coupon.Errors);
                    return false;
                }
                unitofworks.AddList(units);
            }
            return true;
        }

  

    
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual bool CheckOrder(SettlementDto dto)
        {
            if (dto.AddressId > 0 && dto.Address == null)
            {
                dto.AddError("NoAddress");
                return false;
            }
            if (dto.CouponId > 0 && dto.Coupon == null)
            {
                dto.AddError("NoCoupon");
                return false;
            }
            if (dto.InvoiceId > 0 && dto.Invoice == null)
            {
                dto.AddError("NoInvoice");
                return false;
            }
            if (dto.Products != null &&
                dto.Products.Count(it => it.ErrorMsg != null) > 0)
            {
                foreach (var settlementProduct in dto.Products)
                {
                    dto.AddError(settlementProduct.ErrorMsg);
                }
                return false;
            }
            if (dto.OrderType==OrderType.Custom && dto.Products != null && dto.Products.Count(it => it.Product != null && !it.Product.IsCustom) > 0)
            {
                dto.AddError("NotAllowCustomOrder");
                return false;
            }
            return CheckOrderLimitCount(dto) && CheckUnpayOrderCount(dto);
        }
        /// <summary>
        /// 检查未支付订单数量
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual bool CheckOrderLimitCount(SettlementDto dto)
        {
            if (dto.Products == null)
                return true;
            var promotions = dto.Products.Where(it =>it.Product!=null && it.Product.Promotion != null && it.Product.Promotion.OrderLimitCount > 0);
            var promotionIds = promotions.Select(it => it.Product.Promotion.Id).ToArray();
            if (promotionIds.Length == 0)
                return true;
            var query = new QueryInfo();
            query.Query<OrderProductEntity>()
                .Where(it => it.Order.Account.Id == dto.AccountId &&  it.IsCount && promotionIds.Contains(it.Promotion.Id))
                .Select(it => new object[] {it.Count,it.Promotion.Id});
            var infos = Repository.GetEntities<OrderProductEntity>(query);
            if (infos == null || infos.Count == 0)
                return true;
            if (promotions.Count(it =>
                infos.Where(s => s.Promotion != null && s.Promotion.Id == it.Product.Promotion.Id).Sum(s=>s.Count)+it.Count>it.OrderLimitCount)
                > 0)
            {
                dto.AddError("ProductOrderLimitCountError");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 检查未支付订单数量
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual bool CheckUnpayOrderCount(SettlementDto dto)
        {
            if (dto.Account != null)
                return true;
            var query=new QueryInfo();
            query.Query<OrderEntity>()
                .Where(it => it.Account.Id == dto.AccountId && it.PayAmount ==0)
                .Select(it => it.Id);
            Repository.GetEntities<OrderEntity>(query);
            var json = Configuration.ConfigurationManager.GetSetting<string>("Order").DeserializeJson<dynamic>();
            int unpayCount = json == null ? 5 : json.UnpayCount;
            if (query.DataCount >= unpayCount)
            {
                dto.AddError("UnpayCountOver", query.DataCount);
                return false;
            }
            return true;
        }

        #endregion




        #region 重新异常

   

        /// <summary>
        /// 重新异常
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool HandleException(Exception ex, OrderEntity info)
        {
            var rev = base.HandleException(ex, info);
            if (rev)
                return true;
            switch (ex.Message)
            {
                case "ProductCountNotEnough":
                    var hasErr = false;
                    if (info.OrderProducts != null)
                    {
                        foreach (var orderProduct in info.OrderProducts)
                        {
                            if (orderProduct.Product != null && orderProduct.Product.Errors != null &&
                                orderProduct.Product.Errors.Count > 0)
                            {
                                info.AddError("ProductCountNotEnough", orderProduct.Name);
                                hasErr = true;
                            }
                        }
                    }
                    if (!hasErr)
                        info.AddError("ProductCountNotEnough", "");
                    return true;
                default:
                    return false;
            }
        }
        #endregion

    }
}
