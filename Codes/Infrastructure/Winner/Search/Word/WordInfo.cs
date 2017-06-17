using System;
using System.Collections.Generic;

namespace Winner.Search.Word
{
    [Serializable]
    public class WordInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 拥有文章总数
        /// </summary>
        public int DocumentCount { get; set; }
        /// <summary>
        /// 链表
        /// </summary>
        public IList<InversionInfo> Inversions { get; set; }
    }
}
