using System.Collections.Generic;
using Beeant.Domain.Entities.Product;

namespace Beeant.Presentation.Mvc.B2c.Models.Product
{
    public class OrderPropertyModel
    {


        /// <summary>
        /// 属性
        /// </summary>
        public PropertyEntity Property { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public IList<OrderPropertyValueModel> Values { get; set; } 

    }
}