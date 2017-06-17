using System.Collections.Generic;

namespace Beeant.Presentation.Admin.Erp.Product.Goods
{
    public class ProductDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// SKU
        /// </summary>
        public string Sku { set; get; }
        /// <summary>
        /// 价格列表
        /// </summary>
        public List<PriceDto> Prices { set; get; }
    }

    
}