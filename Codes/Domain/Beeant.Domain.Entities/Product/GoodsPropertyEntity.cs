using System;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public class GoodsPropertyEntity : BaseEntity<GoodsPropertyEntity>
    {
        /// <summary>
        /// 商品
        /// </summary>
        public GoodsEntity Goods { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public ProductEntity Product { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public PropertyEntity Property { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
       
       
    }
}
