using System;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Workflow
{
    [Serializable]
    public class MessageEntity : BaseEntity<MessageEntity>
    {
        /// <summary>
        /// 工作流Id
        /// </summary>
        public FlowEntity Flow { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public LevelEntity Level { get; set; }
        /// <summary>
        /// 单据Id
        /// </summary>
        public long DataId { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public NodeEntity Node { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 处理人Id
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 任务
        /// </summary>
        public TaskEntity Task { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 实体
        /// </summary>
        public BaseEntity Data { get; set; }
    }
}
