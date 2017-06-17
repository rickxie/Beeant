using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Authority
{
    public class OwnerAccountDomainService : RealizeDomainService<OwnerAccountEntity>
    {
     
        #region 重写验证
  

        /// <summary>
        /// 是否有重复信息添加
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<OwnerAccountEntity> infos)
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
        protected virtual bool CompareEntities(OwnerAccountEntity info, OwnerAccountEntity compare)
        {
            string errorName = null;
            if (info.SaveType == SaveType.Add && info.Account.Id.Equals(compare.Account.Id) && info.Account.Id.Equals(compare.Account.Id))
                errorName = "RepeatInList";
            if (string.IsNullOrEmpty(errorName)) return true;
            info.AddErrorByName(typeof(BaseEntity).FullName, errorName);
            return false;
        }

        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(OwnerAccountEntity info)
        {
            return ValidateRoleExist(info) && ValidateAccountExist(info) && ValidateAccountRoleExist(info);
        }
 
        /// <summary>
        /// 验证AccountRole是否已经存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccountRoleExist(OwnerAccountEntity info)
        {
            if (info.Account != null && info.Account.SaveType == SaveType.Add ||
             info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            var query = new QueryInfo();
            query.Query<OwnerAccountEntity>().Where(it => it.Account.Id ==  info.Account.Id
                                               && it.Account.Id == info.Account.Id);
            var infos = Repository.GetEntities<OwnerAccountEntity>(query);
            if (infos == null || infos.Count == 0) return true;
            info.AddError("ExistAccountAccount");  
            return false;
        }
        /// <summary>
        /// 验证角色
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateRoleExist(OwnerAccountEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id!=0)
            {
                if (Repository.Get<RoleEntity>(info.Account.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(RoleEntity).FullName, "NoExist");
            return false;
        }
        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccountExist(OwnerAccountEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id!=0)
            {
                if (Repository.Get<AccountEntity>(info.Account.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
            return false;
        }
        #endregion
    }
}
