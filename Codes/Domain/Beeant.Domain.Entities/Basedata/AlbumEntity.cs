using System;

namespace Beeant.Domain.Entities.Basedata
{
    [Serializable]
    public class AlbumEntity : BaseEntity<AlbumEntity>
    {
        /// <summary>
        /// 中文名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 音乐地址
        /// </summary>
        public string MusicUrl { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        ///文件名
        /// </summary>
        public string FrontFileName { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] FrontFileByte { get; set; }
        /// <summary>
        /// 全部文件路径
        /// </summary>
        public string FullFrontFileName
        {
            get { return this.GetFullFileName(FrontFileName); }
        }
        /// <summary>
        ///文件名
        /// </summary>
        
        public string BackFileName { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] BackFileByte { get; set; }
        /// <summary>
        /// 全部文件路径
        /// </summary>
        public string FullBackFileName
        {
            get { return this.GetFullFileName(BackFileName); }
        }
        /// <summary>
        ///文件名
        /// </summary>
        
        public string AboutFileName { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] AboutFileByte { get; set; }
        /// <summary>
        /// 全部文件路径
        /// </summary>
        public string FullAboutFileName
        {
            get { return this.GetFullFileName(AboutFileName); }
        }
    }
}
