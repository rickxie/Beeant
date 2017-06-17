using System;

namespace Beeant.Domain.Entities.Basedata
{
    [Serializable]
    public class BrandEntity : BaseEntity<BrandEntity>
    {
        /// <summary>
        /// 中文名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 拼音首字母
        /// </summary>
        public string Initial { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool IsUsed { set; get; }

        public string Tag { set; get; }

        /// <summary>
        /// 状态名
        /// </summary>
        public string IsUsedName
        {
            get { return this.GetVerifyName(IsUsed); }
        }
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
