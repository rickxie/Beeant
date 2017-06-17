using System.Collections.Generic;
using Beeant.Domain.Services.Search;
using Winner.Search;
using Winner.Search.Word;

namespace Beeant.Application.Services.Search
{
    public class SearchApplicationService : ISearchApplicationService
    {

        public ISearchRepository SearchRepository { get; set; }


        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public virtual SearchResultInfo Search(SearchQueryInfo searchQuery)
        {
           return SearchRepository.Search(searchQuery);
        }

        /// <summary>
        /// 快速搜索
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IList<WordInfo> QuickSearch(string indexName, string key)
        {
            return SearchRepository.QuickSearch(indexName, key);
        }
        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        public virtual void SynchronousIndex(string fileName, byte[] fileByte)
        {
            SearchRepository.SynchronousIndex(fileName, fileByte);
        }
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="indexName"></param>
        public virtual void UpdateIndexCache(string indexName)
        {
            SearchRepository.UpdateIndexCache(indexName);
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="indexName"></param>
        public virtual void RemoveIndex(string indexName)
        {
            SearchRepository.RemoveIndex(indexName);
        }
    }
}
