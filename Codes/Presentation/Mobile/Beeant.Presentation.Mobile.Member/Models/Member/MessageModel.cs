using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Member;

namespace Beeant.Presentation.Mobile.Member.Models.Member
{
    public class MessageModel:PagerModel
    {
        /// <summary>
        /// 
        /// </summary>
        public IList<MessageEntity> Messages { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public MessageEntity Message { get; set; }

    }
}
