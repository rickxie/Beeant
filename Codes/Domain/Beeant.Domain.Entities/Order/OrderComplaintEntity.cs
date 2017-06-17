using System;

namespace Beeant.Domain.Entities.Order
{


    [Serializable]
    public class OrderComplaintEntity : BaseEntity<OrderComplaintEntity>
    {
        /// <summary>
        /// 总订单标识Id
        /// </summary>
        public OrderEntity Order { get; set; }
        /// <summary>
        /// 问题
        /// </summary>
        public string Question { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// 回答时间
        /// </summary>
        public DateTime AnswerTime { get; set; }

        /// <summary>
        /// 是否回复
        /// </summary>
        public bool IsReply { get; set; }
        /// <summary>
        /// 评价类型
        /// </summary>
        public OrderComplaintType Type { get; set; }
 
        /// <summary>
        /// 回复名称
        /// </summary>
        public string IsReplyName
        {
            get { return this.GetReplayName(IsReply); }
        }
        /// <summary>
        /// 评价类型
        /// </summary>
        public string TypeName
        {
            get { return Type.GetName(); }
        }
  
    }

}
