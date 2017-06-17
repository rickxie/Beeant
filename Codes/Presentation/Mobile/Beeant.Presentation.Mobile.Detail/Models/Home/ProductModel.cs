using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Product;

namespace Beeant.Presentation.Mobile.Detail.Models.Home
{

    public class ProductModel 
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public ProductEntity Home { get; set; }

      
    }
}