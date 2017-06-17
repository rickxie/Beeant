using System;

namespace Beeant.Domain.Entities.Workflow
{
    [Serializable]
    public class PropertyEntity : BaseEntity<PropertyEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 节点
        /// </summary>
        public NodeEntity Node { get; set; }
      
    }
}
