using System.Collections.Generic;
using Beeant.Application.Services.Search;
using Beeant.Domain.Services.Search;
using Dependent;
using Winner.Search;
using Winner.Search.Word;

namespace Beeant.Distributed.Service.Host.Service
{
    public class SearchService : ISearchContract
    {
        public virtual IList<WordInfo> QuickSearch(string indexName, string key)
        {
            key = key.Trim();
            if (string.IsNullOrEmpty(key))
                return null;
            return Ioc.Resolve<ISearchApplicationService>().QuickSearch(indexName, key);
        }

        public virtual SearchResultInfo Search(SearchQueryInfo searchQuery)
        {
            if (searchQuery == null)
                return null;
            searchQuery.Key = searchQuery.Key.Trim();
            if (string.IsNullOrEmpty(searchQuery.Key))
                return null;
            return Ioc.Resolve<ISearchApplicationService>().Search(searchQuery);
        }

        public virtual void SynchronousIndex(string fileName, byte[] fileByte)
        {
            Ioc.Resolve<ISearchApplicationService>().SynchronousIndex(fileName, fileByte);
        }

        public virtual void UpdateIndexCache(string indexName)
        {
            Ioc.Resolve<ISearchApplicationService>().UpdateIndexCache(indexName);
        }

        public virtual void RemoveIndex(string indexName)
        {
            Ioc.Resolve<ISearchApplicationService>().RemoveIndex(indexName);
        }
        static SearchService()
        {
            Winner.Creator.Get<Winner.Search.IIndexer>().InitlizeDocumentIndex();
        }
    }
}
