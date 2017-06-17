using System;
using System.Collections.Generic;
using Beeant.Domain.Services.Search;
using Winner.Search;
using Winner.Search.Word;
using Winner.Wcf;

namespace Beeant.Repository.Services.Search
{
    public class RemoteSearchRepository : ISearchRepository
    {
        public IWcfService WcfService { get; set; }
        /// <summary>
        /// 快速搜索
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IList<WordInfo> QuickSearch(string indexName, string key)
        {
            try
            {
                var rev = WcfService.Invoke<ISearchContract>(QuickSearchIndex, indexName, key);
                if (rev == null) return null;
                return (IList<WordInfo>)rev;

            }
            catch (Exception ex)
            {
               Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return null;
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public virtual SearchResultInfo Search(SearchQueryInfo searchQuery)
        {
            try
            {
                var rev = WcfService.Invoke<ISearchContract>(SearchIndex, searchQuery);
                if (rev == null) return null;
                return (SearchResultInfo)rev;
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return null;
           
        }

        /// <summary>
        /// 同步搜索
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        public virtual void SynchronousIndex(string fileName, byte[] fileByte)
        {
            try
            {
                foreach (var endPoint in WcfService.EndPoints)
                {
                    WcfService.Invoke<ISearchContract>(new List<EndPointInfo> { endPoint }, SynchronousIndex, fileName, fileByte);
                }
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }

        }

        /// <summary>
        /// 更新搜索缓存
        /// </summary>
        /// <param name="indexName"></param>
        public virtual void UpdateIndexCache(string indexName)
        {
            try
            {
                foreach (var endPoint in WcfService.EndPoints)
                {
                    WcfService.Invoke<ISearchContract>(new List<EndPointInfo> { endPoint }, UpdateIndexCache, indexName);
                }

            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
          
        }
        /// <summary>
        /// 移除接口
        /// </summary>
        /// <param name="path"></param>
        public void RemoveIndex(string path)
        {
            try
            {
                WcfService.Invoke<ISearchContract>(RemoveIndex, path);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
           
        }


        /// <summary>
        /// 得到回收站数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object SynchronousIndex(ISearchContract service, params object[] paramters)
        {
            var fileName = paramters[0] as string;
            var fileByte = paramters[1] as byte[];
            if (string.IsNullOrEmpty(fileName) || fileByte==null) return null;
            service.SynchronousIndex(fileName,fileByte);
            return null;
        }

        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="service"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object UpdateIndexCache(ISearchContract service, params object[] paramters)
        {
            var indexName = paramters[0] as string;
            if (string.IsNullOrEmpty(indexName)) return null;
            service.UpdateIndexCache(indexName);
            return null;
        }
        /// <summary>
        /// 移除接口
        /// </summary>
        /// <param name="service"></param>
        /// <param name="paramters"></param>
        protected virtual object RemoveIndex(ISearchContract service, params object[] paramters)
        {
            var path = paramters[0] as string;
            service.RemoveIndex(path);
            return null;
        }
        /// <summary>
        /// 移除接口
        /// </summary>
        /// <param name="service"></param>
        /// <param name="paramters"></param>
        protected virtual object SearchIndex(ISearchContract service, params object[] paramters)
        {
            var searchQuery = paramters[0] as SearchQueryInfo;
            return service.Search(searchQuery);
        }

        /// <summary>
        /// 移除接口
        /// </summary>
        /// <param name="service"></param>
        /// <param name="paramters"></param>
        protected virtual object QuickSearchIndex(ISearchContract service, params object[] paramters)
        {

            return service.QuickSearch(paramters[0] as string, paramters[1] as string);
        }
    }
}
