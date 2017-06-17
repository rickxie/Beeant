using System.Collections.Generic;
using System.ServiceModel;
using Winner.Search;

namespace Beeant.Domain.Services.Search
{
    [ServiceContract(Namespace = "http://Beeant.Domain.Services.Search", ConfigurationName = "Beeant.Domain.Services.Search.ISearchContract")]
    public interface ISearchContract
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [OperationContract(
            Action = "http://Beeant.Domain.Services.Search.ISearchContract/QuickSearch",
            ReplyAction = "http://Beeant.Domain.Services.Search.ISearchContract/QuickSearchResponse")]
        IList<Winner.Search.Word.WordInfo> QuickSearch(string indexName, string key);

        [OperationContractAttribute(
           Action = "http://Beeant.Domain.Services.Search.ISearchContract/Search",
           ReplyAction = "http://Beeant.Domain.Services.Search.ISearchContract/SearchResponse")]
        SearchResultInfo Search(SearchQueryInfo searchQuery);
        /// <summary>
        /// 同步接口
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        [OperationContractAttribute(
            Action = "http://Beeant.Domain.Services.Search.ISearchContract/SynchronousIndex",
            ReplyAction = "http://Beeant.Domain.Services.Search.ISearchContract/SynchronousIndexResponse")]
        void SynchronousIndex(string fileName, byte[] fileByte);

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <returns></returns>
        [OperationContractAttribute(
           Action = "http://Beeant.Domain.Services.Search.ISearchContract/UpdateIndexCache",
           ReplyAction = "http://Beeant.Domain.Services.Search.ISearchContract/UpdateIndexCacheResponse")]
        void UpdateIndexCache(string indexName);
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <returns></returns>
        [OperationContractAttribute(
           Action = "http://Beeant.Domain.Services.Search.ISearchContract/RemoveIndex",
           ReplyAction = "http://Beeant.Domain.Services.Search.ISearchContract/RemoveIndexResponse")]
        void RemoveIndex(string indexName);
    }
}
