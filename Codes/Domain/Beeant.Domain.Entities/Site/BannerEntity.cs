using System;

namespace Beeant.Domain.Entities.Site
{
    [Serializable]
    public class BannerEntity : BaseEntity<BannerEntity>
    {
        /// <summary>
        /// 账户
        /// </summary>
        public SiteEntity Site { get; set; }

        /// <summary>
        /// 连接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
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
