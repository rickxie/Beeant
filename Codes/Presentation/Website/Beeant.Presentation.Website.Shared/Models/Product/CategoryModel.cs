using System.Collections.Generic;
using Beeant.Domain.Entities.Cms;
using Beeant.Domain.Entities.Product;

namespace Beeant.Presentation.Mvc.Shared.Models.Product
{
    public class CategoryModel
    {
        /// <summary>
        /// 类目
        /// </summary>
        public CategoryEntity Category { get; set; }
        /// <summary>
        /// 广告
        /// </summary>
        public IList<ContentEntity> Contents { get; set; } 

    }
}