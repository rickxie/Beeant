using System;
using System.Text;

namespace Beeant.Domain.Entities.Security
{
    [Serializable]
    public class LockerEntity : BaseEntity<LockerEntity>
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 锁住截止时间
        /// </summary>
        public DateTime LockTime { get; set; }
        /// <summary>
        /// 出错数量
        /// </summary>
        public int ErrorCount { get; set; }
        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsUsed { get; set; }

     
    }
}
