using System.Collections.Generic;
using Winner.Search.Analysis;
using Winner.Search.Document;
using Winner.Search.Word;

namespace Winner.Search
{
    public class SearchResultInfo
    {
        /// <summary>
        /// 拆分
        /// </summary>
        public IList<TermInfo> Terms { get; set; }
        /// <summary>
        /// 搜索出来的词
        /// </summary>
        public IList<WordInfo> Words { get; set; }
        /// <summary>
        /// 搜索出来的记录
        /// </summary>
        public IList<DocumentInfo> Documents { get; set; }
        ///<summary>
        /// 字段
        /// </summary>
        public IList<string> Feilds { get; set; }
        /// <summary>
        /// 文档总数
        /// </summary>
        public int DocumentCount { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int DataCount { get; set; }
    }
}
