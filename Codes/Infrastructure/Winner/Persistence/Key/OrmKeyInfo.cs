using System;

namespace Winner.Persistence.Key
{
    [Serializable]
    public class OrmKeyInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 恢复语句
        /// </summary>
        public string Recovery { get; set; }
        /// <summary>
        /// 前缀
        /// </summary>
        public virtual string Flag { get; set; }
        /// <summary>
        /// 当前数量
        /// </summary>
        public virtual int Count { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public virtual int RightLength { get; set; }
  
    }
}
