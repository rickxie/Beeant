using System;

namespace Beeant.Domain.Entities.Purchase
{
    [Serializable]
    public class PurchaseAttachmentEntity : BaseEntity<PurchaseAttachmentEntity>
    {
        /// <summary>
        /// 总订单标识Id
        /// </summary>
        public PurchaseEntity Purchase { get; set; }
 
        /// <summary>
        /// 附件名称（标题）
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 附件文件流
        /// </summary>
        public byte[] FileByte { get; set; }
        /// <summary>
        /// 得到文件全路径
        /// </summary>
        public string DownFileName
        {
            get { return this.GetDownLoadUrl(FileName); }
        }

 
   
    }
}
