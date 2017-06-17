using System;


namespace Beeant.Domain.Entities.Authority
{
  
    [Serializable]
    public class RoleAbilityEntity : BaseEntity<RoleAbilityEntity>
    {
    

        /// <summary>
        ///角色
        /// </summary>
        public RoleEntity Role{get;set;}
       
        /// <summary>
        ///功能
        /// </summary>
        public AbilityEntity Ability { get; set; }
      
      
    }
    
}
