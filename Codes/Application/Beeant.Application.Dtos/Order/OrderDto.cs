using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Merchant;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Promotion;
using Winner.Persistence;

namespace Beeant.Application.Dtos.Order
{
    #region 下单DT0

    /// <summary>
    /// 下单
    /// </summary>
    public class OrderDto
    {
     
        /// <summary>
        /// 产品数量
        /// </summary>
        public IList<OrderProductDto> Products { get; set; }
        /// <summary>
        /// 结算
        /// </summary>
        public SettlementDto Settlement { get; set; }
       
        #region 输出结果
        /// <summary>
        /// 支付方式
        /// </summary>
        public virtual string[] PayTypes { get; set; }
        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <returns></returns>
        public virtual void SetPayTypes()
        {
            if (Products == null)
                return ;
            var payarray = new List<string>();
            foreach (var product in Products)
            {
                if (product.Product == null)
                    continue;
                if (product.Product.Promotion != null &&
                    product.Product.Promotion.PayTypeArray != null)
                {
                    payarray.AddRange(product.Product.Promotion.PayTypeArray);

                }
                else if (product.Product.Goods != null && product.Product.Goods.PayTypeArray != null)
                {
                    payarray.AddRange(product.Product.Goods.PayTypeArray);
                }
            }
            PayTypes= payarray.Distinct().ToArray();
        }

        #region 价格计算

        private decimal? _productPrice;
        /// <summary>
        /// 产品总价
        /// </summary>
        public decimal ProductPrice
        {
            get
            {
                if (Products == null)
                    return 0;
                _productPrice = Products.Sum(it => it.Price);
                if (Settlement.Agent != null && Settlement.Agent != null)
                {
                    _productPrice =  Settlement.Agent.GetPrice(_productPrice.Value);
                    return _productPrice==null?decimal.MaxValue: _productPrice.Value;
                }
                return _productPrice.Value;
            }
        }

        /// <summary>
        /// 优惠价格
        /// </summary>
        public decimal DiscountPrice
        {
            get
            {
                if (Products != null)
                    return Products.Sum(it => it.DiscountPrice*it.Count);
                return decimal.MaxValue;
            }
        }
        /// <summary>
        /// 实际结算价格
        /// </summary>
        public decimal FactPrice
        {
            get
            {
                decimal price = int.MaxValue;
                if (Products != null)
                    price= Products.Sum(it => it.Price * it.Count);
                price = price + FreightPrice - CouponPrice;
                return price;
            }
        }

        /// <summary>
        /// 优惠券金额
        /// </summary>
        public decimal CouponPrice { get; set; }

        private decimal? _freightPrice;
        /// <summary>
        /// 运费价格
        /// </summary>
        public decimal FreightPrice
        {
            get
            {
                SetProductFreight();
                return _freightPrice.HasValue?_freightPrice.Value:0;
            }
            set { _freightPrice = value; }
        }
        private decimal? _freightCost;
        /// <summary>
        /// 运费价格
        /// </summary>
        public decimal FreightCost
        {
            get
            {
                SetProductFreight();
                return _freightCost.HasValue ? _freightCost.Value : 0;
            }
            set { _freightCost = value; }
        }


       
        /// <summary>
        /// 设置运费
        /// </summary>
        protected virtual void SetProductFreight()
        {
            if(_freightPrice.HasValue)
                return;
            foreach (var product in Products)
            {
                product.SetFreight(Settlement.Address, Settlement.Area);
            }
            FreightPrice = Products.Where(it => it.Freight != null).Sum(it=>it.Freight.Price);
            FreightCost = Products.Where(it => it.Freight != null).Sum(it => it.Freight.Cost);
        }
   
        /// <summary>
        /// 
        /// </summary>
        public AccountEntity SaleAccount { get; set; }
        #endregion

        #region 创建订单
        /// <summary>
        /// 创建的订单
        /// </summary>
        public OrderEntity Order { get; set; }
        /// <summary>
        /// 商家订单
        /// </summary>
        public MerchantOrderEntity MerchantOrder { get; set; }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        public virtual void Create()
        {
            var order = new OrderEntity
            {
                ChannelType = Settlement.ChannelType,
                Type = Settlement.OrderType,
                Account = new AccountEntity { Id = Settlement.AccountId },
                OrderDate = DateTime.Now,
                Status = OrderStatusType.WaitPay,
                PayTypes = PayTypes==null?"":string.Join(",",PayTypes),
                Remark = "",
                RouteId="",
                Variables = "",
                SaveType = SaveType.Add
            };
            FillOrderProduct(order);
            FillExpressOrderItem(order);
            FillCouponOrderItem(order);
            FillOrderInsurance(order);
            FillOrderInsuranceAttachments(order);
            if (order.Type == OrderType.Custom)
                order.Status = OrderStatusType.WaitHandle;
            FillExpress(order);
            Order = order;
            CreateMerchantOrder(order);
        }

        /// <summary>
        /// 创建商家订单
        /// </summary>
        protected virtual void CreateMerchantOrder(OrderEntity order)
        {
            if (SaleAccount == null || SaleAccount.Id <= 0) return;
            MerchantOrder = new MerchantOrderEntity
            {
                Account = new AccountEntity {Id = SaleAccount.Id},
                Order = order,
                SaveType = SaveType.Add
            };
            order.OrderNumbers.Add(new OrderNumberEntity
            {
                Name = "SaleAccountId",
                Number = SaleAccount.Id.ToString(),
                SaveType = SaveType.Add
            });
        }

   


        #region 填充产品明细
        /// <summary>
        /// 填充订单明细
        /// </summary>
        /// <param name="order"></param>
        protected virtual void FillOrderProduct(OrderEntity order)
        {
            order.OrderProducts = new List<OrderProductEntity>();
            if (Products != null)
            {
                foreach (var product in Products)
                {
                    if (product.Product == null)
                        continue;
                    var orderProduct = new OrderProductEntity
                    {
                        Order = order,
                        Count = product.Count,
                        Price = product.Price,
                        Name = product.Product.Name,
                        Product = new ProductEntity { Id = product.Product.Id },
                        Promotion = new PromotionEntity { Id = product.Product.Promotion == null ? 0 : product.Product.Promotion.Id },
                        IsReturn = product.Product.IsReturn,
                        FileName = "",
                        Remark = "",
                        IsInvoice = true,
                        Description = product.Product.GoodsDetails == null ? "" : product.Product.GoodsDetails.FirstOrDefault(it=>it.Name== "PackageDescription")?.Detail,
                        SaveType = SaveType.Add
                    };
                    order.OrderProducts.Add(orderProduct);
                    if (product.Product.DepositRate > 0)
                    {
                        order.Deposit = order.OrderProducts.Sum(it => it.Count * it.Price) * product.Product.DepositRate;
                    }
                }
            }


        }


        #endregion

        #region 填充优惠券明细
        /// <summary>
        /// 填充优惠券明细
        /// </summary>
        /// <param name="order"></param>
        protected virtual void FillCouponOrderItem(OrderEntity order)
        {
            if (CouponPrice > 0)
            {
                order.OrderItems.Add(new OrderItemEntity
                {
                    Order = order,
                    Count = 1,
                    Price = 0 - CouponPrice,
                    Name = "优惠券",
                    Cost = 0,
                    InvoiceAmount = 0,
                    SaveType = SaveType.Add
                });
            }
        }
        #endregion

        #region 填充运费明细
        /// <summary>
        /// 填充优惠券明细
        /// </summary>
        /// <param name="order"></param>
        protected virtual void FillExpressOrderItem(OrderEntity order)
        {
            if (FreightPrice > 0)
            {
                order.OrderItems.Add(new OrderItemEntity
                {
                    Order = order,
                    Count = 1,
                    Price = FreightPrice,
                    Cost = FreightCost,
                    InvoiceAmount=0,
                    Name = "运费",
                    SaveType = SaveType.Add
                });
            }
        }
        #endregion

        #region 填充快递
        /// <summary>
        /// 填充运费明细
        /// </summary>
        /// <param name="order"></param>
        protected virtual void FillExpress(OrderEntity order)
        {
            if (Settlement == null || Settlement.Address == null)
            {
                return;
            }
            order.OrderExpresses = order.OrderExpresses ?? new List<OrderExpressEntity>();
            order.OrderExpresses.Add(new OrderExpressEntity
            {
                Order=order,
                Address=string.Format("{0}{1}{2}{3}",Settlement.Address.Province,Settlement.Address.City,Settlement.Address.County, Settlement.Address.Address),
                Mobile=Settlement.Address.Mobile,
                Recipient=Settlement.Address.Recipient,
                Postcode=Settlement.Address.Postcode,
                Email=Settlement.Address.Email,
                DeliveryDate = DateTime.Now.AddHours(12),
                Name ="",
                Number="",
                Remark = "",
                SaveType = SaveType.Add
            });
            
        }

        #endregion

        #region 填充保险明细
        /// <summary>
        /// 填充保险明细
        /// </summary>
        /// <param name="order"></param>
        protected virtual void FillOrderInsurance(OrderEntity order)
        {
            order.OrderInsurances = new List<OrderInsuranceEntity>();
            if (Settlement != null && Settlement.Insurances != null &&
                Settlement.Insurances.Count > 0)
            {
                foreach (var insurance in Settlement.Insurances)
                {
                    var orderInsurance = new OrderInsuranceEntity
                    {
                        Order = order,
                        Product = new ProductEntity {Id = insurance.ProductId},
                        Name = insurance.Name,
                        Relation=insurance.Relation,
                        Birthday = insurance.Birthday,
                        Gender = insurance.Gender,
                        Country = insurance.Country,
                        IdCardNumber = insurance.IdCardNumber,
                        MedicalHistory = insurance.MedicalHistory,
                        SaveType = SaveType.Add
                    };
                    order.OrderInsurances.Add(orderInsurance);

                }
            }
        }

        /// <summary>
        /// 填充保险明细
        /// </summary>
        /// <param name="order"></param>
        protected virtual void FillOrderInsuranceAttachments(OrderEntity order)
        {
       
            if (Settlement != null && Settlement.Insurances != null &&
                Settlement.Insurances.Count > 0)
            {
                order.OrderAttachments = order.OrderAttachments ?? new List<OrderAttachmentEntity>();
                foreach (var insurance in Settlement.Insurances)
                {
                    if(string.IsNullOrWhiteSpace(insurance.AttachmentFileName) || insurance.AttachmentFileByte==null)
                        continue;
                    order.OrderAttachments.Add(new OrderAttachmentEntity
                    {
                        Order = order,
                        Name = insurance.AttachmentName,
                        FileName = string.Format("Files/Documents/OrderAttachment/{0}", insurance.AttachmentFileName),
                        FileByte = insurance.AttachmentFileByte,
                        SaveType = SaveType.Add
                    });

                }
            }
        }
        #endregion
        #endregion
        #endregion
    }
    #endregion


}
