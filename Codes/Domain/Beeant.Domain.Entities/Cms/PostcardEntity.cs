namespace Beeant.Domain.Entities.Cms
{
    public class PostcardEntity:BaseEntity<PostcardEntity>
    {
        /// <summary>
        /// 账户信息
        /// </summary>
        public CmsEntity Cms { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 详细信息
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 二维码
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
        /// <summary>
        /// 是否显示
        /// </summary>
        public string IsShowName
        {
            get
            {
                return this.GetStatusName(IsShow);
                
            }
        }
    }
}
