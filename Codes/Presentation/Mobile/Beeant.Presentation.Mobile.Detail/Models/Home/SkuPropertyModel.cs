using System.Collections.Generic;

namespace Beeant.Presentation.Mobile.Detail.Models.Home
{
    public class SkuPropertyModel
    {
        /// <summary>
        /// 属性
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public IList<string> Values { get; set; } 

    }
}