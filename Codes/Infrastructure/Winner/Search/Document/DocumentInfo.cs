using System;
using System.Collections.Generic;

namespace Winner.Search.Document
{
    [Serializable]
    public class DocumentInfo
    {
        /// <summary>
        /// 文档Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public IList<FeildInfo> Feilds { get; set; }
  
    }
}
