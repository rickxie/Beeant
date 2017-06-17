using System;

namespace Winner.Search
{
    /// <summary>
    /// 数据存储信息
    /// </summary>
   [Serializable]
    public class StoreDocumentInfo
    {
   
        /// <summary>
        /// 分页大小
        /// </summary>
        public virtual long PageSize { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public virtual long PageCount
        {
            get
            {
                return DataCount / PageSize;
            }
        }
        /// <summary>
        /// 当前数据量
        /// </summary>
        public virtual long DataCount { get; set; }
    }
  
}
