using System;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;

namespace Beeant.Domain.Entities.Member
{
    public class MessageEntity : BaseEntity<MessageEntity>
    {
        /// <summary>
        /// 所属账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 已读
        /// </summary>
        public bool IsRead { get; set; }



    }
}
