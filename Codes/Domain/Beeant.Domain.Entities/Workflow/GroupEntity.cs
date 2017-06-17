using System.Collections.Generic;

namespace Beeant.Domain.Entities.Workflow
{
    /// <summary>
    /// 组
    /// </summary>
    public class GroupEntity: BaseEntity<GroupEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
 
        /// <summary>
        /// 组织流程
        /// </summary>
        public IList<GroupFlowEntity> GroupFlows { get; set; }
        /// <summary>
        /// 授权用户
        /// </summary>
        public IList<GroupAccountEntity> GroupAccounts { get; set; }
    }
}
