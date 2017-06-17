using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Authority
{
   
    [Serializable]
    public class SubsystemEntity : BaseEntity<SubsystemEntity>
    {
     
    
        /// <summary>
        ///名称 
        /// </summary>
        public string Name{get;set;}
        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 菜单
        /// </summary>
        public IList<MenuEntity> Memus { get; set; }

    }
 
}
