using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Caching;
using Beeant.Domain.Entities.Search;
using Winner;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Winner.Search.Analysis;

namespace Beeant.Repository.Services.Search
{
    public class WordStandardAnalyzer : StandardAnalyzer
    {
        public WordStandardAnalyzer()
        {
            var timer = new Timer(1000*60*60);
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
            timer.Start();
        }
        /// <summary>
        /// 定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            LoadDictionaries();
        }

        public bool IsInitDictionaries=false;
        /// <summary>
        /// 加载词库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
       public override IList<TermInfo> Resolve(string input)
       {
           if (!IsInitDictionaries)
           {
               LoadDictionaries();
               IsInitDictionaries = true;
           } 
           return base.Resolve(input);
       }
        /// <summary>
        /// 加载词典
        /// </summary>
        protected virtual  void LoadDictionaries()
        {
            HttpRuntime.Cache.Insert(Guid.NewGuid().ToString(), "analyzer", null, DateTime.Now.AddDays(1), TimeSpan.Zero, CacheItemPriority.High, CacheRemoved);
            var words = GetWords();
            if(words==null)
                return;
            var mainDictionaries = new List<string>();
            var stopDictionaries = new List<string>();
            var transformDictionaries = new List<KeyValuePair<string, string>>();
            mainDictionaries.AddRange(words.Select(it => it.Name.Trim()));
            mainDictionaries.AddRange(words.Where(it => it.Pinyin!=null && !string.IsNullOrEmpty(it.Pinyin.Trim()) && !it.Pinyin.Contains("(")).Select(it => it.Pinyin));
            stopDictionaries.AddRange(words.Where(it => it.IsForbid).Select(it => it.Name.Trim()));
            transformDictionaries.AddRange(words.Where(it => it.Original != null && !string.IsNullOrEmpty(it.Original.Trim())).Select(it => new KeyValuePair<string, string>(it.Original.Trim().ToLower(), it.Name.Trim().ToLower())));
            transformDictionaries.AddRange(words.Where(it => !string.IsNullOrEmpty(it.Pinyin.Trim()) && !it.Pinyin.Contains("(")).Select(it => new KeyValuePair<string, string>(it.Pinyin.Trim().ToLower(), it.Name.Trim().ToLower())));
            MainDictionaries = mainDictionaries.Where(it => !string.IsNullOrEmpty(it)).Select(it => it.ToLower()).OrderBy(it => it).Distinct().ToArray();
            StopDictionaries = stopDictionaries.Where(it => !string.IsNullOrEmpty(it)).Select(it => it.ToLower()).OrderBy(it => it).Distinct().ToArray();
            TransformDictionaries = transformDictionaries.OrderBy(it => it.Key).Distinct().ToArray();
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="reason"></param>
        protected virtual void CacheRemoved(string key, object value, CacheItemRemovedReason reason)
        {
            LoadDictionaries();
        }

        /// <summary>
        /// 得到同步的词
        /// </summary>
        /// <returns></returns>
        protected virtual IList<WordEntity> GetWords()
        {
            var query = new QueryInfo();
            query.Query<WordEntity>().Where(it=>it.Count>1);
            return Creator.Get<IContext>().GetInfos<IList<WordEntity>>(query);
        }


    }
}
