using System;
using System.Collections.Generic;


namespace Beeant.Domain.Entities.Authority
{

  
    [Serializable]
    public class AbilityEntity : BaseEntity<AbilityEntity>
    {


        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否验证
        /// </summary>
        public bool IsVerify { get; set; }
        /// <summary>
        /// 是否验证名称
        /// </summary>
        public string IsVerifyName
        {
            get { return this.GetVerifyName(IsVerify); }
        }

        /// <summary>
        /// 菜单
        /// </summary>
        public MenuEntity Menu { get; set; }
        /// <summary>
        /// 资源
        /// </summary>
        public IList<ResourceEntity> Resources { get; set; }
        /// <summary>
        /// 功能角色
        /// </summary>
        public IList<RoleAbilityEntity> RoleAbilities { get; set; }
      
    }
    
}
