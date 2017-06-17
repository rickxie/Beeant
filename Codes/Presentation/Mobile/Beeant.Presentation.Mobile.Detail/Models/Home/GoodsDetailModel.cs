using System.Collections.Generic;
using Beeant.Domain.Entities.Product;

namespace Beeant.Presentation.Mobile.Detail.Models.Home
{
    public class GoodsDetailModel
    {

        /// <summary>
        /// 产品详情
        /// </summary>
        public GoodsDetailEntity GoodsDetail { get; set; }

        /// <summary>
        /// 产品属性
        /// </summary>
        public IList<GoodsPropertyEntity> GoodsProperties { get; set; }
    }
}