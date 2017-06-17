using System;
using Beeant.Domain.Entities.Product;

namespace Beeant.Domain.Entities.Merchant
{
    [Serializable]
    public class CatalogueGoodsEntity : BaseEntity<CatalogueGoodsEntity>
    {
        /// <summary>
        /// 父类
        /// </summary>
        public CatalogueEntity Catalogue { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public GoodsEntity Goods { get; set; }
    }

}
