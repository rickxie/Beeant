using System.Collections.Generic;
using System.Linq;
using Winner.Persistence;
using Winner.Persistence.Linq;
using WordEntity = Beeant.Domain.Entities.Search.WordEntity;

namespace Beeant.Domain.Services.Search
{
    public class WordDomainService : RealizeDomainService<WordEntity>
    {
      
        #region 重写验证
   

        /// <summary>
        /// 是否有重复信息添加
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<WordEntity> infos)
        {
            bool rev = true;
            for (int i = 0; i < infos.Count; i++)
                for (int k = i + 1; k < infos.Count; k++)
                {
                    rev &= CompareEntities(infos[i], infos[k]);
                }
            return rev;
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="info"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        protected virtual bool CompareEntities(WordEntity info, WordEntity compare)
        {
            string errorName = null;
            if (info.Name.ToLower().Equals(compare.Name.ToLower()))
                errorName = "RepeatNameInList";
            if (string.IsNullOrEmpty(errorName)) return true;
            info.AddError(errorName);
            return false;
        }
  

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(WordEntity info)
        {
            return ValidateExist(info);
        }

        /// <summary>
        ///  验证是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(WordEntity info)
        {
            var query = new QueryInfo();
            query.Query<WordEntity>().Where(it => it.Name == info.Name);
            var infos = Repository.GetEntities<WordEntity>(query);
            if (infos != null && infos.Count == 0)
                return true;
            info.AddError("Exist");
            return false;
        }

        #endregion

       
    }
}
