using System.Collections.Generic;
using Winner.Search.Analysis;
using Winner.Search.Document;

namespace Winner.Search
{
    public interface IIndexer
    {
        /// <summary>
        /// 存储信息
        /// </summary>
        IDictionary<string, StoreIndexInfo> StoreIndexs { get; set; }
        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="name"></param>
        void Begin(string name);
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="name"></param>
        /// <param name="documents"></param>
        void AddDocuments(string name, IList<DocumentInfo> documents);
        /// <summary>
        /// 添加完文档，创建索引
        /// </summary>
        /// <param name="name"></param>
        void End(string name);
        /// <summary>
        /// 删除词典
        /// </summary>
        /// <param name="name"></param>
        /// <param name="terms"></param>
        bool DeleteWords(string name, IList<TermInfo> terms);
        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="name"></param>
        void Delete(string name);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        SearchResultInfo Search(SearchQueryInfo searchQuery);
        /// <summary>
        /// 初始化主键
        /// </summary>
        void InitlizeDocumentIndex();
        /// <summary>
        /// 刷新缓存
        /// </summary>
        void Flush(string key);
    }
}
