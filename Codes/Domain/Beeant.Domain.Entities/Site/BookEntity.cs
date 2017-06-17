using System;

namespace Beeant.Domain.Entities.Site
{
    [Serializable]
    public class BookEntity : BaseEntity<BookEntity>
    {
        /// <summary>
        /// 账户
        /// </summary>
        public SiteEntity Site { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        ///文件名
        /// </summary>
        public string FileName { get; set; }
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
