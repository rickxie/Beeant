namespace Beeant.Presentation.Admin.Erp.Product.Goods
{
    

    public class PriceDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 会员等级
        /// </summary>
        public int Grade { set; get; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { set; get; }
    }
}