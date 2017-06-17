namespace Beeant.Presentation.Mvc.B2c.Models.Product
{
    public class OrderPropertyValueModel
    {
        public long GoodsId { get; set; }
        /// <summary>
        /// 是否为当前选项
        /// </summary>
        public bool IsCurrent { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
      
    }
}