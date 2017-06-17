using System;
using System.Collections.Generic;
using System.Linq;
using Winner.Search.Store;

namespace Winner.Search.Word
{
    public class Worder : IWorder
    {
        #region 属性
        protected string IndexName = @"{0}\Word\{1}.inx";
        protected string RootName = @"{0}\Word\";


        /// <summary>
        /// 存储实例
        /// </summary>
        public IStorer Storer { get; set; }

        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public Worder()
        {
        }

        /// <summary>
        ///  排序实例，存储器实例,数据实例,关系词实例,拼音实例
        /// </summary>
        /// <param name="storer"></param>
        public Worder(IStorer storer)
        {
            Storer = storer;
        }
        #endregion

        #region 接口的实现
        #region 插入

        public IDictionary<string, IList<WordInfo>> WordDictionarties; 
        /// <summary>
        /// 加载词库
        /// </summary>
        /// <param name="storeIndex"></param>
        public virtual void Load(StoreIndexInfo storeIndex)
        {
            WordDictionarties = new Dictionary<string, IList<WordInfo>>();
        }
        /// <summary>
        /// 写入词库
        /// </summary>
        /// <param name="storeIndex"></param>
        public virtual void Write(StoreIndexInfo storeIndex)
        {
            foreach (var word in WordDictionarties)
            {
                foreach (var wd in word.Value)
                {
                    SetWordWeight(storeIndex,wd);
                    wd.Inversions =
                        wd.Inversions.OrderByDescending(it => it.Weight).Take(storeIndex.TopDocumentCount).ToList();
                }
                Storer.Save(word.Key, word.Value);
            }
            WordDictionarties = null;
        }
        /// <summary>
        /// 计算权重值，密度越接近设置的值，权重越高
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        protected virtual void SetWordWeight(StoreIndexInfo storeIndex, WordInfo word)
        {
            foreach (var inversion in word.Inversions)
            {
                double pr = 0;
                var defaultPercentage = storeIndex.StoreSequences == null
                                            ? (double) 1/inversion.Feilds.Count
                                            : 1 - storeIndex.StoreSequences.Sum(it => it.Percentage);
                foreach (var frequency in inversion.Feilds)
                {
                    var storeSequence = storeIndex.StoreSequences == null
                                            ? null
                                            : storeIndex.StoreSequences.FirstOrDefault(
                                                it => it.Index == frequency.FeildIndex);
                    if (storeSequence != null)
                    {
                        pr += frequency.Frequency * Math.Log(storeIndex.StoreDocument.DataCount / (word.DocumentCount / (double)storeIndex.StoreDocument.DataCount)) *
                            (1 - Math.Abs(frequency.Frequency - storeSequence.Density)) * storeSequence.Percentage;
                    }
                    else
                    {
                        pr += (frequency.Frequency * Math.Log(storeIndex.StoreDocument.DataCount / (word.DocumentCount / (double)storeIndex.StoreDocument.DataCount))) * defaultPercentage;
                    }
                }
                inversion.Weight = pr;
            }

        }
        /// <summary>
        /// 插入词
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public virtual bool Insert(StoreIndexInfo storeIndex, WordInfo word)
        {
            if (storeIndex == null || word == null)
                return false;
            string indexName = GetStoreName(storeIndex.Name, word.Name);
            if (!WordDictionarties.ContainsKey(indexName))
            {
                var words = Storer.Read<IList<WordInfo>>(indexName);
                words = words ?? new List<WordInfo>();
                WordDictionarties.Add(indexName, words);
            }
            var infos = WordDictionarties[indexName];
            return InsertWord(storeIndex,word, infos);
        }

        /// <summary>
        /// 二分查找到词并插入词
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="word"></param>
        /// <param name="infos"></param>
        protected virtual bool InsertWord(StoreIndexInfo storeIndex, WordInfo word, IList<WordInfo> infos)
        {
            if (infos == null) return false;
            int low = 0, high = infos.Count - 1;
            while (low <= high)
            {
                int mid = low + ((high - low) / 2);
                if (infos[mid].Name.Equals(word.Name))
                {
                    infos[mid].DocumentCount += word.DocumentCount;
                    foreach (var inversion in word.Inversions)
                    {
                        infos[mid].Inversions.Add(inversion);
                    }
                    return true;
                }
                if (infos[mid].Name.CompareTo(word.Name) > 0)
                    high = mid - 1;
                else
                    low = mid + 1;
            }
            infos.Insert(low, word);
            return true;
        }
  
        #endregion

        /// <summary>
        /// 更新词
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public virtual bool Update(StoreIndexInfo storeIndex, WordInfo word)
        {
            return Remove(storeIndex, word.Name) && Insert(storeIndex, word);
        }
        /// <summary>
        /// 移除索引库的词
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public virtual bool Remove(StoreIndexInfo storeIndex, string word)
        {
            if (string.IsNullOrEmpty(word))
                return false;
            string indexName = GetStoreName(storeIndex.Name, word);
            var words = Storer.Read<IList<WordInfo>>(indexName);
            return RemoveWord(words, word, indexName);
        }
        /// <summary>
        /// 移除词
        /// </summary>
        /// <param name="words"></param>
        /// <param name="word"></param>
        /// <param name="indexName"></param>
        /// <returns></returns>
        protected virtual bool RemoveWord(IList<WordInfo> words, string word, string indexName)
        {
            if (words == null)
                return false;
            int i = GetWordInfoIndex(words, word);
            if (i != -1)
            {
                words.RemoveAt(i);
                Storer.Save(indexName, words);
            }
            return true;
        }

        /// <summary>
        /// 得到词信息
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public virtual WordInfo GetInfo(StoreIndexInfo storeIndex, string word)
        {
            string indexname = GetStoreName(storeIndex.Name, word);
            var words = Storer.Read<IList<WordInfo>>(indexname);
            if (words == null)
                return null;
            int i = GetWordInfoIndex(words, word);
            if (i != -1)
                return words[i];
            return null;
        }

     
        /// <summary>
        /// 清空词库
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <returns></returns>
        public virtual bool Clear(StoreIndexInfo storeIndex)
        {
            var names = Storer.GetNames(string.Format(RootName, storeIndex.Name));
            if (names == null) return true;
            foreach (var indexName in names)
            {
                Storer.Delete(indexName);
            }
            return true;
        }


        #endregion
 
        #region 得到词库信息
        /// <summary>
        /// 根据词得到索引下标
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        protected virtual int GetWordInfoIndex(IList<WordInfo> infos, string word)
        {
            if (infos == null || infos.Count == 0) return -1;
            int low = 0, high = infos.Count - 1;
            while (low <= high)
            {
                int mid = low + ((high - low) / 2);
                if (infos[mid].Name.Equals(word))
                    return mid;
                if (infos[mid].Name.CompareTo(word) > 0)
                    high = mid - 1;
                else
                    low = mid + 1;
            }
            return -1;
        }

        /// <summary>
        /// 得到索引名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public virtual string GetStoreName(string name, string word)
        {
            return string.Format(IndexName, name, word.Length);
        }
 
     
        #endregion



    }
}
