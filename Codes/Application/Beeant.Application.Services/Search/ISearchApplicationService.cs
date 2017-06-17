using System.Collections.Generic;
using Winner.Search;
using Winner.Search.Word;


namespace Beeant.Application.Services.Search
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ISearchService”。

    public interface ISearchApplicationService
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        SearchResultInfo Search(SearchQueryInfo searchQuery);
    
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        IList<WordInfo> QuickSearch(string indexName, string key);
        /// <summary>
        /// 同步接口
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        void SynchronousIndex(string fileName, byte[] fileByte);

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <returns></returns>
        void UpdateIndexCache(string indexName);
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <returns></returns>
        void RemoveIndex(string indexName);
    }
}
