using System;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;

namespace Beeant.Domain.Entities.Editor
{

  
    [Serializable]
    public class FolderEntity : BaseEntity<FolderEntity>
    {
 
        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public FolderType Type { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string TypeName
        {
            get { return Type.GetName(); }
        }
    }
    
}
