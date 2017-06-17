namespace Beeant.Domain.Entities.Workflow
{
    /// <summary>
    /// 组
    /// </summary>
    public class GroupFlowEntity : BaseEntity<GroupFlowEntity>
    {
        /// <summary>
        /// 组
        /// </summary>
        public GroupEntity Group { get; set; }
        /// <summary>
        /// 流程
        /// </summary>
        public FlowEntity Flow { get; set; }

 
    }
}
