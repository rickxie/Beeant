using System;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Product
{


    [Serializable]
    public class InqueryEntity : BaseEntity<InqueryEntity>
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public GoodsEntity Goods { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 问题类型
        /// </summary>
        public string Tag { get; set; }
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
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 回复名称
        /// </summary>
        public string IsReplyName
        {
            get { return this.GetStatusName(IsReply); }
        }
        /// <summary>
        /// 回复名称
        /// </summary>
        public string IsShowName
        {
            get { return this.GetShowName(IsShow); }
        }

    }

}
