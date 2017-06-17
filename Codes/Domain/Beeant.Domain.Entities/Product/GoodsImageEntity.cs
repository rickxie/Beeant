using System;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public class GoodsImageEntity : BaseEntity<GoodsImageEntity>
    {
        /// <summary>
        /// 商品
        /// </summary>
        public GoodsEntity Goods { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 销售属性
        /// </summary>
        public ProductEntity Product { get; set; }
        /// <summary>
        /// 图片名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 图片流
        /// </summary>
        public byte[] FileByte { get; set; }
        /// <summary>
        /// 图片全路径
        /// </summary>
        public string FullFileName
        {
            get { return this.GetFullFileName(FileName); }
        }
    }
}
