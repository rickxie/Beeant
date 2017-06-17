using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Authority;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Authority
{
    public class RoleAccountDomainService : RealizeDomainService<RoleAccountEntity>
    {
     
        #region 重写验证
  

        /// <summary>
        /// 是否有重复信息添加
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<RoleAccountEntity> infos)
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
        protected virtual bool CompareEntities(RoleAccountEntity info, RoleAccountEntity compare)
        {
            string errorName = null;
            if (info.SaveType == SaveType.Add && info.Account.Id.Equals(compare.Account.Id) && info.Role.Id.Equals(compare.Role.Id))
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
        protected override bool ValidateAdd(RoleAccountEntity info)
        {
            return ValidateRoleExist(info) && ValidateAccountExist(info) && ValidateRoleAccountExist(info);
        }
 
        /// <summary>
        /// 验证UserRole是否已经存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateRoleAccountExist(RoleAccountEntity info)
        {
            if (info.Role != null && info.Role.SaveType == SaveType.Add ||
             info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            var query = new QueryInfo();
            query.Query<RoleAccountEntity>().Where(it => it.Account.Id ==  info.Account.Id
                                               && it.Role.Id == info.Role.Id);
            var infos = Repository.GetEntities<RoleAccountEntity>(query);
            if (infos == null || infos.Count == 0) return true;
            info.AddError("ExistRoleAccount");  
            return false;
        }
        /// <summary>
        /// 验证角色
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateRoleExist(RoleAccountEntity info)
        {
            if (!info.HasSaveProperty(it => it.Role.Id))
                return true;
            if (info.Role != null && info.Role.SaveType == SaveType.Add)
                return true;
            if (info.Role != null && info.Role.Id!=0)
            {
                if (Repository.Get<RoleEntity>(info.Role.Id) != null)
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
        protected virtual bool ValidateAccountExist(RoleAccountEntity info)
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
