using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Search;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Search
{
    public class SimilarDomainService : RealizeDomainService<SimilarEntity>
    {
 

        #region 重写验证
     

        /// <summary>
        /// 是否有重复信息添加
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<SimilarEntity> infos)
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
        protected virtual bool CompareEntities(SimilarEntity info, SimilarEntity compare)
        {
            string errorName = null;
            if (info.Name.ToLower().Equals(compare.Name.ToLower()) && info.Word.Id.Equals(compare.Word.Id))
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
        protected override bool ValidateAdd(SimilarEntity info)
        {
            var rev = ValidateWordExist(info);
            if (rev) rev = ValidateExist(info);
            return rev;
        }

        /// <summary>
        /// 验证词是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateWordExist(SimilarEntity info)
        {
            if (!info.HasSaveProperty(it => it.Word.Id))
                return true;
            if (info.Word != null && info.Word.SaveType == SaveType.Add)
                return true;
            if (info.Word != null && info.Word.Id!=0)
            {

                if (Repository.Get<WordEntity>(info.Word.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(WordEntity).FullName, "NoExist");
            return false;
        }
        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(SimilarEntity info)
        {
            var query = new QueryInfo();
            query.Query<SimilarEntity>().Where(it => it.Name == info.Name && it.Word.Id==info.Word.Id);
            var infos = Repository.GetEntities<SimilarEntity>(query);
            if (infos != null && infos.Count == 0)
                return true;
            info.AddError("Exist");
            return false;
        }

        #endregion


    }
}
