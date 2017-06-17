using System;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;

namespace Beeant.Domain.Entities.Editor
{

  
    [Serializable]
    public class TemplateEntity : BaseEntity<TemplateEntity>
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
        /// 内容
        /// </summary>
        public string Detail { get; set; }
    }
    
}
