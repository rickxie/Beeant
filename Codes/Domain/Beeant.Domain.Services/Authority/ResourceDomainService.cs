using Beeant.Domain.Entities.Authority;
using Winner.Persistence;

namespace Beeant.Domain.Services.Authority
{
    public class ResourceDomainService : RealizeDomainService<ResourceEntity>
    {


        
        #region 重写验证

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(ResourceEntity info)
        {
            return ValidateAbility(info,null); 
        }
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(ResourceEntity info)
        {
            var dataEntity = Repository.Get<ResourceEntity>(info.Id);
            return ValidateAbility(info, dataEntity);
        }
        /// <summary>
        /// 验证功能
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateAbility(ResourceEntity info, ResourceEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Ability.Id))
                return true;
            if (info.Ability != null && info.Ability.SaveType == SaveType.Add)
                return true;
            if (info.Ability != null && info.Ability.Id!=0)
            {
                if (dataEntity != null && dataEntity.Ability != null && dataEntity.Ability.Id == info.Ability.Id)
                    return true;
                if (Repository.Get<AbilityEntity>(info.Ability.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(AbilityEntity).FullName, "NoExist");
            return false;
        }
     
        #endregion

 

       
    }
}
