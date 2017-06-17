using System;
using System.Collections.Generic;

namespace Winner.Search.Word
{
    [Serializable]
    public class InversionInfo
    {
        /// <summary>
        /// 文档Id
        /// </summary>
        public long DocumentId { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// 频率
        /// </summary>
        public IList<InversionFeildInfo> Feilds { get; set; }
    }
}
