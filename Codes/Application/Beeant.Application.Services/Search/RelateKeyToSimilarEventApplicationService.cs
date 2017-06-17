using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Search;
using Beeant.Domain.Services;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Search
{
    public class RelateKeyToSimilarEventApplicationService : RelateKeyApplicationService, IJobApplicationService
    {
        /// <summary>
        /// 词库服务
        /// </summary>
        public IDomainService SimilarDomainService { get; set; }
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
                var infos = GetRelateKeyGroups(i);
                if (infos == null || infos.Count == 0)
                    break;
                UpdateRelateKeyToSimilar(infos);
                if (infos.Count < 1000)
                    break;
            }
            IsExecte = false;
            return true;
        }

        /// <summary>
        /// 得到词库
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        protected virtual IList<RelateKeyEntity> GetRelateKeyGroups(int pageIndex)
        {
            var query = new QueryInfo();
            query.Query<RelateKeyEntity>().Where(it => it.InsertTime >= DateTime.Now.Date).GroupBy(it => new object[] { it.Name, it.KeyName });
            query.SetPageIndex(pageIndex)
                .SetPageSize(1000).Query<RelateKeyEntity>().Select(it => new object[] { it.Name });
            return Repository.GetEntities<RelateKeyEntity>(query);
        }

        #endregion

        #region 添加词库接口

        /// <summary>
        /// 添加词库到搜索库
        /// </summary>
        /// <param name="infos"></param>
        protected virtual bool UpdateRelateKeyToSimilar(IList<RelateKeyEntity> infos)
        {
            if (infos == null || infos.Count == 0) return false;
            var words = GetWords(infos.Select(it => it.KeyName).ToArray());
            foreach (var info in infos)
            {
                SetSimilarSaveType(words, info);
            }
            IList<SimilarEntity> sims = new List<SimilarEntity>();
            foreach (var word in words)
            {
                foreach (var similar in word.Value)
                {
                    if (similar.Value.SaveType != SaveType.None)
                        sims.Add(similar.Value);
                }
            }
            var unitofworks = SimilarDomainService.Handle(sims);
            return Commit(unitofworks);

        }

        /// <summary>
        /// 设置词存储类型
        /// </summary>
        /// <param name="words"></param>
        /// <param name="info"></param>
        protected virtual void SetSimilarSaveType(IDictionary<string, IDictionary<string, SimilarEntity>> words, RelateKeyEntity info)
        {
            if (!words.ContainsKey(info.KeyName))
                return;
            var similars = words[info.KeyName];
            if (similars.ContainsKey(info.Name))
            {
                var similar = similars[info.Name];
                if (similar.Count == int.MaxValue)
                    return;
                similar.Count++;
                similar.SaveType = SaveType.Modify;
                similar.SetProperty(it => it.Count);
            }
            else
            {
                var word = new SimilarEntity
                {
                    Name = info.Name,
                    Count = 1,
                    SaveType = SaveType.Add
                };
                similars.Add(info.Name, word);
            }
        }

        /// <summary>
        /// 得到已经存在的词
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        protected virtual Dictionary<string, IDictionary<string, SimilarEntity>> GetWords(string[] names)
        {
            var query = new QueryInfo();
            query.Query<WordEntity>().Where(it => names.Contains(it.Name)).Select(it => new object[] { it.Id, it.Name, it.Similars.Select(s => s) });
            var infos = Repository.GetEntities<WordEntity>(query);
            var words = new Dictionary<string, IDictionary<string, SimilarEntity>>();
            foreach (var info in infos)
            {
                var similars = new Dictionary<string, SimilarEntity>();
                if (info.Similars != null)
                {
                    foreach (var similar in info.Similars)
                    {
                        similars.Add(similar.Name, similar);
                    }
                }
                words.Add(info.Name, similars);
            }
            return words;
        }

        #endregion
    }
}
