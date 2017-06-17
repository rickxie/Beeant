using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.International.Converters.PinYinConverter;
using Beeant.Domain.Entities.Search;
using Beeant.Domain.Services;

using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Search
{
    public class KeyToWordEventApplicationService : KeyApplicationService, IJobApplicationService
    {
        /// <summary>
        /// 词库服务
        /// </summary>
        public IDomainService WordDomainService { get; set; }
        public static bool IsExecte;

        #region 执行服务

        /// <summary>
        /// 执行服务
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual bool Execute(object[] args)
        {
            if (args == null || IsExecte) return false;
            IsExecte = true;
            for (int i = 0; ; i++)
            {
                var infos = GetKeyGroups(i);
                if (infos == null || infos.Count == 0)
                    break;
                UpdateKeyToWord(infos);
                if (infos.Count < 1000)
                    break;
            }
            IsExecte = false;
            return true;
        }

        /// <summary>
        /// 得到词库
        /// </summary>
        /// <returns></returns>
        protected virtual IList<KeyEntity> GetKeyGroups(int pageIndex)
        {
            var query = new QueryInfo();
            query.Query<KeyEntity>();
            query.SetPageIndex(pageIndex)
                .SetPageSize(1000).Query<KeyEntity>().Where(it => it.InsertTime >= DateTime.Now.Date).Select(it => new object[] { it.Name });
            return Repository.GetEntities<KeyEntity>(query);
        }

        #endregion

        #region 添加词库接口

        /// <summary>
        /// 添加词库到搜索库
        /// </summary>
        /// <param name="infos"></param>
        protected virtual bool UpdateKeyToWord(IList<KeyEntity> infos)
        {
            if (infos == null || infos.Count == 0) return false;
            var words = GetWords(infos.Select(it => it.Name).ToArray());
            foreach (var info in infos)
            {
                SetWordSaveType(words, info);
            }
            IList<WordEntity> wds = words.Select(it => it.Value).Where(it => it.SaveType != SaveType.None).ToList();
            var unitofworks = WordDomainService.Handle(wds);
            return Commit(unitofworks);

        }

        /// <summary>
        /// 设置词存储类型
        /// </summary>
        /// <param name="words"></param>
        /// <param name="info"></param>
        protected virtual void SetWordSaveType(Dictionary<string, WordEntity> words, KeyEntity info)
        {
            if (words.ContainsKey(info.Name))
            {
                var word = words[info.Name];
                if (word.Count == long.MaxValue)
                    return;
                word.Count++;
                word.SaveType = SaveType.Modify;
                word.SetProperty(it => it.Count);
            }
            else
            {
                var word = new WordEntity
                    {
                        Name = info.Name,
                        Pinyin = GetPinyin(info.Name),
                        Count = 1,
                        IsForbid = false,
                        SaveType = SaveType.Add
                    };
                words.Add(info.Name, word);
            }
        }

        /// <summary>
        /// 得到已经存在的词
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        protected virtual Dictionary<string, WordEntity> GetWords(string[] names)
        {
            var query = new QueryInfo();
            query.Query<WordEntity>().Where(it => names.Contains(it.Name));
            var infos = Repository.GetEntities<WordEntity>(query);
            var words = new Dictionary<string, WordEntity>();
            foreach (var info in infos)
            {
                words.Add(info.Name, info);
            }
            return words;
        }
        /// <summary>
        /// 得到拼音
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual string GetPinyin(string input)
        {
            var builder = new StringBuilder();
            foreach (char c in input)
            {
                var chineseChar = GetChineseChar(c);

                if (chineseChar == null || chineseChar.PinyinCount <= 0)
                {
                    builder.AppendFormat("'{0}", c);
                    continue;
                }
                var values = new List<string>();
                foreach (var pinyin in chineseChar.Pinyins)
                {
                    if (string.IsNullOrEmpty(pinyin)) break;
                    var value = pinyin.Substring(0, pinyin.Length - 1).ToLower();
                    if (values.Contains(value)) continue;
                    values.Add(value);
                }
                if (values.Count == 1)
                    builder.AppendFormat("'{0}", values[0]);
                else if (values.Count > 1)
                {
                    builder.Append("'(");
                    foreach (var value in values)
                    {
                        builder.AppendFormat("{0}|", value);
                    }
                    builder.Remove(builder.Length - 1, 1);
                    builder.Append(")");
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 得到中文
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected virtual ChineseChar GetChineseChar(char c)
        {
            try
            {
                return new ChineseChar(c);

            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
