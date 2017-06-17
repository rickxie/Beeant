using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Agent;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Gis;
using Beeant.Domain.Entities.Member;
using Beeant.Domain.Entities.Order;
using Winner;
using Winner.Filter;
using AddressEntity = Beeant.Domain.Entities.Member.AddressEntity;

namespace Beeant.Application.Dtos.Order
{
    #region 下单DT0

    /// <summary>
    /// 下单
    /// </summary>
    public class SettlementDto
    {
        /// <summary>
        /// 订单来源
        /// </summary>
        public ChannelType ChannelType { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 选择的地址
        /// </summary>
        public long AddressId { get; set; }

        /// <summary>
        /// 选择的优惠券编号
        /// </summary>
        public long CouponId { get; set; }

        /// <summary>
        /// 选择的发票
        /// </summary>
        public long InvoiceId { get; set; }

        /// <summary>
        /// 账户编号
        /// </summary>
        public long AccountId { get; set; }
        /// <summary>
        /// 是否区域
        /// </summary>
        public bool IsArea { get; set; }
        /// <summary>
        /// 是否生成订单
        /// </summary>
        public bool IsGenerate { get; set; }
        /// <summary>
        /// 产品数量
        /// </summary>
        public IList<OrderProductDto> Products { get; set; }
        /// <summary>
        /// 保险人员
        /// </summary>
        public IList<OrderInsuranceDto> Insurances { get; set; }
  
        #region 输出结果
        /// <summary>
        /// 会员地址
        /// </summary>
        public AddressEntity Address { get; set; }
        /// <summary>
        /// 发票
        /// </summary>
        public InvoiceEntity Invoice { get; set; }
        /// <summary>
        /// 优惠券
        /// </summary>
        public CouponEntity Coupon { get; set; }

        /// <summary>
        /// dialin
        /// </summary>
        public AgentEntity Agent { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 区域 
        /// </summary>
        public AreaEntity Area { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public IList<PayTypeEntity> PayTypes { get; set; }



        /// <summary>
        /// 总计
        /// </summary>
        public decimal FactPrice
        {
            get
            {
                if (Orders == null)
                    return 0;
                return Orders.Sum(it => it.FactPrice);
            }
        }
        /// <summary>
        /// 总计
        /// </summary>
        public decimal FreightPrice
        {
            get
            {
                if (Orders == null)
                    return 0;
                return Orders.Sum(it => it.FreightPrice);
            }
        }
        /// <summary>
        /// 总计
        /// </summary>
        public decimal DiscountPrice
        {
            get
            {
                if (Orders == null)
                    return 0;
                return Orders.Sum(it => it.DiscountPrice);
            }
        }
        /// <summary>
        /// 总计
        /// </summary>
        public decimal ProductPrice
        {
            get
            {
                if (Orders == null)
                    return 0;
                return Orders.Sum(it => it.ProductPrice);
            }
        }
        /// <summary>
        /// 优化
        /// </summary>
        public decimal CouponPrice
        {
            get
            {
                if (Coupon == null)
                    return 0;
                return Coupon.Amount;
            }
        }

        /// <summary>
        /// 得到支付方式
        /// </summary>
        /// <returns></returns>
        protected virtual void SetPayTypes()
        {
            if (Orders == null || PayTypes==null || PayTypes.Count==0)
                return ;
            PayTypes = PayTypes.Where(it => Orders.All(s=>s.PayTypes==null || s.PayTypes.Length==0 || s.PayTypes.Contains(it.Name))).ToList();
            foreach (var payType in PayTypes)
            {
               payType.SetUrl();
            }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public IList<ErrorInfo> Errors { get; set; }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="paramters"></param>
        public virtual void AddError(string propertyName, params object[] paramters)
        {
            Errors = Errors ?? new List<ErrorInfo>();
            var error = Creator.Get<IValidation>().GetErrorInfo(typeof(SettlementDto).FullName, propertyName);
            error.Message = string.Format(error.Message, paramters);
            Errors.Add(error);
        }

        /// <summary>
        /// 是保存成功
        /// </summary>
        public bool IsSaveSuccess
        {
            get { return Errors == null || Errors.Count == 0; }
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        public IList<OrderDto> Orders { get; set; }
        /// <summary>
        /// 创建Dto
        /// </summary>
        public virtual void Create()
        {
            if(Products==null)
                return;
            var orderGroups = new Dictionary<long, OrderDto>();
            foreach (var product in Products)
            {
                if (product.Product == null || product.Product.Goods == null || product.Product.Goods.Account == null)
                    continue;
                if (!orderGroups.ContainsKey(product.Product.Goods.Account.Id))
                {
                    var order = new OrderDto { Settlement = this,SaleAccount= product.Product.Goods.Account, Products = new List<OrderProductDto>() };
                    order.SetPayTypes();
                    orderGroups.Add(product.Product.Goods.Account.Id, order);
                }
                orderGroups[product.Product.Goods.Account.Id].Products.Add(product);
            }
            Orders = orderGroups.Values.ToList();
            SetCoupon();
            SetPayTypes();
        }
  
       
        /// <summary>
        /// 设置优惠
        /// </summary>
        protected virtual void SetCoupon()
        {
            if (Coupon == null) return;
            var order =
                Orders.OrderByDescending(it => it.ProductPrice)
                    .FirstOrDefault(it => it.ProductPrice >= Coupon.Amount);
            if (order != null)
                order.CouponPrice = Coupon.Amount;
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Generate()
        {
            foreach (var orderDto in Orders)
            {
                orderDto.Create();
            }
        }

        #endregion

       
    }

    #endregion


}
