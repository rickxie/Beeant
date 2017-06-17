using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Authority;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Authority
{
    public class RoleAbilityDomainService : RealizeDomainService<RoleAbilityEntity>
    {
      

        /// <summary>
        /// 是否有重复信息添加
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<RoleAbilityEntity> infos)
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
        protected virtual bool CompareEntities(RoleAbilityEntity info, RoleAbilityEntity compare)
        {
            //string errorName = null;
            //if (info.SaveType==SaveType.Add && info.Ability.Id.Equals(compare.Ability.Id) && info.Role.Id.Equals(compare.Role.Id))
            //    errorName = "RepeatInList";
            //if (string.IsNullOrEmpty(errorName)) return true;
            //info.AddErrorByName(typeof(BaseEntity).FullName, errorName);
            //return false;
            return true;
        }
   
        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(RoleAbilityEntity info)
        {
            return ValidateAbilityExist(info) && ValidateRoleExist(info) && ValidateRoleAbilityExist(info);
        }



        /// <summary>
        /// 验证RoleAbility是否已经存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateRoleAbilityExist(RoleAbilityEntity info)
        {
            if (info.Role != null && info.Role.SaveType == SaveType.Add ||
                info.Ability != null && info.Ability.SaveType == SaveType.Add)
                return true;
            var query = new QueryInfo();
            query.Query<RoleAbilityEntity>().Where(it => it.Ability.Id == info.Ability.Id
                                                      && it.Role.Id == info.Role.Id);
            var infos = Repository.GetEntities<RoleAbilityEntity>(query);
            if (infos == null || infos.Count == 0) return true;
            info.AddError("ExistRoleAbility");
            return false;
        }

        /// <summary>
        /// 验证角色类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateRoleExist(RoleAbilityEntity info)
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
        /// 验证功能类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAbilityExist(RoleAbilityEntity info)
        {
            if (!info.HasSaveProperty(it => it.Ability.Id))
                return true;
            if (info.Ability != null && info.Ability.SaveType == SaveType.Add)
                return true;
            if (info.Ability != null && info.Ability.Id!=0)
            {
                if (Repository.Get<AbilityEntity>(info.Ability.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(AbilityEntity).FullName, "NoExist");
            return false;
        }
   
    }
}
