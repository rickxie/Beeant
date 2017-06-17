using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Winner.Search.Analysis;
using Winner.Cache;
using Winner.Search.Document;
using Winner.Search.Word;

namespace Winner.Search
{
    public class Indexer : IIndexer
    {
        #region 属性
    
        /// <summary>
        /// 分词实例
        /// </summary>
        public IAnalyzer Analyzer { get; set; }
        /// <summary>
        /// 文档
        /// </summary>
        public IDocumentor Documentor { get; set; }
        /// <summary>
        /// 词典实例
        /// </summary>
        public IWorder Worder { get; set; }
        /// <summary>
        /// 缓存
        /// </summary>
        public ICache Cacher { get; set; }
        /// <summary>
        /// 存储信息
        /// </summary>
        public IDictionary<string,StoreIndexInfo> StoreIndexs { get; set; }

        /// <summary>
        /// 缓存前缀
        /// </summary>
        public virtual string CacheTag
        {
            get { return "Winner_SearchQuery_"; }
        }

        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public Indexer()
        {
         
        }

        /// <summary>
        /// 拆分字符串,词库实例,拼音实例
        /// </summary>
        /// <param name="analyzer"></param>
        /// <param name="documentor"></param>
        /// <param name="worder"></param>
        /// <param name="cacher"></param>
        /// <param name="storeIndexs"></param>
        public Indexer(IAnalyzer analyzer, IDocumentor documentor, IWorder worder, ICache cacher, IDictionary<string, StoreIndexInfo> storeIndexs)
        {
  
            Analyzer = analyzer;
            StoreIndexs = storeIndexs;
            Documentor = documentor;
            Worder = worder;
            Cacher = Cacher;
        }
        #endregion

        #region 接口实现


        #region 创建索引
        /// <summary>
        /// 开始索引
        /// </summary>
        /// <param name="name"></param>
        public virtual void Begin(string name)
        {
            if(!StoreIndexs.ContainsKey(name))
                return;
            var storeIndex = StoreIndexs[name];
            if (storeIndex == null)
                return;
            Worder.Load(storeIndex);
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="name"></param>
        /// <param name="documents"></param>
        public virtual void AddDocuments(string name, IList<DocumentInfo> documents)
        {
            if (!StoreIndexs.ContainsKey(name))
                return;
            var storeIndex = StoreIndexs[name];
            if (storeIndex == null)
                return;
            foreach (var document in documents)
            {
                var feildTerms = SaveDocument(storeIndex, document);
                CreateWords(storeIndex, document,feildTerms);
            }
     
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="name"></param>
        public virtual void End(string name)
        {
            if (!StoreIndexs.ContainsKey(name))
                return;
            var storeIndex = StoreIndexs[name];
            if (storeIndex == null)
                return;
            Worder.Write(storeIndex);
        }

        #region 保存文档

        /// <summary>
        /// 保存文档
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        protected virtual IDictionary<int, IList<TermInfo>> SaveDocument(StoreIndexInfo storeIndex, DocumentInfo document)
        {
            var feildTerms = new Dictionary<int, IList<TermInfo>>();
            var i = 0;
            foreach (var feild in document.Feilds)
            {
                var storeFeild = storeIndex.GetStoreField(i);
                if (storeFeild != null && storeFeild.StoreType != FieldIndexType.OnlyStore)
                {
                    var terms = Analyzer.Resolve(feild.Text);
                    feildTerms.Add(i,terms??new List<TermInfo>());
                }
                if (storeFeild != null && storeFeild.StoreType == FieldIndexType.OnlyIndex)
                {
                    document.Feilds[i] = null;
                }
                i++;
            }
            Documentor.Insert(storeIndex, document);
            return feildTerms;
        }
        #endregion

        #region 创建词典

        /// <summary>
        /// 创建词典
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="document"></param>
        /// <param name="feildTerms"></param>
        protected virtual void CreateWords(StoreIndexInfo storeIndex,DocumentInfo document,IDictionary<int, IList<TermInfo>> feildTerms)
        {
            if (feildTerms == null || document==null)
                return ;
            var wordKeys = new Dictionary<string, WordInfo>();
            var words=new List<WordInfo>();
            foreach (var feildTerm in feildTerms)
            {
                var groupTems = from p in feildTerm.Value
                                group p by p.Name
                                into g
                                select new
                                    {
                                        g.Key,
                                        Frequency = (double)g.Count() / (feildTerm.Value.Count == 0 ? 1 : feildTerm.Value.Count)
                                    };
                foreach (var groupTem in groupTems)
                {
                    WordInfo word;
                    if (!wordKeys.ContainsKey(groupTem.Key))
                    {
                        word = new WordInfo
                        {
                            Name = groupTem.Key,
                            DocumentCount = 1,
                            Inversions =
                                new List<InversionInfo>
                                        {
                                            new InversionInfo
                                                {
                                                    DocumentId = document.Id,
                                                    Feilds = new List<InversionFeildInfo>()
                                                }
                                        }
                        };
                        words.Add(word);
                        wordKeys.Add(groupTem.Key,word);
                    }
                    else
                    {
                        word = wordKeys[groupTem.Key];
                    }
                    var inversion = word.Inversions.First();
                    var feild = new InversionFeildInfo { FeildIndex = feildTerm.Key, Frequency = groupTem.Frequency };
                    inversion.Feilds.Add(feild);
                }
      
            }
            InsertWords(storeIndex,words);
        }

        /// <summary>
        /// 保存索引
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="words"></param>
        protected virtual void InsertWords(StoreIndexInfo storeIndex, IList<WordInfo> words)
        {
            foreach (var word in words)
            {
                Worder.Insert(storeIndex, word);
            }
        }


        #endregion
        #endregion

        #region 搜索
        /// <summary>
        /// 缓存锁
        /// </summary>
        static public readonly object KeyLocker = new object();
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public virtual SearchResultInfo Search(SearchQueryInfo searchQuery)
        {
            if (searchQuery == null || string.IsNullOrEmpty(searchQuery.Name) || string.IsNullOrEmpty(searchQuery.Key.Trim()))
                return null;
            searchQuery.Key = searchQuery.Key.Trim().ToLower();
            if (searchQuery.TimeSpan != 0 || searchQuery.CecheTime!=null)
                return GetSearchResultByCache(searchQuery);
            return GetSearchResultByFind(searchQuery);
        }


        /// <summary>
        /// 得到缓存数据
        /// </summary>
        /// <param name="searchQuery"></param>
        protected virtual SearchResultInfo GetSearchResultByCache(SearchQueryInfo searchQuery)
        {
            var name =string.Format("{0}_{1}_{2}", searchQuery.Key, searchQuery.PageIndex, searchQuery.PageSize) ;
            var builder = new StringBuilder(name);
            if (searchQuery.Conditions != null)
            {
                foreach (var condition in searchQuery.Conditions)
                {
                    builder.AppendFormat("{0}{1}", condition.Key, condition.Value);
                }
            }
            searchQuery.CacheKey = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(builder.ToString(), "MD5");
            searchQuery.CacheKey = string.Format("{0}{1}_{2}_{3}", CacheTag,searchQuery.Name,searchQuery.Key, searchQuery.CacheKey);
            var result = Cacher.Get<SearchResultInfo>(searchQuery.CacheKey);
            lock (KeyLocker)
            {
                if (result == null)
                {
                    result = GetSearchResultByFind(searchQuery);
                    if (searchQuery.TimeSpan > 0)
                    {
                        Cacher.Set(searchQuery.CacheKey, result, searchQuery.TimeSpan); 
                    }
                    else if(searchQuery.CecheTime.HasValue)
                    {
                        Cacher.Set(searchQuery.CacheKey, result, searchQuery.CecheTime.Value);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 得到结果
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        protected virtual SearchResultInfo GetSearchResultByFind(SearchQueryInfo searchQuery)
        {
            var result = new SearchResultInfo {Terms = Analyzer.Resolve(searchQuery.Key)};
            if (result.Terms == null)
                return result;
            if (!StoreIndexs.ContainsKey(searchQuery.Name))
                return result;
            var storeIndex = StoreIndexs[searchQuery.Name];
            if (storeIndex == null)
                return result;
            result.Feilds = storeIndex.StoreFields.Select(it => it.Name).ToList();
            result.Words = new List<WordInfo>();
            foreach (var term in result.Terms)
            {
                if (result.Words.Count(it => it.Name == term.Name) > 0)
                    continue;
                var word = Worder.GetInfo(storeIndex, term.Name);
                if (word != null)
                    result.Words.Add(word);
            }
            var documentIds = GetSearchDocumentIds(storeIndex, result, searchQuery);
            if (documentIds != null)
            {
                result.DocumentCount = documentIds.Count;
                AddSearchDocuments(storeIndex, searchQuery, result, documentIds);
            }
            if (result.Words.Count > 0)
                result.DataCount = result.Words.Sum(it => it.DocumentCount)/result.Words.Count;
            return result;
        }

        #region 得到文档编号
        /// <summary>
        /// 查询得到结果
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="result"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        protected virtual IList<long> GetSearchDocumentIds(StoreIndexInfo storeIndex, SearchResultInfo result, SearchQueryInfo searchQuery)
        {
            if (result.Words == null || result.Words.Count == 0)
                return null;
            result.Documents = new List<DocumentInfo>();
            if (result.Words.Count == 1)
            {
                return result.Words[0].Inversions.Select(it => it.DocumentId).ToList();
            }
            var scQuery = new List<double>();
            foreach (var word in result.Words)
            {
                var weight = word.DocumentCount == 0
                                 ? 0
                                 : ((double)result.Terms.Count(it => it.Name == word.Name) / result.Terms.Count *
                                   Math.Log(storeIndex.StoreDocument.DataCount / (word.DocumentCount / (double)storeIndex.StoreDocument.DataCount)));
                scQuery.Add(weight);
            }
            var scDocuments = GetSeachDocumentVector(result);
            return GetOrderbySearchDocuments(scQuery, scDocuments);
        }
        /// <summary>
        /// 得到文档向量
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual IDictionary<long, IList<double>> GetSeachDocumentVector(SearchResultInfo result)
        {
            var scDocs = new Dictionary<long, IList<double>>();
            var i = 0;
            foreach (var word in result.Words)
            {
                foreach (var inversion in word.Inversions)
                {
                    if (!scDocs.ContainsKey(inversion.DocumentId))
                    {
                        var values =new double[result.Words.Count];
                        scDocs.Add(inversion.DocumentId, values);
                    }
                    scDocs[inversion.DocumentId][i] = inversion.Weight;
                }
                i++;
            }
            return scDocs;
        }
        /// <summary>
        /// 排序文档
        /// </summary>
        /// <param name="scQuery"></param>
        /// <param name="scDocuments"></param>
        protected virtual IList<long> GetOrderbySearchDocuments(List<double> scQuery, IDictionary<long, IList<double>> scDocuments)
        {
            var scoreDocuments = new Dictionary<long, double>();
             double queryfm = 0;
             foreach (var scq in scQuery)
             {
                 queryfm += scq*scq;
             }
            queryfm = Math.Sqrt(queryfm);
            foreach (var sd in scDocuments)
            {
                double fz = 0;
                double docfm = 0;
                var i = 0;
                foreach (var scq in scQuery)
                {
                    fz += scq*sd.Value[i];
                    docfm += sd.Value[i]*sd.Value[i];
                    i++;
                }
                var w = fz == 0 || docfm == 0 ? 0 : fz / (Math.Sqrt(docfm) * queryfm);
                scoreDocuments.Add(sd.Key,w);
            }
           return scoreDocuments.OrderByDescending(it => it.Value).Select(it => it.Key).ToList();
        }
     
        #endregion

        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="searchQuery"></param>
        /// <param name="result"></param>
        /// <param name="documentIds"></param>
        protected virtual void AddSearchDocuments(StoreIndexInfo storeIndex, SearchQueryInfo searchQuery,SearchResultInfo result, IList<long> documentIds)
        {
            if (searchQuery.PageSize > 0)
                documentIds = documentIds.Skip(searchQuery.PageIndex * searchQuery.PageSize).Take(searchQuery.PageSize).ToList();
            foreach (var documentId in documentIds)
            {
                var document = Documentor.GetInfo(storeIndex, documentId);
                if (document != null)
                    result.Documents.Add(document);
            }
        }

        
        #endregion
        /// <summary>
        /// 刷新缓存
        /// </summary>
        public virtual void Flush(string key)
        {
            lock (KeyLocker)
            {
                Cacher.Remove(key);
            }
        }
        /// <summary>
        /// 删除词典
        /// </summary>
        /// <param name="name"></param>
        /// <param name="terms"></param>
        public virtual bool DeleteWords(string name, IList<TermInfo> terms)
        {
            if (!StoreIndexs.ContainsKey(name))
                return false;
            var storeIndex = StoreIndexs[name];
            if (storeIndex == null)
                return false;
            foreach (var term in terms)
            {
                Worder.Remove(storeIndex, term.Name);
            }
            return true;
        }
        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="name"></param>
        public virtual void Delete(string name)
        {
            if (!StoreIndexs.ContainsKey(name))
                return;
            var storeIndex = StoreIndexs[name];
            if (storeIndex == null)
                return;
            Worder.Clear(storeIndex);
            Documentor.Clear(storeIndex);
        }
       
        /// <summary>
        /// 初始化文档
        /// </summary>
        public virtual void InitlizeDocumentIndex()
        {
            Documentor.InitlizeIndex(StoreIndexs.Values.ToList());
        }

        #endregion


    }
}
