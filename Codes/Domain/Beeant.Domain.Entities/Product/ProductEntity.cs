using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Purchase;
using Beeant.Domain.Entities.Wms;
using Newtonsoft.Json;
using Beeant.Domain.Entities.Promotion;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public class ProductEntity : BaseEntity<ProductEntity>
    {
        /// <summary>
        /// 商品
        /// </summary>
        public GoodsEntity Goods { get; set; }
    
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 默认图
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 底价
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
   
        /// <summary>
        /// 最小订单数量
        /// </summary>
        public int OrderMinCount { get; set; }
        /// <summary>
        /// 增加步长数量
        /// </summary>
        public int OrderStepCount { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public int SalesCount { set; get; }
        /// <summary>
        /// 访问数量
        /// </summary>
        public int VisitCount { get; set; }
        /// <summary>
        /// 关注数量
        /// </summary>
        public int AttentionCount { get; set; }
        /// <summary>
        /// 销售属性
        /// </summary>
        public string Sku { get; set; }
        /// <summary>
        /// 是否销售
        /// </summary>
        public bool IsSales { get; set; }
        /// <summary>
        /// 外部编码
        /// </summary>
        public string DataId { get; set; }
        /// <summary>
        /// 定金比率
        /// </summary>
        public decimal DepositRate { get; set; }
        /// <summary>
        /// 是否定制
        /// </summary>
        public bool IsCustom { get; set; }
        /// <summary>
        /// 是否支持退货
        /// </summary>
        public bool IsReturn { get; set; }

        /// <summary>
        /// 是否销售
        /// </summary>
        public string IsSalesName
        {
            get {return this.GetSalesName(IsSales); }
        }
        /// <summary>
        /// 是否支持退货
        /// </summary>
        public string IsReturnName
        {
            get
            {
                return this.GetStatusName(IsReturn);
            }
        }
        /// <summary>
        /// 是否支持定制
        /// </summary>
        public string IsCustomName
        {
            get
            {
                return this.GetStatusName(IsCustom);
            }
        }

        /// <summary>
        /// 改变的数量
        /// </summary>
        public int ChangeCount { get; set; }

        /// <summary>
        /// 库存清单
        /// </summary>
        public IList<InventoryEntity> Inventories { get; set; }

        /// <summary>
        /// 出入库
        /// </summary>
        public IList<StockEntity> Stocks { get; set; }
        /// <summary>
        /// 采购
        /// </summary>
        public IList<PurchaseEntity> Purchases { get; set; }

        /// <summary>
        /// 商品属性
        /// </summary>
        public IList<GoodsPropertyEntity> GoodsProperties { get; set; }
        /// <summary>
        /// 订单描述
        /// </summary>
        public IList<GoodsDetailEntity> GoodsDetails { get; set; }

        /// <summary>
        /// 活动
        /// </summary>
        public IList<PromotionEntity> Promotions { get; set; }
        /// <summary>
        /// 当前活动
        /// </summary>
        public PromotionEntity Promotion { get; set; }
        /// <summary>
        /// 销售额
        /// </summary>
        public decimal Saleroom
        {
            get { return Price * SalesCount; }
        }
        /// <summary>
        /// 获取SKU名称
        /// </summary>
        public string SkuName
        {
            get
            {
                if (!string.IsNullOrEmpty(Sku))
                {
                    var skuList = SkuJsons;
                    if (skuList != null)
                        return string.Join(",", skuList.Select(it => string.Format("{0}:{1}", it.Name, it.Value)));
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// SKUJson
        /// </summary>
        public IList<SkuClass> SkuJsons
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<SkuClass>>(Sku);
                }
                catch 
                {
                  
                }
                return null;
            }
        }
        public class SkuClass
        {
            public long Id { get; set; }
            public string Name { set; get; }
            public string Value { set; get; }
        }
        /// <summary>
        /// 加载产品
        /// </summary>
        public ProductEntity DataEntity { get; set; }

    
        #region 业务处理

        /// <summary>
        /// 删除
        /// </summary>
        protected override void SetRemoveBusiness()
        {
            InvokeItemLoader("Inventories");
            if (Inventories != null)
            {
                foreach (var inventory in Inventories)
                {
                    inventory.SaveType = SaveType.Remove;
                }
            }
        }
        #endregion

        #region 业务规则
    




        /// <summary>
        /// 面价毛利率
        /// </summary>
        public virtual decimal PriceRate
        {
            get
            {
                if (Price == 0)
                {
                    return 0.00M;
                }
                return (Price - Cost) / Price;
            }
        }
        /// <summary>
        /// 得到下单金额
        /// </summary>
        /// <returns></returns>
        public virtual decimal GetOrderPrice()
        {
            if (Promotion != null)
            {
                return Promotion.Price;
            }
            return Price;
        }
        #endregion
    }
}
