using System;

namespace Beeant.Domain.Entities.Cms
{


    [Serializable]
    public class ContentEntity :BaseEntity<ContentEntity>
    {
        /// <summary>
        /// 类别
        /// </summary>
        public ClassEntity Class { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        ///文件名
        /// </summary>
        
        public string FileName { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 连接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 是否展示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 是否展示名称
        /// </summary>
        public string IsShowName
        {
            get { return this.GetShowName(IsShow); }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
 
        /// <summary>
        ///文件名
        /// </summary>
        
        public string AttachmentName { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] AttachmentByte { get; set; }
        /// <summary>
        /// 全部文件路径
        /// </summary>
        public string DownAttachmentName
        {
            get { return this.GetDownLoadUrl(AttachmentName); }
        }
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] FileByte { get; set; }
        /// <summary>
        /// 全部文件路径
        /// </summary>
        public string FullFileName
        {
            get { return this.GetFullFileName(FileName); }
        }

    }
    
}
