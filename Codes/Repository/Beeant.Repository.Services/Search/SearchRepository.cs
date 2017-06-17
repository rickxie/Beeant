using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Beeant.Domain.Services.Search;
using Winner;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Winner.Search;
using Winner.Search.Store;
using Winner.Search.Word;
using Winner.Storage;

namespace Beeant.Repository.Services.Search
{
    public class SearchRepository : ISearchRepository
    {

        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        public virtual void SynchronousIndex(string fileName, byte[] fileByte)
        {
            Creator.Get<IFile>("Winner.Storage.ILocalFile").Save(fileName, fileByte);
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="indexName"></param>
        public virtual void UpdateIndexCache(string indexName)
        {
            var path = string.Format("{0}\\", Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Creator.Get<IStorer>().Path));
            var sourceName = string.Format("{0}{1}", path, indexName);
            UpdateCache(path, sourceName);
        }

        /// <summary>
        /// 复制目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sourceName"></param>
        protected virtual void UpdateCache(string path, string sourceName)
        {
            string[] fileNames = Directory.GetFiles(sourceName);
            string[] directoryNames = Directory.GetDirectories(sourceName);
            foreach (string fileName in fileNames)
            {
                Creator.Get<IStorer>().Flush(fileName.Replace(path, ""));
            }
            if (directoryNames.Length == 0) return;
            foreach (string directoryName in directoryNames)
            {
                UpdateCache(path, directoryName);
            }
        }
        /// <summary>
        /// 移除接口
        /// </summary>
        /// <param name="path"></param>
        public virtual void RemoveIndex(string path)
        {
            path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, path);
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

       
     

        #region 搜索

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public virtual SearchResultInfo Search(SearchQueryInfo searchQuery)
        {
           // searchQuery.CecheTime = DateTime.Now.AddMinutes(2);
            return Creator.Get<IIndexer>().Search(searchQuery);

        }

   

        #endregion

        #region 快速搜索

        /// <summary>
        /// 快速搜索
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IList<WordInfo> QuickSearch(string indexName, string key)
        {
            var indexer = Creator.Get<IIndexer>();
            if (!indexer.StoreIndexs.ContainsKey(indexName)) 
                return null;
            var names = GetWordNames(key);
            var words = new List<WordInfo>();
            if (names != null)
            {
                var storeIndex = indexer.StoreIndexs[indexName];
                foreach (var name in names)
                {
                    var word = Creator.Get<IWorder>().GetInfo(storeIndex, name);
                    if(word==null)
                        continue;
                    words.Add(word);
                }
            }
            return words;
        }
        /// <summary>
        /// 得到数据库词
        /// </summary>
        /// <returns></returns>
        protected virtual IList<string> GetWordNames(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            var query = new QueryInfo();
            query.SetCacheTime(DateTime.Now.AddDays(1)).Query<Domain.Entities.Search.WordEntity>()
                 .Where(it => !it.IsForbid && it.Count>1).OrderBy(it => it.Name);
            var infos= Creator.Get<IContext>().GetInfos<IList<Domain.Entities.Search.WordEntity>>(query);
            if (infos == null)
                return null;
            return
                infos.Where(it => it.Name.StartsWith(key) || it.Name.EndsWith(key) || it.Pinyin.StartsWith(key) || it.Pinyin.EndsWith(key))
                     .OrderByDescending(it => it.Count)
                     .Take(5).Select(it=>it.Name)
                     .ToList();
        }
        #endregion
    }
}
