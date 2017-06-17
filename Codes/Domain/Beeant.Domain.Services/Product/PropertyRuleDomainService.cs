using Beeant.Domain.Entities.Product;
using Winner.Persistence;

namespace Beeant.Domain.Services.Product
{
    public class PropertyRuleDomainService : RealizeDomainService<PropertyRuleEntity>
    {
        #region 重写验证
 

        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PropertyRuleEntity info)
        {
            var rev = ValidatePropertyExist(info);
            if (rev) rev = ValidateRuleExist(info);
            return rev;
        }
        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(PropertyRuleEntity info)
        {
            var rev = ValidatePropertyExist(info);
            if (rev) rev = ValidateRuleExist(info);
            return rev;
        }
  
        
        /// <summary>
        /// 验证角色是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateRuleExist(PropertyRuleEntity info)
        {
            if (!info.HasSaveProperty(it => it.Rule.Id))
                return true;
            if (info.Rule != null && info.Rule.SaveType == SaveType.Add)
                return true;
            if (info.Rule != null && info.Rule.Id!=0)
            {

                if (Repository.Get<RuleEntity>(info.Rule.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(RuleEntity).FullName, "NoExist");
            return false;
        }
        /// <summary>
        /// 验证功能是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidatePropertyExist(PropertyRuleEntity info)
        {
            if (!info.HasSaveProperty(it => it.Property.Id))
                return true;
            if (info.Property != null && info.Property.SaveType == SaveType.Add)
                return true;
            if (info.Rule != null && info.Property.Id!=0)
            {
                if (Repository.Get<PropertyEntity>(info.Property.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(PropertyEntity).FullName, "NoExist");
            return false;
        }
        #endregion
    }
}
