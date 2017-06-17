using System;

namespace Winner.Search
{
    
    /// <summary>
    /// 字段
    /// </summary>
    [Serializable]
    public class StoreFieldInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 存储类型
        /// </summary>
        public FieldIndexType StoreType { get; set; }
     
    }
   
}
