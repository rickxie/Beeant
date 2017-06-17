using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Agent;
using Beeant.Domain.Entities.Product;

namespace Beeant.Presentation.Mobile.Detail.Models.Home
{

    public class GoodsModel 
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        public long? ProductId { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public GoodsEntity Goods { get; set; }
        /// <summary>
        /// 代理
        /// </summary>
        public AgentEntity Agent { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public ProductEntity Product
        {
            get
            {
                if (Goods == null || Goods.Products == null)
                    return null;
                if (ProductId.HasValue)
                    return Goods.Products.FirstOrDefault(it => it.Id == ProductId.Value);
                return Goods.Products.FirstOrDefault();
            }
        }
        /// <summary>
        /// 下单属性
        /// </summary>
        public IList<SkuPropertyModel> SkuProperties { get; set; }
        /// <summary>
        /// 会员Id
        /// </summary>
        public long IdentityId { get; set; }
   

        /// <summary>
        /// 销售属性JSON
        /// </summary>
        public string ProductJson
        {
            get
            {
                if (Goods == null || Goods.Products == null)
                    return null;
                var list=new List<Dictionary<string, object>>();
                foreach (var info in Goods.Products)
                {
                    var dis = new Dictionary<string, object>();
                    dis.Add("Id", info.Id);
                    dis.Add("Count", info.Count);
                    dis.Add("FileName", info.GetFullFileName(info.FileName,"s"));
                    dis.Add("OrderStepCount", info.OrderStepCount);
                    dis.Add("OrderLimitCount",info.Promotion==null?0: info.Promotion.OrderLimitCount);
                    dis.Add("OrderMinCount", info.OrderMinCount);
                    dis.Add("Name", info.Name);
                    dis.Add("Price", info.Price);
                    dis.Add("Sku", info.Sku);
                    list.Add(dis);
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(list);
            }
        }
        /// <summary>
        /// 销售属性JSON
        /// </summary>
        public string SkuPropertyJson
        {
            get
            {
                if (SkuProperties == null)
                    return "[]";
                return Newtonsoft.Json.JsonConvert.SerializeObject(SkuProperties);
            }
        }
        /// <summary>
        /// 是否默认SKU
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public bool IsDefaultSku(long propertyId, string propertyValue)
        {
            if (Product == null || Product.SkuJsons == null)
                return false;
            return Product.SkuJsons.Count(it => it.Id == propertyId && it.Value == propertyValue) > 0;
        }

        /// <summary>
        /// 设置订单属性
        /// </summary>
        public virtual void SetSkuProperties()
        {
            if (Goods == null || Goods.Products == null) return;
            var products =
                Goods.Products.Where(it => !string.IsNullOrEmpty(it.Sku)).ToList();
            SkuProperties = new List<SkuPropertyModel>();
            foreach (var product in products)
            {
                foreach (var sku in product.SkuJsons)
                {
                    var skuProperty = SkuProperties.FirstOrDefault(it => it.Id == sku.Id);
                    if (skuProperty == null)
                    {
                        skuProperty = new SkuPropertyModel
                        {
                            Id = sku.Id,
                            Name = sku.Name,
                            Values = new List<string>()
                        };
                        SkuProperties.Add(skuProperty);
                    }
                    if (!skuProperty.Values.Contains(sku.Value))
                        skuProperty.Values.Add(sku.Value);
                }
            }
        }
        /// <summary>
        /// 设置订单属性
        /// </summary>
        public virtual void SetPrice()
        {
            if (Goods == null || Goods.Products == null || Agent==null) return;
            var products =
                Goods.Products.Where(it => !string.IsNullOrEmpty(it.Sku)).ToList();
            SkuProperties = new List<SkuPropertyModel>();
            foreach (var product in products)
            {
                product.Price = Agent.GetPrice(product.Price);
            }
        }
  
    }
}