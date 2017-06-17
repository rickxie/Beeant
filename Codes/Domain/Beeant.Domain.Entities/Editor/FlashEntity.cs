using System;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;

namespace Beeant.Domain.Entities.Editor
{

  
    [Serializable]
    public class FlashEntity : BaseEntity<FlashEntity>
    {
        /// <summary>
        /// 文件夹
        /// </summary>
        public FolderEntity Folder { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }

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
