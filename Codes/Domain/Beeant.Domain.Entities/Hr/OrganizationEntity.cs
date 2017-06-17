using System;


namespace Beeant.Domain.Entities.Hr
{

    [Serializable]
    public class OrganizationEntity : BaseEntity<OrganizationEntity>
    {
    
        /// <summary>
        /// 
        /// </summary>
        public HrEntity Hr { get; set; }
 
       
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }
        
    }
    
}
