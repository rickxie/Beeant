using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Gis;
using Beeant.Domain.Entities.Product;
using Winner;
using Winner.Filter;

namespace Beeant.Application.Dtos.Order
{
    public class OrderProductDto
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int Count { get; set; }


        #region 输出结果

        private ProductEntity _product;

        /// <summary>
        /// 产品
        /// </summary>
        public ProductEntity Product
        {
            get { return _product; }
            set
            {
                _product = value;
                _price = null;
            }
        }



        /// <summary>
        /// 是否销售
        /// </summary>
        public bool IsSales
        {
            get
            {
                return Product != null && Product.IsSales;
            }
        }
     

        private const decimal DefaultPrice = 100000;
        private decimal? _price;
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price
        {
            get
            {
                if (!_price.HasValue)
                    _price = Product?.GetOrderPrice() ?? DefaultPrice;
                return _price.Value;
            }
        }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal Cost
        {
            get
            {
                return Product == null ? DefaultPrice : Product.Cost;
            }
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal DiscountPrice
        {
            get
            {
                if (Product == null)
                    return 0;
                return Product.Price - Price;
            }
        }
        /// <summary>
        /// 起订量
        /// </summary>
        public int OrderMinCount
        {
            get
            {
                if (Product == null)
                    return 1;
                return Product.OrderMinCount;
            }
        }
        /// <summary>
        /// 步长
        /// </summary>
        public int OrderStepCount
        {
            get
            {
                if (Product == null)
                    return 1;
                return Product.OrderStepCount;
            }
        }
        /// <summary>
        /// 订单限购数
        /// </summary>
        public int OrderLimitCount
        {
            get
            {
                if (Product==null || Product.Promotion == null)
                    return 0;
                return Product.Promotion.OrderLimitCount;
            }
        }
  
        #region 运费

        /// <summary>
        /// 运费
        /// </summary>
        public FreightEntity Freight { get; set; }

        /// <summary>
        /// 得到运费
        /// </summary>
        /// <returns></returns>
        public virtual void SetFreight(Domain.Entities.Member.AddressEntity address, AreaEntity area)
        {
            if (Freight == null || Product == null || address == null)
            {
                return;
            }
            var freightCalculator = new FreightCalculatorEntity
            {
                Area=area,
                Province=address.Province,
                City=address.City,
                County = address.County,
                Price=Price,
                Cost=Cost,
                Count=Count
            };
            Freight.Set(freightCalculator);
        }
        #endregion
        private bool? _isGetErrorMsg;
        private string _errorMsg;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg
        {
            get
            {
                if (_isGetErrorMsg.HasValue)
                    return _errorMsg;
                _isGetErrorMsg = true;
                if (Product == null)
                {
                    _errorMsg= GetErrorMessage("NoProduct");
                    return _errorMsg;
                }
                if (Product.OrderMinCount > Count)
                {
                    _errorMsg= GetErrorMessage("ProductOrderMinCountError");
                    return _errorMsg;
                }
                if (Product.OrderStepCount>0 && Count % Product.OrderStepCount != 0)
                {
                    _errorMsg = GetErrorMessage("ProductOrderStepCountError");
                    return _errorMsg;
                }
                if (Product != null && Product.Promotion!=null && Product.Promotion.OrderLimitCount > 0 && Product.Promotion.OrderLimitCount < Count)
                {
                    _errorMsg= GetErrorMessage("ProductOrderLimitCountError");
                    return _errorMsg;
                }
                return null;
            }
        }
        /// <summary>
        /// 得到错误信息
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual string GetErrorMessage(string propertyName)
        {
            var error = Creator.Get<IValidation>().GetErrorInfo(typeof(SettlementDto).FullName, propertyName);
            return string.IsNullOrEmpty(error.Message) ? error.Key : error.Message;
        }
        #endregion
    }

}
