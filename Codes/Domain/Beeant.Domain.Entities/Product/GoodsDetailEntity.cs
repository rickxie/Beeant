using System;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public class GoodsDetailEntity : BaseEntity<GoodsDetailEntity>
    {
        /// <summary>
        /// 商品
        /// </summary>
        public GoodsEntity Goods { get; set; }
        /// <summary>
        /// 销售属性
        /// </summary>
        public ProductEntity Product { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string Attachment { get; set; }
        /// <summary>
        /// 文件附件
        /// </summary>
        public byte[] AttachmentByte { get; set; }
        /// <summary>
        /// 文件下载
        /// </summary>

        public string DownAttachmentUrl
        {
            get { return this.GetDownLoadUrl(Attachment); }
        }

        /// <summary>
        /// 详情页
        /// </summary>
        public string Detail { get; set; }
    }
}
