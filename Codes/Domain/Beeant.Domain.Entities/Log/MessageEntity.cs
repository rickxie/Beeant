using System;

namespace Beeant.Domain.Entities.Log
{
    [Serializable]
    public class MessageEntity : BaseEntity<MessageEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public MessageType Type { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string FromAddress { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string ToAddress { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public string Number { get; set; }
       
    }
}
