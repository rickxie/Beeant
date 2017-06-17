using System;

namespace Beeant.Domain.Entities.Basedata
{
    [Serializable]
    public class TagEntity : BaseEntity<TagEntity>
    {
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public TagGroupEntity TagGroup { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 文件名
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
